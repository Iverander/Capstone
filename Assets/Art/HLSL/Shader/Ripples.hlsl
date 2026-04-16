#include "Assets/Art/HLSL/Shader/Noise.hlsl"

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
            float2 noise = voronoiNoise(fmod(lattice + g, cellDensity));
            float2 offset = float2( 
                sin(noise.y * +angleOffset) * .5 + .5, 
                cos(noise.x * angleOffset) * .5 + .5 );
            
            float d = distance(lattice + offset, f);
            
            float t = frac(time + (offset.x * 5));
            
            d = (1-d) * (1-d) * strength * pow(saturate(1 - abs(d - t)), 8) * (sin((d-t) * 30));
            Out = max(Out, -d);
            Normal += d * (normalize(float3(normalize((lattice + offset).xy - f), 3))).xyz;
        }
    }
    
    Normal = normalize(Normal);
}


#ifndef Ripples_Include
#define Ripples_Include

void Ripples_float(float2 uv, float angleOffset, float cellDensity, float time, float strength, out float Out, out float3 Normal)
{
   Ripples(uv, angleOffset, cellDensity, time, strength, Out, Normal); 
}

#endif