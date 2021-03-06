﻿using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;
using System.Drawing;
using System.Drawing.Imaging;

namespace _3dgrowth
{
    /// <summary>
    /// ゲート1:四角形の描画
    /// </summary>
    public class DrawRectangle : System.IDisposable
    {
        private Device _device;
        private Buffer _indexBuffer;
        private Buffer _vertexBuffer;
        private InputLayout _inputLayout;
        private Effect _effect;

        public DrawRectangle(Device device)
        {
            _device = device;
        }

        public void Draw()
        {
            SetTexture();
            _effect.GetTechniqueByIndex(0).GetPassByIndex(0).Apply(_device.ImmediateContext);
            _device.ImmediateContext.DrawIndexed(6, 0, 0);
        }

        public void InitializeContent()
        {
            _effect = CreateEffect();
            _inputLayout = CreateInputLayout();
            _indexBuffer = CreateIndexBuffer(IndexList);
            _vertexBuffer = CreateVertexBuffer(TriangleVertice);
        }

        public void InitializeTriangleInputAssembler()
        {
            _device.ImmediateContext.InputAssembler.InputLayout = _inputLayout;
            _device.ImmediateContext.InputAssembler.SetIndexBuffer(_indexBuffer, SlimDX.DXGI.Format.R32_UInt, 0);
            _device.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, VertexPositionTexture.SizeInBytes, 0));
            _device.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
        }

        public void Dispose()
        {
            _vertexBuffer?.Dispose();
            _inputLayout?.Dispose();
            _indexBuffer?.Dispose();
            _effect?.Dispose();
        }

        private InputLayout CreateInputLayout()
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
                        SemanticName = "TEXCOORD",
                        Format = SlimDX.DXGI.Format.R32G32_Float,
                        AlignedByteOffset = InputElement.AppendAligned//自動的にオフセット決定
                    }
                });
        }

        private Buffer CreateIndexBuffer(System.Array indexes)
        {
            using (DataStream stream = new DataStream(indexes, true, true))
            {
                return new Buffer(
                    _device,
                    stream,
                    sizeof(uint) * 6,
                    ResourceUsage.Default,
                    BindFlags.IndexBuffer,
                    0,
                    0,
                    0
                );
            }
        }

        private Buffer CreateVertexBuffer(System.Array vertice)
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

        private Effect CreateEffect()
        {
            using (ShaderBytecode shader = ShaderBytecode.Compile(Properties.Resource1.EffectTest, "fx_5_0", ShaderFlags.None, EffectFlags.None))
            {
                return new Effect(_device, shader);
            }
        }

        public void SetView(System.Windows.Forms.Form form)
        {
            Matrix view = Matrix.LookAtLH(
                new Vector3(0, 0, -3f),
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

        private static System.Array IndexList
        {
            get
            {
                return new uint[]
                {
                    0, 1, 2, 1, 3, 0
                };
            }
        }

        private static System.Array TriangleVertice
        {
            get
            {
                return new[]
                {
                    new VertexPositionTexture
                    {
                        Position = new Vector3(-1, 1, 0),
                        TextureCoordinate = new Vector2(0, 0)
                    },

                    new VertexPositionTexture
                    {
                        Position = new Vector3(1, -1, 0),
                        TextureCoordinate = new Vector2(1, 1)
                    },

                    new VertexPositionTexture
                    {
                        Position = new Vector3(-1, -1, 0),
                        TextureCoordinate = new Vector2(0, 1)
                    },

                    new VertexPositionTexture
                    {
                        Position = new Vector3(1, 1, 0),
                        TextureCoordinate = new Vector2(1, 0)
                    },
                };
            }
        }

        private struct VertexPositionTexture
        {
            public Vector3 Position;
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
                        Marshal.SizeOf(typeof(VertexPositionTexture));
                }
            }
        }
        #endregion
    }
}
