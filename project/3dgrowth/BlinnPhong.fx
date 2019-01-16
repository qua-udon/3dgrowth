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
	float4 tex = diffuseTexture.Sample(mySampler, input.TextureCoordinate);
	float3 lightColor = float3(1, 1, 1);
	float3 lightDirection = float3(1, -1, 1);
	float3 ambientColor = float3(0.21, 0.22, 0.26);
	int shiness = 10;

	float3 normal = normalize(input.Normal.xyz);
    float3 eyeAddLight = float3(EyePositionX, EyePositionY, EyePositionZ) - lightDirection;
	float3 halfVec = eyeAddLight / length(eyeAddLight);
	float3 i = pow(saturate(dot(halfVec, normal)), shiness) * lightColor;

	return float4((i + ambientColor) * tex.xyz, 1.0);
}

technique10 TestTechnique
{
	pass TestPass
	{
		SetVertexShader(CompileShader(vs_4_1, TestVertexShader()));
		SetPixelShader(CompileShader(ps_4_1, TestPixelShader()));
	}
}