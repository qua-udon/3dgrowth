matrix ViewProjection;
Texture2D diffuseTexture;

SamplerState mySampler
{
};

struct VertexPositionTexture
{
    float4 Position : SV_Position;
	float2 TextureCoordinate : TEXCOORD;
};
 
VertexPositionTexture TestVertexShader(VertexPositionTexture input)
{
    VertexPositionTexture output = input;
    output.Position = mul(output.Position, ViewProjection);
    return output;
}

float4 TestPixelShader(VertexPositionTexture input) : SV_Target
{
	return diffuseTexture.Sample(mySampler, input.TextureCoordinate);
}

technique10 TestTechnique
{
	pass TestPass
	{
		SetVertexShader(CompileShader(vs_4_1, TestVertexShader()));
		SetPixelShader(CompileShader(ps_4_1, TestPixelShader()));
	}
}