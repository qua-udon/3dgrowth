matrix ViewProjection;
 
struct VertexPositionColor
{
    float4 Position : SV_Position;
    float4 Color : COLOR;
};
 
VertexPositionColor TestVertexShader(VertexPositionColor input)
{
    VertexPositionColor output = input;
    output.Position = mul(output.Position, ViewProjection);
    return output;
}

float4 TestPixelShader(VertexPositionColor input) : SV_Target
{
	return input.Color;
}

technique10 TestTechnique
{
	pass TestPass
	{
		SetVertexShader(CompileShader(vs_4_1, TestVertexShader()));
		SetPixelShader(CompileShader(ps_4_1, TestPixelShader()));
	}
}