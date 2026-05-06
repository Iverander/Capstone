Shader "Custom/WetPuddleShader"
{
    Properties
    {
        _MainTex ("Ground Texture", 2D) = "white" {}
        _WetMask ("Puddle Mask", 2D) = "white" {}
        _WaterNormal ("Water Normal", 2D) = "bump" {}
        _Cubemap ("Reflection Cubemap", CUBE) = "" {}
        _PuddleColor ("Puddle Color", Color) = (0.3,0.4,0.6,0.5)
        _WetStrength ("Wet Strength", Range(0,1)) = 0.7
        _Gloss ("Gloss", Range(0,1)) = 0.9
        _Distortion ("Distortion", Range(0,0.2)) = 0.03
        _NormalSpeed ("Normal Speed", Float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
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
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _WetMask_ST;
                float4 _WaterNormal_ST;
                float4 _PuddleColor;
                float _WetStrength;
                float _Gloss;
                float _Distortion;
                float _NormalSpeed;
            CBUFFER_END

            TEXTURE2D(_MainTex);        SAMPLER(sampler_MainTex);
            TEXTURE2D(_WetMask);        SAMPLER(sampler_WetMask);
            TEXTURE2D(_WaterNormal);    SAMPLER(sampler_WaterNormal);
            TEXTURECUBE(_Cubemap);      SAMPLER(sampler_Cubemap);

            Varyings vert (Attributes IN)
            {
                Varyings OUT = (Varyings)0;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.worldNormal = TransformObjectToWorldNormal(IN.normalOS);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                // Main and mask tex
                float3 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv).rgb;
                float puddleMask = SAMPLE_TEXTURE2D(_WetMask, sampler_WetMask, IN.uv).r;

                // Animated normal
                float2 nUV = IN.uv + float2(_Time.y * _NormalSpeed, _Time.y * _NormalSpeed * 0.8);
                float3 wNormal = UnpackNormal( SAMPLE_TEXTURE2D(_WaterNormal, sampler_WaterNormal, nUV) );

                float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - IN.worldPos);
                float3 surfaceNormal = normalize(IN.worldNormal + float3(wNormal.xy * _Distortion, 0));
                float3 reflDir = reflect(-viewDir, surfaceNormal);
                float4 refl = SAMPLE_TEXTURECUBE(_Cubemap, sampler_Cubemap, reflDir);

                // Mixing colors and gloss
                float puddle = puddleMask * _WetStrength;
                float3 color = lerp(albedo, _PuddleColor.rgb + refl.rgb * _Gloss, puddle);

                return float4(color, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack "Unlit"
}
