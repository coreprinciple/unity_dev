using UnityEngine;

namespace Rendering
{
    public static class ShaderConst
    {
        public const string HIDDEN_OUTLINE = "Hidden/Outline";
        public const string HIDDEN_VIEW_SPACE_NORMAL = "Hidden/ViewSpaceNormal";

        public static readonly int ID_OutlineColor = UnityEngine.Shader.PropertyToID("_OutlineColor");
        public static readonly int ID_OutlineScale = UnityEngine.Shader.PropertyToID("_OutlineScale");
        public static readonly int ID_DepthThreshold = UnityEngine.Shader.PropertyToID("_DepthThreshold");
        public static readonly int ID_RobertCrossMultiplier = UnityEngine.Shader.PropertyToID("_RobertCrossMultiplier");
        public static readonly int ID_NormalThreshold = UnityEngine.Shader.PropertyToID("_NormalThreshold");

        public static readonly int ID_SteepAngleThreshold = UnityEngine.Shader.PropertyToID("_SteepAngleThreshold");
        public static readonly int ID_SteepAngleMultiplier = UnityEngine.Shader.PropertyToID("_SteepAngleMultiplier");

        public static string[] OutLineShaderTagIds = {
            "UniversalForward",
            "UniversalForwardOnly",
            "LightweightForward",
            "SRPDefaultUnlit",
        };
    }
}