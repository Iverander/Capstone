Shader "Custom/Water"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white"
        [RippleStrength] _RippleStrength("RippleStrength", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue"="Transparent" "RenderPipeline" = "UniversalPipeline" }
        LOD 2000
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
                float _RippleStrength;
            CBUFFER_END
            
            float2 voronoi_randomVector(float2 uv, float offset)
            {
                float2x2 m = float2x2(15.27, 57.63, 99.41, 89.98);
                uv = frac(sin(mul(uv, m)) * 46839.32);
                return float2(sin(uv.y * +offset) * .5 + .5, cos(uv.x * offset) * .5 + .5);
            }
            
            void Ripples(float2 uv, float angleOffset, float cellDensity, float time, float strength, out float Out, out float3 Normal)
            {
                float2 g = floor(uv *  cellDensity);
                float2 f = frac(uv *  cellDensity); //Returns the fractions. ie 2.54 => 0.54
                
                Out = 0;
                Normal = float3(0,0,1);

                for (int y = -1; y <= 1; ++y)
                {
                    for (int x = -1; x <= 1; ++x)
                    {
                        float2 lattice = float2(x, y);
                        float2 offset = voronoi_randomVector(fmod(lattice + g, cellDensity), angleOffset);
                        float d = distance(lattice + offset, f);
                        
                        float t = frac(time + (offset.x * 5));
                        
                        d = (1-d) * (1-d) * strength * pow(saturate(1 - abs(d - t)), 8) * (sin((d-t) * 30));
                        Out = max(Out, -d);
                        Normal += d * (normalize(float3(normalize((lattice + offset).xy - f), 3))).xyz;
                    }
                }
                
                Normal = normalize(Normal);
            }
            
            float InverseLerp(float a, float b, float v)
            {
                return  (v - a) / (b - a);
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float4 returnColor = _BaseColor;
                
                float Out;
                float3 Normal;
                Ripples(IN.uv, 3, 10, _Time.w, _RippleStrength, Out, Normal);
                
                float3 rippleNormal = InverseLerp(-1, 1, Normal);
                
                returnColor *= float4(rippleNormal, 1);
                
                return returnColor;
            }
            ENDHLSL
        }
    }
}
