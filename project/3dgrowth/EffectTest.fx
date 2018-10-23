matrix ViewProjection;

float4 TestVertexShader(float4 position : SV_Position) : SV_Position
{
	return mul(position, ViewProjection);
}

float4 TestPixelShader() : SV_Target
{
	return float4(1, 1, 1, 1);
}

technique10 TestTechnique
{
	pass TestPass
	{
		SetVertexShader(CompileShader(vs_5_0, TestVertexShader()));
		SetPixelShader(CompileShader(ps_5_0, TestPixelShader()));
	}
}