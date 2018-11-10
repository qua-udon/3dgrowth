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
        private Device _device;
        private Buffer _vertexBuffer;
        private InputLayout _inputLayout;
        private Effect _effect;
        private double _theta;
        private DirectInputDetector _detector;
        private Vector3 _cameraAngle;

        public DrawTriangle(Device device)
        {
            _device = device;
            _theta = 0d;
            _cameraAngle = new Vector3();
            _detector = new DirectInputDetector();
            _detector.onMouseInputHandleCallback = RotateCamera;
        }

        public void Draw()
        {
            _effect.GetTechniqueByIndex(0).GetPassByIndex(0).Apply(_device.ImmediateContext);
            _device.ImmediateContext.Draw(3, 0);
        }

        public void InitializeContent()
        {
            _effect = CreateEffect();
            _inputLayout = CreateInputLayout();
            _vertexBuffer = CreateVertexBuffer(TriangleVertice);
        }

        public void InitializeTriangleInputAssembler()
        {
            _device.ImmediateContext.InputAssembler.InputLayout = _inputLayout;
            _device.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, VertexPositionColor.SizeInBytes, 0));
            _device.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
        }

        public void Dispose()
        {
            _vertexBuffer?.Dispose();
            _inputLayout?.Dispose();
            _effect?.Dispose();
        }

        private InputLayout CreateInputLayout()
        {
            return new InputLayout(_device, _effect.GetTechniqueByIndex(0).GetPassByIndex(0).Description.Signature,
                new []
                {
                    new InputElement
                    {
                        SemanticName = "SV_Position",
                        Format = SlimDX.DXGI.Format.R32G32B32_Float
                    },
                    new InputElement
                    {
                        SemanticName = "COLOR",
                        Format = SlimDX.DXGI.Format.R32G32B32_Float,
                        AlignedByteOffset = InputElement.AppendAligned//自動的にオフセット決定
                    }
                });
        }

        private Buffer CreateVertexBuffer(System.Array vertice)
        {
            DataStream stream = new DataStream(vertice, true, true);
            return new Buffer(_device, stream,
                new BufferDescription
                {
                    SizeInBytes = (int)stream.Length,
                    BindFlags = BindFlags.VertexBuffer,
                });
        }

        private Effect CreateEffect()
        {
            ShaderBytecode shader = ShaderBytecode.Compile(Properties.Resource1.EffectTest, "fx_5_0", ShaderFlags.None, EffectFlags.None);
            return new Effect(_device, shader);
        }

        public void SetView(System.Windows.Forms.Form form)
        {
            _detector.CheckMouseInput();
            Matrix view = Matrix.LookAtLH(
                new Vector3(0, 0, -3f).RotateByAxis(MathUtility.Axis.Y, _cameraAngle.X).RotateByAxis(MathUtility.Axis.X, _cameraAngle.Y),
                new Vector3(),
                new Vector3(0, 1, 0)
            );

            Matrix projection = Matrix.PerspectiveFovLH(
                (float)System.Math.PI / 2,
                form.ClientSize.Width / form.ClientSize.Height,
                0.1f, 1000
            );

            _effect.GetVariableByName("ViewProjection").AsMatrix().SetMatrix(view * projection);
        }

        private void Rotate(DirectionX dir)
        {
            if(dir == DirectionX.Left)
            {
                _theta += System.Math.PI / 360d;
            }
            else if(dir == DirectionX.Right)
            {
                _theta -= System.Math.PI / 360d;
            }
        }

        private void RotateCamera(int x, int y, int z)
        {
            _cameraAngle += new Vector3((float)x * (float)(System.Math.PI / 360d), (float)y * (float)(System.Math.PI / 360d), z);
            _cameraAngle = new Vector3(_cameraAngle.X, _cameraAngle.Y.Clamp<float>((float)-System.Math.PI / 2f, (float)System.Math.PI / 2f), _cameraAngle.Z);
        }

        private System.Array TriangleVertice
        {
            get
            {
                return new[]
                {
                    new VertexPositionColor
                    {
                        Position = new Vector3(0, 1, 0).RotateByAxis(MathUtility.Axis.Z, _theta),
                        Color = new Vector3(0, 0, 1),
                    },
                    new VertexPositionColor
                    {
                        Position = new Vector3(1, 0, 0).RotateByAxis(MathUtility.Axis.Z, _theta),
                        Color = new Vector3(0, 1, 0)
                    },
                    new VertexPositionColor
                    {
                        Position = new Vector3(-1, 0, 0).RotateByAxis(MathUtility.Axis.Z, _theta),
                        Color = new Vector3(1, 0, 0)
                    },
                };
            }
        }

        struct VertexPositionColor
        {
            public Vector3 Position;
            public Vector3 Color;

            public static readonly InputElement[] VertexElements = new[]
            {
                new InputElement
                {
                    SemanticName = "SV_Position",
                    Format = SlimDX.DXGI.Format.R32G32B32_Float
                },
                new InputElement
                {
                    SemanticName = "COLOR",
                    Format = SlimDX.DXGI.Format.R32G32B32_Float,
                    AlignedByteOffset = InputElement.AppendAligned//自動的にオフセット決定
                }
            };

            public static int SizeInBytes
            {
                get
                {
                    return System.Runtime.InteropServices.
                        Marshal.SizeOf(typeof(VertexPositionColor));
                }
            }
        }
    }
}
