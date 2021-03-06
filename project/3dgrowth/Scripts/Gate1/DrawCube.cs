﻿using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;

namespace _3dgrowth
{
    /// <summary>
    /// ゲート1:キューブの描画
    /// </summary>
    public class DrawCube : System.IDisposable
    {
        private Device _device;
        private Buffer _vertexBuffer;
        private InputLayout _inputLayout;
        private Effect _effect;

        public DrawCube(Device device)
        {
            _device = device;
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
            _device.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, sizeof(float) * 3, 0));
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
                new[]
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
            ShaderBytecode shader = ShaderBytecode.CompileFromFile("C:/work/3dgrowth/project/3dgrowth/EffectTest.fx", "fx_5_0", ShaderFlags.None, EffectFlags.None);
            return new Effect(_device, shader);
        }

        public void SetView(System.Windows.Forms.Form form)
        {
            double time = System.Environment.TickCount / 500d;
            Matrix view = Matrix.LookAtRH(
                new Vector3(0f, 0f, -3f),
                new Vector3(),
                new Vector3(0, 1, 0)
            );

            Matrix projection = Matrix.PerspectiveFovRH(
                (float)System.Math.PI / 2,
                form.ClientSize.Width / form.ClientSize.Height,
                0.1f, 1000
            );

            _effect.GetVariableByName("ViewProjection").AsMatrix().SetMatrix(view * projection);
        }

        private static System.Array TriangleVertice
        {
            get
            {
                return new[]
                {
                    new Vector3(-1f, 0f, 0f),
                    new Vector3(1f, 0f, 0f),
                    new Vector3(0f, 1f, 0f),
                };
            }
        }
    }
}
