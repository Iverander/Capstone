Shader "HLSLTesting2/Gradient"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white"
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            

            struct MeshData
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            
            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
                float _MinSize;
                float _MaxSize;
            CBUFFER_END
            
            //float _OcilatingTime;
            
            Varyings vert(MeshData IN)
            {
                Varyings OUT;
                
                OUT.position = TransformObjectToHClip(IN.positionOS.xyz);
                
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {

                half4 color = _BaseColor * IN.position.y / 1000;
                        
                return SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * color;
                
            }
            ENDHLSL
        }
    }
}
