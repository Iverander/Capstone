Shader "HLSLTesting/Gradient"
{
    Properties
    {
        _Color("Base Color", Color) = (1, 1, 1, 1)
        _TransitionColor("Transition Color", Color) = (1, 1, 1, 1)
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
                half4 color : TEXCOORD1;
            };
            
            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                half4 _TransitionColor;
            CBUFFER_END
            
            Varyings vert(MeshData IN)
            {
                Varyings OUT;
                
                OUT.position = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.color = lerp(_Color, _TransitionColor, IN.positionOS.y);
                
                /*
                OUT.color = _Color;
                
                if (IN.positionOS.y > 0)
                {
                    OUT.color = _TransitionColor;
                }*/
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return IN.color;
            }
            ENDHLSL
        }
    }
}
