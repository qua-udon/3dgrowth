matrix ViewProjection;
matrix Model;
Texture2D diffuseTexture;

SamplerState mySampler
{
};

cbuffer Constant
{
    int IsHit;
};

struct VertexPositionTexture
{
    float4 Position : SV_Position;
	float2 TextureCoordinate : TEXCOORD;
};
 
VertexPositionTexture TestVertexShader(VertexPositionTexture input)
{
    VertexPositionTexture output = input;
    output.Position = mul(output.Position, Model);
    output.Position = mul(output.Position, ViewProjection);
    return output;
}

float4 TestPixelShader(VertexPositionTexture input) : SV_Target
{
    float3 collision = float3(1, 0.3, 0.3);
	float4 output = diffuseTexture.Sample(mySampler, input.TextureCoordinate);
    return IsHit == 1 ? float4(output.xyz * collision, output.w) : output;
}

technique10 TestTechnique
{
	pass TestPass
	{
		SetVertexShader(CompileShader(vs_4_1, TestVertexShader()));
		SetPixelShader(CompileShader(ps_4_1, TestPixelShader()));
	}
}