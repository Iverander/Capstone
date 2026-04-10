//-------------------White Noise--------------------------------------------------
//------------------1D----------------------

//turns a 3D vector to a 'random' 1D vector (color to black & white)
float random3Dto1D(float3 vec, float3 dotDir = float3(32.3215,  78.2317, 58.123))
{
    float3 minivec = sin(vec); //makes the vecor smaller as to avoid flaot limits
    float random = dot(minivec, dotDir); //
    random = frac(sin(random) * 943758.5453);
    return random;
}
float random2Dto1D(float2 vec, float2 dotDir = float2(32.3215,  78.2317))
{
    float2 minivec = sin(vec); 
    float random = dot(minivec, dotDir); 
    random = frac(sin(random) * 943758.5453);
    return random;
}
float random1Dto1D(float vec, float dotDir = 32.3215)
{
    float minivec = sin(vec); 
    float random = dot(minivec, dotDir); 
    random = frac(sin(random) * 943758.5453);
    return random;
}

//------------------2D----------------------
float2 random2Dto2D(float2 vec)
{
    return float2(
        random2Dto1D(vec, float2(63.3215,  95.15134)),
        random2Dto1D(vec, float2(26.24812,  78.18634))
        );
}

//------------------3D----------------------
//turns a 3D vector to a 'random' 3D vector
float3 random3Dto3D(float3 vec)
{
    return float3(
        random3Dto1D(vec, float3(42.12312, 84.56244, 81.35183)),
        random3Dto1D(vec, float3(75.51634, 32.68362, 85.745158)),
        random3Dto1D(vec, float3(31.14758, 60.42754, 95.81244))
        );
}

//-------------------Vornoi Noise--------------------------------------------------
float voronoiNoise(float2 value)
{
    float2 baseCell = floor(value);
    float minDistancToCell = 10;
    
    for (int x = -1; x <= 1; x++)
    {
        for (int y = -1; y <= 1; y++)
        {
            float2 cell = baseCell + float2(x, y);
            float2 cellPos = cell + random2Dto2D(cell);
            float2 toCell = cellPos - value;
            float distanceToCell = length(toCell);
            
            if (distanceToCell < minDistancToCell)
                minDistancToCell = distanceToCell;
        }
    }
    
    return minDistancToCell;
}

//-----------------Perlin Noise------------------------------------------------
float perlinNoise(float2 value)
{
    
}
