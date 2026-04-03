Shader "Custom/Reflective"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white"
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f
            {
                half3 worldRefl : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert(float4 vertex : POSITION, float3 normal : NORMAL)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(vertex);
                float3 worldPos = mul(unity_ObjectToWorld, vertex).xyz;
                float3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                float3 worldNormal = UnityObjectToWorldNormal(normal);
                o.worldRefl = reflect(-worldViewDir, worldNormal);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, i.worldRefl);
                half3 skyColor = DecodeHDR(skyData, unity_SpecCube0_HDR);
                half4 c = 0;
                c.rgb = skyColor;
                return c;
            }
            ENDHLSL
        }
    }
}
