matrix ViewProjection;
matrix Model;
Texture2D diffuseTexture;

cbuffer Constant
{
    float EyePositionX;
    float EyePositionY;
    float EyePositionZ;
};

SamplerState mySampler
{
};

struct VertexOutput
{
    float4 Position : SV_Position;
	float4 Normal : NORMAL;
	float2 TextureCoordinate : TEXCOORD;
};
 
VertexOutput TestVertexShader(VertexOutput input)
{
    VertexOutput output = input;
    output.Position = mul(output.Position, ViewProjection);
    return output;
}

float4 TestPixelShader(VertexOutput input) : SV_Target
{
    float4 wireColor = float4(0.7, 1, 0.08, 1);

	return wireColor;
}

technique10 TestTechnique
{
	pass TestPass
	{
		SetVertexShader(CompileShader(vs_4_1, TestVertexShader()));
		SetPixelShader(CompileShader(ps_4_1, TestPixelShader()));
	}
}