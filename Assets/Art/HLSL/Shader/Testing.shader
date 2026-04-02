Shader "HLSLTesting/Testing"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        
        [MinSize] _MinSize("Min Size", Float) = 1
        [MaxSize] _MaxSize("Max Size", Float) = 2
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
                float4 Color : TEXCOORD1;
            };
            
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            
            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
                float _MinSize;
                float _MaxSize;
            CBUFFER_END
            
            Varyings vert(MeshData IN)
            {
                Varyings OUT;
                
                float _OcilatingTime = (1+_SinTime.w)/2;
                OUT.position = TransformObjectToHClip(IN.positionOS.xyz * (_MinSize + _OcilatingTime * (_MaxSize - _MinSize)));
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.Color = float4(_SinTime.w, -_SinTime.w, 1, 1);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return IN.Color;
            }
            ENDHLSL
        }
    }
}
