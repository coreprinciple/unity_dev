Shader "URP/Custom/GBufferBake"
{
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        ZWrite Off Cull Off
        
        // Pass
        // {
        //     Name  "Albedo"
            
        //     HLSLPROGRAM

        //     #pragma vertex Vert
        //     #pragma fragment Frag

        //     #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        //     #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

        //     float4 Frag(Varyings input) : SV_Target
        //     {
        //         UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        //         float2 uv = input.texcoord.xy;
        //         half4 color = SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearRepeat, uv, _BlitMipLevel);
        //         return color;
        //     }
        //     ENDHLSL
        // }

        Pass
        {
            Name  "Normal"
            
            HLSLPROGRAM
            
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "DeferredReflectionNormal.hlsl"

            // Varyings Vert(Attributes input)
            // {
            //     Varyings output = (Varyings)0;

            //     float4 positionOS = input.positionOS;
            //     float3 positionWS = TransformObjectToWorld(positionOS.xyz);
            //     // float3 positionVS = TransformWorldToView(positionWS);
            //     float4 positionCS = TransformWorldToHClip(positionWS);
            //     output.positionCS = positionCS;

            //     VertexPositionInputs positions = GetVertexPositionInputs(positionOS);
            //     VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

            //     half3 normalWS = TransformTangentToWorld(input.normalOS, half3x3(normalInput.tangentWS.xyz, normalInput.bitangentWS.xyz, normalInput.normalWS.xyz));
            //     normalWS = PackingNormal(NormalizeNormalPerPixel(normalWS));

            //     output.normalWS = normalWS;
            //     return output;
            // }

            float4 Frag(Varyings input) : SV_Target
            {
                //return float4(input.normalWS.xyz, 1);

                float2 uv = input.texcoord.xy;
                half4 color = SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearRepeat, uv, _BlitMipLevel);
                float3 normalWS = PackingNormal(NormalizeNormalPerPixel(color.xyz));
                return float4(normalWS, 1);
            }
            ENDHLSL
        }
    }
}
