matrix ViewProjection;
matrix Model;
Texture2D diffuseTexture;

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
    output.Position = mul(output.Position, Model);
    output.Position = mul(output.Position, ViewProjection);
    return output;
}

float4 TestPixelShader(VertexOutput input) : SV_Target
{
	float3 lightColor = float3(1, 1, 1);
	float3 lightDirection = float3(1, -1, 1);
    float4 tex = diffuseTexture.Sample(mySampler, input.TextureCoordinate);

	float3 normal = normalize(input.Normal.xyz);
	float3 light = normalize(-lightDirection);
	float3 i = saturate(dot(light, normal)) * lightColor;

	return float4(tex.xyz, 1.0);
}

technique10 TestTechnique
{
	pass TestPass
	{
		SetVertexShader(CompileShader(vs_4_1, TestVertexShader()));
		SetPixelShader(CompileShader(ps_4_1, TestPixelShader()));
	}
}