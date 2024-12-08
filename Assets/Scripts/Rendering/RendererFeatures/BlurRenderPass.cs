using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;
using static Rendering.RendererFeatures.BlurRendererFeature;

namespace Rendering.RendererFeatures
{
    public class BlurRenderPass : ScriptableRenderPass
    {
        private static readonly int horizontalBlurId = Shader.PropertyToID("_HorizontalBlur");
        private static readonly int verticalBlurId = Shader.PropertyToID("_VerticalBlur");
        private const string k_BlurTextureName = "_BlurTexture";
        private const string k_VerticalPassName = "VerticalBlurRenderPass";
        private const string k_HorizontalPassName = "HorizontalBlurRenderPass";

        private BlurSettings _defaultSettings;
        private Material _material;

        private RenderTextureDescriptor _blurTextureDescriptor;

        public BlurRenderPass(Material material, BlurSettings defaultSettings)
        {
            _material = material;
            _defaultSettings = defaultSettings;

            _blurTextureDescriptor = new RenderTextureDescriptor(Screen.width, Screen.height, RenderTextureFormat.Default, 0);
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();

            TextureHandle srcCamColor = resourceData.activeColorTexture;
            TextureHandle dst = UniversalRenderer.CreateRenderGraphTexture(renderGraph, _blurTextureDescriptor, k_BlurTextureName, false);

            UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

            if (resourceData.isActiveTargetBackBuffer)
                return;

            _blurTextureDescriptor.width = cameraData.cameraTargetDescriptor.width;
            _blurTextureDescriptor.height = cameraData.cameraTargetDescriptor.height;
            _blurTextureDescriptor.depthBufferBits = 0;

            UpdateBlurSettings();

            if (!srcCamColor.IsValid() || !dst.IsValid()) 
                return;

            RenderGraphUtils.BlitMaterialParameters paraVertical = new(srcCamColor, dst, _material, 0);
            renderGraph.AddBlitPass(paraVertical, k_VerticalPassName);

            RenderGraphUtils.BlitMaterialParameters paraHorizontal = new(dst, srcCamColor, _material, 1);
            renderGraph.AddBlitPass(paraHorizontal, k_HorizontalPassName);
        }

        private void UpdateBlurSettings()
        {
            if (_material == null)
                return;

            var volumeComponent = VolumeManager.instance.stack.GetComponent<CustomVolumeComponent>();

            float horizontalBlur = volumeComponent.horizontalBlur.overrideState ? volumeComponent.horizontalBlur.value : _defaultSettings.horizontalBlur;
            float verticalBlur = volumeComponent.verticalBlur.overrideState ? volumeComponent.verticalBlur.value : _defaultSettings.verticalBlur;

            _material.SetFloat(horizontalBlurId, horizontalBlur);
            _material.SetFloat(verticalBlurId, verticalBlur);
        }
    }
}
