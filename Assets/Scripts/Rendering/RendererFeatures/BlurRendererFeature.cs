using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using System.Collections.Generic;

namespace Rendering.RendererFeatures
{
    public class BlurRendererFeature : ScriptableRendererFeature
    {
        [System.Serializable]
        public class BlurSettings
        {
            [Range(0.0f, 0.4f)] public float horizontalBlur;
            [Range(0.0f, 0.4f)] public float verticalBlur;
        }

        [SerializeField] private BlurSettings _settings;
        [SerializeField] private Shader _shader;

        private BlurRenderPass _blurRenderPass;
        private Material _material;

        public override void Create()
        {
            if (_shader == null)
                return;

            _material = new Material(_shader);
            _blurRenderPass = new BlurRenderPass(_material, _settings);
            _blurRenderPass.renderPassEvent = RenderPassEvent.AfterRenderingSkybox;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (_blurRenderPass == null)
                return;

            if (renderingData.cameraData.cameraType == CameraType.Game)
                renderer.EnqueuePass(_blurRenderPass);
        }

        protected override void Dispose(bool disposing)
        {
            if (Application.isPlaying)
                Destroy(_material);
            else
                DestroyImmediate(_material);
        }
    }
}
