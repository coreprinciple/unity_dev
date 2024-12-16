//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

//struct Attributes
//{
//    float4 positionOS : POSITION;
//    float3 normalOS : NORMAL;
//    float4 tangentOS : TANGENT;
//    UNITY_VERTEX_INPUT_INSTANCE_ID
//};

//struct Varyings
//{
//    float4 positionCS : SV_POSITION;
//    half3 normalWS : TEXCOORD1;
//    UNITY_VERTEX_INPUT_INSTANCE_ID
//    UNITY_VERTEX_OUTPUT_STEREO
//};

half3 PackingNormal(const half3 n)
{
    return n * 0.5 + 0.5;
}