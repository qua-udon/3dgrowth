using System;
using Microsoft.DirectX.DirectInput;
using SlimDX;
using SlimDX.D3DCompiler;
using SlimDX.Direct3D11;
using Device = SlimDX.Direct3D11.Device;
using Effect = SlimDX.Direct3D11.Effect;

namespace _3dgrowth
{
    public class DrawSphere : MouseRotateMesh
    {
        protected override int IndexSize => 6 * _separateX * _separateY;

        protected override System.Array IndexList => GetSphereIndexes();
        protected override System.Array VerticeList => GetSphereVertices();

        protected override string ShaderSource => Properties.Resource1.BlinnPhong;

        private SlimDX.Direct3D11.Buffer _constantBuffer;
        private DirectInputDetector _detector;
        private bool _isWire;

        private double _radius = 1d;
        private int _separateX = 20;
        private int _separateY = 20;

        public DrawSphere(Device device, System.Windows.Forms.Form form) : base(device, form)
        {
            _detector = new DirectInputDetector(Microsoft.DirectX.DirectInput.SystemGuid.Keyboard);
            _detector.SetDownKey(Microsoft.DirectX.DirectInput.Key.D2);
        }

        public override void Dispose()
        {
            base.Dispose();
            _constantBuffer?.Dispose();
        }

        public override void InitializeContent()
        {
            base.InitializeContent();
            _constantBuffer = new SlimDX.Direct3D11.Buffer(
                _device,
                new BufferDescription
                {
                    SizeInBytes = sizeof(float) * 4,
                    BindFlags = BindFlags.ConstantBuffer
                });
        }

        public override void Draw()
        {
            base.Draw();
            if (_detector.CheckKeyBoardDownInputRegister(Key.D2))
            {
                _isWire = !_isWire;
            }

            if (_isWire)
            {
                using (ShaderBytecode shader = ShaderBytecode.Compile(Properties.Resource1.WireFrame, "fx_5_0", ShaderFlags.None, SlimDX.D3DCompiler.EffectFlags.None))
                {
                    _effect = new Effect(_device, shader);
                }
                SetView();
                SetEyePositionBuffer();
                PreDraw();
                _device.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;
                _device.ImmediateContext.DrawIndexed(IndexSize, 0, 0);
            }
        }

        private void SetEyePositionBuffer()
        {
            Constant constant = new Constant
            {
                EyePositionX = EyePosition.X,
                EyePositionY = EyePosition.Y,
                EyePositionZ = EyePosition.Z
            };

            DataBox data = new DataBox(0, 0, new DataStream(new [] { constant }, true, true));
            _device.ImmediateContext.UpdateSubresource(data, _constantBuffer, 0);
            _effect.GetConstantBufferByName("Constant").ConstantBuffer = _constantBuffer;
        }

        public override void SetView()
        {
            base.SetView();
            SetEyePositionBuffer();
        }

        private struct Constant
        {
            public float EyePositionX;
            public float EyePositionY;
            public float EyePositionZ;
        }

        private System.Array GetSphereIndexes()
        {
            uint[] indexes = new uint[IndexSize];
            int indexCount = 0;

            for (int y = 0; y < _separateY; y++)
            {
                for (int x = 0; x < _separateX / 4; x++)
                {
                    if(x == _separateX - 1)
                    {

                    }

                    int next = x == _separateX - 1 ? 0 : x + 1;
                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);

                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);
                    indexes[indexCount++] = (uint)(y * _separateX + x + _separateX);
                }

                for (int x = _separateX - 1; x >= _separateX * 3 / 4; x--)
                {
                    if (x == _separateX - 1)
                    {

                    }

                    int next = x == _separateX - 1 ? 0 : x + 1;
                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);

                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);
                    indexes[indexCount++] = (uint)(y * _separateX + x + _separateX);
                }

                for (int x = _separateX / 2 + 1; x < _separateX * 3 / 4; x++)
                {
                    if (x == _separateX - 1)
                    {

                    }

                    int next = x == _separateX - 1 ? 0 : x + 1;
                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);

                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);
                    indexes[indexCount++] = (uint)(y * _separateX + x + _separateX);
                }

                for (int x = _separateX / 2; x >= _separateX / 4; x--)
                {
                    if (x == _separateX - 1)
                    {

                    }

                    int next = x == _separateX - 1 ? 0 : x + 1;
                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);

                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);
                    indexes[indexCount++] = (uint)(y * _separateX + x + _separateX);
                }
            }

            return indexes;
        }

        private System.Array GetSphereVertices()
        {
            int vertexCount = _separateX * (_separateY + 1);
            VertexOutput[] vertices = new VertexOutput[vertexCount];

            for(int y = 0; y <= _separateY; y++)
            {
                for(int x = 0; x < _separateX; x++)
                {
                    double theta = Math.PI * (double)y / (double) _separateY;
                    double phi = Math.PI * 2d * (double)x / (double) _separateX;
                    double xPos = _radius * Math.Sin(theta) * Math.Cos(phi);
                    double yPos = _radius * Math.Cos(theta);
                    double zPos = _radius * Math.Sin(theta) * Math.Sin(phi);
                    double u = (x * 2) % _separateX == 0 ? 1d : (double)((x * 2) % _separateX) / _separateX;
                    double v = y == _separateY ? 1d : (double)(y % _separateY) / _separateY;
                    if(x > 10)
                    {
                        u = 1f - u;
                    }
                    if(x == 0 || x == 19)
                    {
                        u = 0f;
                    }
                    VertexOutput vert = new VertexOutput
                    {
                        Position = new SlimDX.Vector3((float)xPos, (float)yPos, (float)zPos),
                        Normal = new SlimDX.Vector3((float)xPos, (float)yPos, (float)zPos),
                        TextureCoordinate = new SlimDX.Vector2((float)u, (float)v)                      
                    };
                    vertices[_separateX * y + x] = vert;
                }
            }

            return vertices;
        }

        private System.Array GetWireSphereVertices()
        {
            int vertexCount = _separateX * (_separateY + 1);
            VertexOutput[] vertices = new VertexOutput[vertexCount];

            for (int y = 0; y <= _separateY; y++)
            {
                for (int x = 0; x < _separateX; x++)
                {
                    double theta = Math.PI * (double)y / (double)_separateY;
                    double phi = Math.PI * 2d * (double)x / (double)_separateX;
                    double xPos = _radius * Math.Sin(theta) * Math.Cos(phi);
                    double yPos = _radius * Math.Cos(theta);
                    double zPos = _radius * Math.Sin(theta) * Math.Sin(phi);
                    double u = (x * 2) % _separateX == 0 ? 1d : (double)((x * 2) % _separateX) / _separateX;
                    double v = y == _separateY ? 1d : (double)(y % _separateY) / _separateY;
                    if (x > 10)
                    {
                        u = 1f - u;
                    }
                    if (x == 0 || x == 19)
                    {
                        u = 0f;
                    }
                    VertexOutput vert = new VertexOutput
                    {
                        Position = new SlimDX.Vector3((float)xPos, (float)yPos, (float)zPos),
                        Normal = new SlimDX.Vector3((float)xPos, (float)yPos, (float)zPos),
                        TextureCoordinate = new SlimDX.Vector2((float)u, (float)v)
                    };
                    vertices[_separateX * y + x] = vert;
                }
            }

            return vertices;
        }
    }
}
