using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

namespace Rendering.RendererFeatures
{
    internal class ColorBlitRendererFeature : ScriptableRendererFeature
    {
        public Shader shader;

        [Range(0, 1)]
        public float intensity = 1.0f;
        public Color blitColor = Color.white;

        private Material _material;
        private ColorBlitPass _renderPass;

        public override void Create()
        {
            shader = Shader.Find("URP/Custom/ColorBlit");
            _material = CoreUtils.CreateEngineMaterial(shader);
            _renderPass = new ColorBlitPass(_material);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (_material == null || renderingData.cameraData.cameraType != CameraType.Game)
                return;

            _renderPass.SetIntensity(intensity);
            _renderPass.SetBlitColor(blitColor);
            renderer.EnqueuePass(_renderPass);
        }

        protected override void Dispose(bool disposing)
        {
            CoreUtils.Destroy(_material);
        }
    }

    public class ColorBlitPass : ScriptableRenderPass
    {
        private static readonly int k_IntensityID = Shader.PropertyToID("_intensity");
        private static readonly int k_BlitColorID = Shader.PropertyToID("_blitColor");
        private const string k_PassName = "ColorBlitPass";

        private Material _material;
        private Color _blitColor;
        private float _intensity;

        public ColorBlitPass(Material material)
        {
            _material= material;
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public void SetIntensity(float intensity) => _intensity = intensity;
        public void SetBlitColor(Color blitColor) => _blitColor = blitColor;

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var resourceData = frameData.Get<UniversalResourceData>();

            if (resourceData.isActiveTargetBackBuffer)
            {
                Debug.LogError($"Skipping render pass. ColorBlitRendererFeature requires an intermediate ColorTexture, we can't use the BackBuffer as a texture input.");
                return;
            }

            var source = resourceData.activeColorTexture;

            var destinationDesc = renderGraph.GetTextureDesc(source);
            destinationDesc.name = $"CameraColor-{k_PassName}";
            destinationDesc.clearBuffer = false;
            destinationDesc.depthBufferBits = 0;

            TextureHandle destination = renderGraph.CreateTexture(destinationDesc);

            RenderGraphUtils.BlitMaterialParameters param = new(source, destination, _material, 0);
            param.material.SetFloat(k_IntensityID, _intensity);
            param.material.SetColor(k_BlitColorID, _blitColor);
            renderGraph.AddBlitPass(param, passName: k_PassName);

            resourceData.cameraColor = destination;
        }
    }
}
