Shader "Custom/Snow"
{
    Properties
    {
        //[MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
    	_Texture("Texture", 2D) = "white" {}
    	_NormalMap("NormalMap", 2D) = "white" {}
	    _HeightMap("HeightMap", 2D) = "gray" {}
    	_SnowAmount("SnowAmount", float) = 1
    	
		_Tess("Tessellation", Range(1, 32)) = 20
		_MaxTessDistance("Max Tess Distance", Range(1, 32)) = 20
		//_Weight("Displacement Amount", Range(0, 1)) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue"="Transparent" "RenderPipeline" = "UniversalPipeline" }
        LOD 5000
        Blend SrcAlpha OneMinusSrcAlpha
        //ZWrite Off

        Pass
        {
            HLSLPROGRAM

            #pragma vertex TessellationVertexProgram
            #pragma fragment frag
            // This line defines the name of the hull shader. 
			#pragma hull hull
			// This line defines the name of the domain shader. 
			#pragma domain domain

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "CustomTessellation.hlsl"

            CBUFFER_START(UnityPerMaterial)
                sampler2D _Texture;
                sampler2D _HeightMap;
				//float _Weight;
				sampler2D _NormalMap;
				float _SnowAmount;
            CBUFFER_END
            
            ControlPoint TessellationVertexProgram(Attributes v)
			{
				ControlPoint p;

				p.vertex = v.vertex;
				p.uv = v.uv;
				p.normal = v.normal;//UnpackNormal(tex2D(_NormalMap, v.uv));
				p.color = v.color;

				return p;
			}

			Varyings vert(Attributes input)
			{
				Varyings output;
				float Noise = tex2Dlod(_HeightMap, float4(input.uv, 0, 0)).r;

				input.vertex.xyz += (input.normal) *  Noise * _SnowAmount / 30;
				output.vertex = TransformObjectToHClip(input.vertex.xyz);
				output.color = input.color;
				output.normal = input.normal;
				output.uv = input.uv;
				return output;
			}
            
            [UNITY_domain("tri")]
			Varyings domain(TessellationFactors factors, OutputPatch<ControlPoint, 3> patch, float3 barycentricCoordinates : SV_DomainLocation)
			{
				Attributes v;

				#define DomainPos(fieldName) v.fieldName = \
						patch[0].fieldName * barycentricCoordinates.x + \
						patch[1].fieldName * barycentricCoordinates.y + \
						patch[2].fieldName * barycentricCoordinates.z;

				DomainPos(vertex)
				DomainPos(uv)
				DomainPos(color)
				DomainPos(normal)

				return vert(v);
			}
			
            float4 frag(Varyings IN) : SV_Target
			{
				
				
				//Light & shadows
				//half4 shadowCoord = TransformWorldToShadowCoord(IN.)
				//Light 
				
				
				float4 tex = tex2D(_Texture, IN.uv);
				float4 height = tex2D(_HeightMap, IN.uv);
				//half3 normal = UnpackNormal(tex2D(_NormalMap, IN.uv));
				
				//tex = float4(tex.xyz, clamp((height.x * height.y  * height.z)*_SnowAmount, 0, .5)) + tex;

				return tex;
			}
            ENDHLSL
        }
    }
}
