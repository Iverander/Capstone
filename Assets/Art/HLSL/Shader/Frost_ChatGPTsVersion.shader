Shader "Custom/Frost_ChatGPTsVersion"
{
Properties
{
    [MainColor] _BaseColor("Base Color", Color) = (1,1,1,1)

    _FrostTexture("Frost Texture", 2D) = "white" {}
    _FrostNormal("Frost Normal", 2D) = "bump" {}
    _NoiseTexture("Noise Texture", 2D) = "gray" {}

    _FrostColor("Frost Color", Color) = (0.8,0.9,1,1)
    _GrowthColor("Growth Color", Color) = (0.6,0.8,1,1)

    _Tiling("Frost Tiling", Vector) = (1,1,0,0)
    _NoiseTiling("Noise Tiling", Vector) = (1,1,0,0)

    _Growth("Growth", Range(0,1)) = 0.5
    _GrowthStrength("Growth Strength", Range(0,5)) = 1

    _IceSmoothness("Ice Smoothness", Range(0,1)) = 0.8
    _FrostSmoothness("Frost Smoothness", Range(0,1)) = 0.4

    _Metallic("Metallic", Range(0,1)) = 0.0

    _NormalStrength("Normal Strength", Range(0,2)) = 1

    _RimPower("Rim Power", Range(0,8)) = 2
    _RimColor("Rim Color", Color) = (1,1,1,1)

    _SaturateColor("Saturate Color", Color) = (1,1,1,1)
    _SaturateStrength("Saturate Strength", Range(0,2)) = 0
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

            // ===== TEXTURES =====
TEXTURE2D(_FrostTexture);
SAMPLER(sampler_FrostTexture);

TEXTURE2D(_FrostNormal);
SAMPLER(sampler_FrostNormal);

TEXTURE2D(_NoiseTexture);
SAMPLER(sampler_NoiseTexture);

// ===== PROPERTIES =====
float3 _BaseColor;
float3 _FrostColor;

float2 _Tiling;
float _Growth;

float _IceSmoothness;
float _FrostSmoothness;
float _Metallic;

float3 _GrowthColor;
float _GrowthStrength;

float2 _NoiseTiling;

// ===== INPUT =====
struct Attributes
{
    float4 positionOS : POSITION;
    float2 uv : TEXCOORD0;
};

struct Varyings
{
    float4 positionCS : SV_POSITION;
    float2 uv : TEXCOORD0;
};

Varyings vert (Attributes v)
{
    Varyings o;
    o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
    o.uv = v.uv;
    return o;
}

// ===== FRAGMENT =====
float4 frag (Varyings i) : SV_Target
{
    float2 uv = i.uv;

    // ---- Frost Texture ----
    float2 frostUV = uv * _Tiling;
    float frostTex = SAMPLE_TEXTURE2D(_FrostTexture, sampler_FrostTexture, frostUV).r;

    // ---- Growth Mask ----
    float edge1 = _Growth - 0.2;
    float edge2 = _Growth + 0.2;
    float frostMask = smoothstep(edge1, edge2, frostTex);

    // ---- Noise (for growth breakup) ----
    float2 noiseUV = uv * _NoiseTiling;
    float noise = SAMPLE_TEXTURE2D(_NoiseTexture, sampler_NoiseTexture, noiseUV).r;

    // combine noise with mask (approx of your graph)
    frostMask *= noise;

    // ---- Color Blend ----
    float3 baseBlend = lerp(_BaseColor, _FrostColor, frostMask);

    // extra brightness (your Add + Multiply chain)
    float3 finalColor = baseBlend + (baseBlend * frostMask);

    // ---- Emission (growth glow) ----
    float3 emission = _GrowthColor * _GrowthStrength * frostMask;

    // ---- Smoothness ----
    float smoothness = lerp(_IceSmoothness, _FrostSmoothness, frostMask);

    // ---- Output (simple version) ----
    return float4(finalColor + emission, 1.0);
}
            ENDHLSL
        }
    }
}
