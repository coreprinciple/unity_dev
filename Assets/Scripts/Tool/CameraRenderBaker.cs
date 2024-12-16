using EasyButtons;
using UnityEngine;
using UnityEngine.Rendering;

namespace Tool
{
    public class CameraRenderBaker : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private RenderTexture _renderTexture;

        [Button("Bake")]
        public void Bake()
        {
            CommandBuffer commandBuffer = CommandBufferPool.Get();
            RenderTargetIdentifier rtIdentifier = new RenderTargetIdentifier(_renderTexture);
            commandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, rtIdentifier);
            _camera.AddCommandBuffer(CameraEvent.AfterEverything, commandBuffer);
        }
    }
}
