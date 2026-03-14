//-------------------White Noise--------------------------------------------------

//turns a 3D vector to a 'random' 1D vector (color to black & white)
float random3Dto1D(float3 vec, float3 dotDir = float3(32.3215,  78.2317, 58.123))
{
    float3 minivec = sin(vec); //makes the vecor smaller as to avoid flaot limits
    float random = dot(minivec, dotDir); //
    random = frac(sin(random) * 943758.5453);
    return random;
}
            
//turns a 3D vector to a 'random' 3D vector
float3 random3Dto3D(float3 vec)
{
    return float3(
        random3Dto1D(vec, float3(42.12312, 84.56244, 81.35183)),
        random3Dto1D(vec, float3(75.51634, 32.68362, 85.745158)),
        random3Dto1D(vec, float3(31.14758, 60.42754, 95.81244))
        );
}
