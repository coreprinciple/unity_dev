using EasyButtons;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using System.IO;

namespace Rendering.RendererFeatures
{
    public class BakeGBufferRendererFeature : ScriptableRendererFeature
    {
        public Shader shader;
        public RenderTexture albedoTexture;
        public RenderTexture normalTexture;

        private Material _material;
        private BakeGBufferPass _renderPass;

        public override void Create()
        {
            shader = Shader.Find("URP/Custom/GBufferBake");
            _material = CoreUtils.CreateEngineMaterial(shader);
            _renderPass = new BakeGBufferPass(_material);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (_material == null || renderingData.cameraData.cameraType != CameraType.Game)
                return;

            _renderPass.SetRenderTexture(albedoTexture, normalTexture);
            renderer.EnqueuePass(_renderPass);
        }

        [Button("Record GBuffer")]
        public void SetRecordGBuffeR()
        {
            _renderPass.SetRecordGBuffer();
        }

        protected override void Dispose(bool disposing)
        {
            CoreUtils.Destroy(_material);
        }
    }

    public class BakeGBufferPass : ScriptableRenderPass
    {
        private const string k_PassName = "BakeGBufferPass";

        private class PassData
        {
            internal TextureHandle cameraAlbedoTexture;
            internal TextureHandle cameraNormalsTexture;
            internal RenderTexture albedoTextureBuffer;
            internal RenderTexture normalTextureBuffer;
        }

        private RenderTexture _albedoTexture;
        private RenderTexture _normalTexture;
        private Material _material;

        private bool _recordGBuffer;

        public BakeGBufferPass(Material material)
        {
            _material = material;
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public void SetRenderTexture(RenderTexture albedoTexture, RenderTexture normalTexture)
        {
            _albedoTexture = albedoTexture;
            _normalTexture = normalTexture;
        }

        public void SetRecordGBuffer()
        {
            _recordGBuffer = true;
        }

        static void ExecutePass(PassData passData, UnsafeGraphContext context, Material material)
        {
            CommandBuffer unsafeAlbedoCB = CommandBufferHelpers.GetNativeCommandBuffer(context.cmd);

            context.cmd.SetRenderTarget(passData.albedoTextureBuffer);
            Blitter.BlitTexture(unsafeAlbedoCB, passData.cameraAlbedoTexture, new Vector4(1, 1, 0, 0), 0, false);

            context.cmd.SetRenderTarget(passData.normalTextureBuffer);
            Blitter.BlitTexture(unsafeAlbedoCB, passData.cameraNormalsTexture, new Vector4(1, 1, 0, 0), material, 0);

            SaveAsTexture2D(passData.albedoTextureBuffer, "Albedo", passData.albedoTextureBuffer.width);
            SaveAsTexture2D(passData.normalTextureBuffer, "Normal", passData.normalTextureBuffer.width);
        }

        private static void SaveAsTexture2D(RenderTexture renderTexture, string fileName, int width)
        {
            int cubemapSize = Mathf.Min(Mathf.NextPowerOfTwo(width), 8192);

            Texture2D output = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            output.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

            byte[] bytes = I360Render.InsertXMPIntoTexture2D_PNG(output);
            if (bytes != null)
            {
                string path = Path.Combine(Application.dataPath, $"{fileName}" + ".png");
                File.WriteAllBytes(path, bytes);
                //Debug.Log("360 render saved to " + path);
            }
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            if (_recordGBuffer == false)
                return;

            _recordGBuffer = false;

            //var normalSource = resourceData.cameraNormalsTexture;
            //var normalDestinationDesc = renderGraph.GetTextureDesc(normalSource);
            //normalDestinationDesc.name = $"CameraNormal-{k_PassName}";
            //normalDestinationDesc.clearBuffer = false;
            //normalDestinationDesc.depthBufferBits = 0;

            //TextureHandle normal = renderGraph.CreateTexture(normalDestinationDesc);
            //RenderGraphUtils.BlitMaterialParameters param1 = new(normalSource, normal, _material, 0);
            //renderGraph.AddBlitPass(param1, passName: "BakeGBufferPass_Normal");

            using (var builder = renderGraph.AddUnsafePass<PassData>("Draw GBuffer", out var passData))
            {
                var resourceData = frameData.Get<UniversalResourceData>();

                if (resourceData.isActiveTargetBackBuffer)
                {
                    Debug.LogError($"Skipping render pass. BakeGBufferRendererFeature requires an intermediate ColorTexture, we can't use the BackBuffer as a texture input.");
                    return;
                }

                ConfigureInput(ScriptableRenderPassInput.Normal);

                passData.albedoTextureBuffer = _albedoTexture;
                passData.normalTextureBuffer = _normalTexture;

                passData.cameraAlbedoTexture = resourceData.activeColorTexture;
                builder.UseTexture(passData.cameraAlbedoTexture);

                passData.cameraNormalsTexture = resourceData.cameraNormalsTexture;
                builder.UseTexture(passData.cameraNormalsTexture);

                builder.AllowPassCulling(false);
                builder.SetRenderFunc((PassData data, UnsafeGraphContext context) => ExecutePass(data, context, _material));
            }
        }
    }

}
