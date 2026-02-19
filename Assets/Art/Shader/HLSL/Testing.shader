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
            
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            
            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
                float _MinSize;
                float _MaxSize;
            CBUFFER_END
            
            //float _OcilatingTime;
            
            Varyings vert(MeshData IN)
            {
                Varyings OUT;
                
                float _OcilatingTime = (1+_SinTime.w)/2;
                //OUT.position = TransformObjectToHClip(IN.positionOS.xyz * ((_MinSize + _OcilatingTime * (_MaxSize - _MinSize))));
                
                //float4 finalPosition = IN.positionOS + float4(_SinTime.w / 10 / IN.positionOS.y, 0, 0, 0);
                
                OUT.position = TransformObjectToHClip(IN.positionOS.xyz);
                
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }
            

            float4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.position.xy * 2  / _ScreenSize.xy;
                uv -= float2(.8, .8);
                /*
                if (IN.position.x < _ScreenSize.x /2)
                {
                    uv = float2(0,0);
                }*/

                for (float i = -100; i < 100; ++i)
                {
                    if (IN.position.y > i / 100)
                    {
                        float4 color = float4((i+100)/200 , 1, 1, 1);
                        
                        return  color;
                    }
                }
                
                return  float4(0,0,0,1);
                
                //float _OcilatingTime = (1+_SinTime.w)/2;
                //float4 color = float4(_SinTime.w, -_SinTime.w, 1, 1);
            }
            ENDHLSL
        }
    }
}
