Shader "HLSLTesting/Gradient"
{
    Properties
    {
        _Color("Base Color", Color) = (1, 1, 1, 1)
        _TransitionColor("Transition Color", Color) = (1, 1, 1, 1)
        _TransitionSpeed("Transition Speed", Float) = .5
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
            
            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                half4 _TransitionColor;
                float _TransitionSpeed;
            CBUFFER_END
            
            Varyings vert(MeshData IN)
            {
                Varyings OUT;
                
                OUT.position = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                
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
                float3 color = lerp(_Color, _TransitionColor, smoothstep(-_SinTime.w, _SinTime.w, IN.uv.y));
                return float4(color, 0);
            }
            ENDHLSL
        }
    }
}
