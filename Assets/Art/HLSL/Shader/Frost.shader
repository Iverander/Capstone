Shader "Custom/Frost"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _BaseMap("Base Map", 2D) = "white" {}
        _FrostTexture("Frost Texture", 2D) = "white" {}
        _FrostNormal("Frost Normal", 2D) = "bump" {}
        _FrostColor("Frost Color", Color) = (0.85, 0.95, 1.0, 1.0)
        _FresnelColor("Fresnel Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _Growth("Growth", Range(0, 1)) = 0.3
        _RimPower("Rim Power", Range(0.1, 10)) = 4.0
        _Smoothness("Ice Smoothness", Range(0, 1)) = 0.85
        _FrostIntensity("Frost Intensity", Range(0, 1)) = 1.0
        _Metallic("Metallic", Range(0, 1)) = 0.05
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        LOD 2000

        Pass
        {
            Tags { "LightMode" = "UniversalForward" }
            
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float3 tangentWS : TEXCOORD3;
                float3 bitangentWS : TEXCOORD4;
                float3 viewDir : TEXCOORD5;
                float fogCoord : TEXCOORD6;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            TEXTURE2D(_FrostTexture);
            SAMPLER(sampler_FrostTexture);
            TEXTURE2D(_FrostNormal);
            SAMPLER(sampler_FrostNormal);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
                half4 _FrostColor;
                half4 _FresnelColor;
                float4 _FrostTexture_ST;
                float4 _FrostNormal_ST;
                float _Growth;
                float _RimPower;
                float _Smoothness;
                float _FrostIntensity;
                float _Metallic;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                
                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.positionWS = positionWS;
                
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.tangentWS = TransformObjectToWorldDir(IN.tangentOS.xyz);
                OUT.bitangentWS = cross(OUT.normalWS, OUT.tangentWS) * IN.tangentOS.w;
                
                OUT.viewDir = GetWorldSpaceViewDir(positionWS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                
                OUT.fogCoord = ComputeFogFactor(OUT.positionHCS.z);
                
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Sample base color
                half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;
                
                // Sample frost texture for coverage
                float frostTex = SAMPLE_TEXTURE2D(_FrostTexture, sampler_FrostTexture, IN.uv).r;
                float frostCoverage = frostTex * _Growth;
                frostCoverage = saturate(frostCoverage);
                
                // Sample frost normal map
                half3 frostNormalMap = UnpackNormal(SAMPLE_TEXTURE2D(_FrostNormal, sampler_FrostNormal, IN.uv));
                
                // Create TBN matrix for normal mapping
                float3x3 TBN = float3x3(IN.tangentWS, IN.bitangentWS, IN.normalWS);
                float3 normalWS = normalize(mul(frostNormalMap, TBN));
                
                // Blend normals based on frost coverage
                normalWS = normalize(lerp(IN.normalWS, normalWS, frostCoverage));
                
                // Calculate Fresnel/Rim effect for icy appearance
                float3 viewDirNorm = normalize(IN.viewDir);
                float fresnel = 1.0 - saturate(dot(viewDirNorm, normalWS));
                fresnel = pow(fresnel, _RimPower);
                fresnel *= _FrostIntensity;
                
                // Blend colors: base to frost
                half3 frostBlended = _FrostColor.rgb;
                half3 albedo = lerp(baseColor.rgb, frostBlended, frostCoverage);
                
                // Add fresnel highlight for icy appearance
                albedo += fresnel * _FresnelColor.rgb * 0.3;
                
                // Main light calculations
                Light mainLight = GetMainLight();
                half3 lightDir = normalize(mainLight.direction);
                half3 halfDir = normalize(viewDirNorm + lightDir);
                
                half NdotL = saturate(dot(normalWS, lightDir));
                half NdotH = saturate(dot(normalWS, halfDir));
                
                // Specular (using Fresnel for ice effect)
                half specular = pow(NdotH, (1.0 - _Smoothness) * 100.0) * fresnel;
                
                // Final color
                half3 finalColor = albedo * mainLight.color * NdotL;
                finalColor += specular * mainLight.color;
                
                // Apply fog
                finalColor = MixFog(finalColor, IN.fogCoord);
                
                return half4(finalColor, baseColor.a);
            }
            ENDHLSL
        }

        // Shadow casting pass
        Pass
        {
            Tags { "LightMode" = "ShadowCaster" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            float3 _LightDirection;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.positionHCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }
    }
    
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}