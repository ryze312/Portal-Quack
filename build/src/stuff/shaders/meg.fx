sampler s0;

float4 PS(float2 coords : TEXCOORD0) : COLOR0
{
    float4 new_color = tex2D(s0, coords);
    new_color.rgb *= 0.2;
    
    return new_color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PS();
    }
}