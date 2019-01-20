using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;
using System.Drawing;
using System.Drawing.Imaging;

namespace _3dgrowth
{
    /// <summary>
    /// ゲート1:四角形の描画
    /// </summary>
    public abstract class RendererBase : System.IDisposable
    {
        protected System.Windows.Forms.Form _form;
        protected Device _device;
        protected Buffer _indexBuffer;
        protected Buffer _vertexBuffer;
        protected InputLayout _inputLayout;
        protected Effect _effect;
        protected Vector3 _position = Vector3.Zero;
        protected Vector3 _cameraPosition = new Vector3(0, 0, -3f);
        protected float _scale = 1;

        protected virtual int IndexSize => 6;

        public virtual Vector3 EyePosition => _cameraPosition;
        public virtual Vector3 ModelPosition => UseModel ? _position : Vector3.Zero;

        protected virtual Vector3 CameraDirection => new Vector3();
        protected virtual Vector3 CameraAxis => new Vector3(0, 1, 0);

        protected virtual bool UseModel => false;
        protected virtual float Fov => (float)System.Math.PI / 2;
        protected virtual float Aspect => _form.ClientSize.Width / _form.ClientSize.Height;
        protected virtual float Znear => 0.1f;
        protected virtual float Zfar => 1000;

        protected virtual string ShaderSource => Properties.Resource1.Ambient;

        public RendererBase(Device device, System.Windows.Forms.Form form)
        {
            _device = device;
            _form = form;
        }

        public virtual void InitializeContent()
        {
            _effect = CreateEffect();
            _inputLayout = CreateInputLayout();
            _indexBuffer = CreateIndexBuffer(IndexList);
            _vertexBuffer = CreateVertexBuffer(VerticeList);
        }

        public virtual void Draw()
        {
            InitializeTriangleInputAssembler();
            PreDraw();
            _device.ImmediateContext.DrawIndexed(IndexSize, 0, 0);
        }

        protected virtual void PreDraw()
        {
            SetTexture();
            _effect.GetTechniqueByIndex(0).GetPassByIndex(0).Apply(_device.ImmediateContext);
        }

        protected virtual void InitializeTriangleInputAssembler()
        {
            _device.ImmediateContext.Rasterizer.State =
                DeviceSetting.DeviceDefine.GetRasterizerState(_device);
            _device.ImmediateContext.InputAssembler.InputLayout = _inputLayout;
            _device.ImmediateContext.InputAssembler.SetIndexBuffer(_indexBuffer, SlimDX.DXGI.Format.R32_UInt, 0);
            _device.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, VertexOutput.SizeInBytes, 0));
            _device.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
        }

        public virtual void Dispose()
        {
            _vertexBuffer?.Dispose();
            _inputLayout?.Dispose();
            _indexBuffer?.Dispose();
            _effect?.Dispose();
        }

        public virtual void Move(Vector3 position)
        {
            _position += position;
        }

        public virtual void SetPosition(Vector3 position)
        {
            _position = position;
        }

        public virtual void SetCamera(Vector3 position)
        {
            _cameraPosition = position;
        }

        public virtual void SetScale(float scale)
        {
            _scale = scale;
        }

        protected virtual InputLayout CreateInputLayout()
        {
            return new InputLayout(_device, _effect.GetTechniqueByIndex(0).GetPassByIndex(0).Description.Signature,
                new[]
                {
                    new InputElement
                    {
                        SemanticName = "SV_Position",
                        Format = SlimDX.DXGI.Format.R32G32B32_Float
                    },
                    new InputElement
                    {
                        SemanticName = "NORMAL",
                        Format = SlimDX.DXGI.Format.R32G32B32_Float,
                        AlignedByteOffset = InputElement.AppendAligned//自動的にオフセット決定
                    },
                    new InputElement
                    {
                        SemanticName = "TEXCOORD",
                        Format = SlimDX.DXGI.Format.R32G32_Float,
                        AlignedByteOffset = InputElement.AppendAligned//自動的にオフセット決定
                    }
                });
        }

        protected virtual Buffer CreateIndexBuffer(System.Array indexes)
        {
            using (DataStream stream = new DataStream(indexes, true, true))
            {
                return new Buffer(
                    _device,
                    stream,
                    sizeof(uint) * IndexSize,
                    ResourceUsage.Default,
                    BindFlags.IndexBuffer,
                    0,
                    0,
                    0
                );
            }
        }

        protected virtual Buffer CreateVertexBuffer(System.Array vertice)
        {
            using (DataStream stream = new DataStream(vertice, true, true))
            {
                return new Buffer(_device, stream,
                new BufferDescription
                {
                    SizeInBytes = (int)stream.Length,
                    BindFlags = BindFlags.VertexBuffer,
                });
            }
        }

        protected virtual Effect CreateEffect()
        {
            using (ShaderBytecode shader = ShaderBytecode.Compile(ShaderSource, "fx_5_0", ShaderFlags.None, EffectFlags.None))
            {
                return new Effect(_device, shader);
            }
        }

        public virtual void SetView()
        {
            Matrix model = Matrix.Translation(ModelPosition);

            Matrix view = Matrix.LookAtLH(
                EyePosition,
                CameraDirection,
                CameraAxis
            );

            Matrix projection = Matrix.PerspectiveFovLH(
                Fov,
                Aspect,
                Znear,
                Zfar
            );

            _effect.GetVariableByName("Model").AsMatrix().SetMatrix(model);
            _effect.GetVariableByName("ViewProjection").AsMatrix().SetMatrix(view * projection);
        }

        public void SetTexture()
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                Image img = Properties.Resource1.Penguins;
                img.Save(ms, ImageFormat.Jpeg);

                using (ShaderResourceView texture = ShaderResourceView.FromMemory(_device, ms.ToArray()))
                {
                    _effect.GetVariableByName("diffuseTexture").AsResource().SetResource(texture);
                }
            }
        }

        #region Define

        protected virtual System.Array IndexList
        {
            get
            {
                return new uint[]
                {
                    0, 1, 2, 1, 3, 0
                };
            }
        }

        protected virtual System.Array VerticeList
        {
            get
            {
                return new[]
                {
                    new VertexOutput
                    {
                        Position = new Vector3(-1, 1, 0),
                        TextureCoordinate = new Vector2(0, 0)
                    },

                    new VertexOutput
                    {
                        Position = new Vector3(1, -1, 0),
                        TextureCoordinate = new Vector2(1, 1)
                    },

                    new VertexOutput
                    {
                        Position = new Vector3(-1, -1, 0),
                        TextureCoordinate = new Vector2(0, 1)
                    },

                    new VertexOutput
                    {
                        Position = new Vector3(1, 1, 0),
                        TextureCoordinate = new Vector2(1, 0)
                    },
                };
            }
        }

        protected struct VertexOutput
        {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector2 TextureCoordinate;

            public static readonly InputElement[] VertexElements = new[]
            {
                new InputElement
                {
                     SemanticName = "SV_Position",
                     Format = SlimDX.DXGI.Format.R32G32B32_Float
                },
                new InputElement
                {
                     SemanticName = "NORMAL0",
                     Format = SlimDX.DXGI.Format.R32G32B32A32_Float,
                     AlignedByteOffset = InputElement.AppendAligned//自動的にオフセット決定
                },
                new InputElement
                {
                    SemanticName = "TEXCOORD",
                    Format = SlimDX.DXGI.Format.R32G32_Float,
                    AlignedByteOffset = InputElement.AppendAligned//自動的にオフセット決定
                }
            };

            public static int SizeInBytes
            {
                get
                {
                    return System.Runtime.InteropServices.
                        Marshal.SizeOf(typeof(VertexOutput));
                }
            }
        }
        #endregion
    }
}
