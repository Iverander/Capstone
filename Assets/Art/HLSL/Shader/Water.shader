Shader "Custom/Water"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white"
        [RippleStrength] _RippleStrength("RippleStrength", Float) = 0
        [Depth] _Depth("Depth", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue"="Transparent" "RenderPipeline" = "UniversalPipeline" }
        LOD 5000
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            //#include "Assets/Art/HLSL/Shader/Noise.hlsl"
            #include "Assets/Art/HLSL/Shader/Ripples.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float alpha : TEXCOORD1;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
                float _RippleStrength;
                float _Depth;
            CBUFFER_END
            
            float InverseLerp(float a, float b, float v)
            {
                return  (v - a) / (b - a);
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.alpha = clamp(distance(_WorldSpaceCameraPos, OUT.positionHCS) / 70, .3, 3);
                
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float4 returnColor = _BaseColor;
                
                float Out;
                float3 Normal;
                Ripples(IN.uv, 3, 10, _Time.w, _RippleStrength, Out, Normal);
                float3 rippleNormal = InverseLerp(-1, 1, Normal);
                
                float inverseAlpha = abs(-IN.alpha + IN.alpha);
                
                returnColor *= float4(rippleNormal, 1); //add ripples
                returnColor *= max(voronoiNoise(IN.uv * 10 * ((_Time.y + 300) / 1000)) * 1.1, .7); //add noise

                return returnColor * IN.alpha;
                
                
                //return float4 (1, 1, 1, (IN.positionHCS.a + _Depth) - (SHADERG * _ProjectionParams.z));
            }
            ENDHLSL
        }
    }
}
