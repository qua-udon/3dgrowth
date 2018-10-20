using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;

namespace _3dgrowth
{
    /// <summary>
    /// ゲート0:三角形の描画
    /// </summary>
    public class DrawTriangle : System.IDisposable
    {
        private Buffer _vertexBuffer;
        private InputLayout _inputLayout;

        public void Draw(Device device)
        {
            InitializeTriangleInputAssembler(device);
            device.ImmediateContext.Draw(3, 0);
        }

        private void InitializeTriangleInputAssembler(Device device)
        {
            _inputLayout = CreateInputLayout(device);
            _vertexBuffer = CreateVertexBuffer(device, TriangleVertice);
            device.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, sizeof(float) * 3, 0));
            device.ImmediateContext.InputAssembler.InputLayout = _inputLayout;
            device.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
        }

        public void Dispose()
        {
            _vertexBuffer?.Dispose();
            _inputLayout?.Dispose();
        }

        private InputLayout CreateInputLayout(Device device)
        {
            using (Effect effect = CreateEffect(device))
            {
                return new InputLayout(device, effect.GetTechniqueByIndex(0).GetPassByIndex(0).Description.Signature,
                new InputElement[]
                {
                    new InputElement
                    {
                        SemanticName = "SV_Position",
                        Format = SlimDX.DXGI.Format.R32G32B32A32_Float
                    }
                });
            }
        }

        private Buffer CreateVertexBuffer(Device device, System.Array vertice)
        {
            DataStream stream = new DataStream(vertice, true, true);
            return new Buffer(device, stream,
                new BufferDescription
                {
                    SizeInBytes = (int)stream.Length,
                    BindFlags = BindFlags.VertexBuffer,
                });
        }

        private Effect CreateEffect(Device device)
        {
            ShaderBytecode shader = ShaderBytecode.CompileFromFile("EffectTest.fx", "fx_5_0", ShaderFlags.None, EffectFlags.None);
            return new Effect(device, shader);
        }

        private static System.Array TriangleVertice
        {
            get
            {
                return new[]
                {
                    new Vector3(-1, 0, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                };
            }
        }
    }
}
