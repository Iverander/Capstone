Shader "Custom/Pulse"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white"
        _val("Value", float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline" = "UniversalPipeline" }
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
                float _val;
            CBUFFER_END

            float3 pulse()
            {
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap) - float2(.5,.5);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float4 color;

                float outerCircle = (_SinTime.w + 1) /4;
                float innerCircle = outerCircle - .1;

                if(
                (abs(IN.uv.y) < outerCircle && abs(IN.uv.x) < outerCircle) && 
                (abs(IN.uv.y) > innerCircle && abs(IN.uv.x) > innerCircle))
                    color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;
                else
                    color = float4(1,1,1,0);
                return color;
            }
            ENDHLSL
        }
    }
}
