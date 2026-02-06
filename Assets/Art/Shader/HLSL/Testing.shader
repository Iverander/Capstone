Shader "Custom/Testing"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

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
            
            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
            CBUFFER_END
            
            
            float4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.position.xy / _ScreenSize.xy;
                /*
                if (IN.position.x < _ScreenSize.x /2)
                {
                    uv = float2(0,0);
                }*/
                return float4(uv -.4, 0, 1);
            }
            
            
            Varyings vert(MeshData IN)
            {
                Varyings OUT;
                OUT.position = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }
            ENDHLSL
        }
    }
}
