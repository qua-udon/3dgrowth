using System;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D11;

namespace _3dgrowth
{
    public class DrawWireSphere : DrawSphere
    {
        protected override string ShaderSource => Properties.Resource1.WireFrame;

        public DrawWireSphere(Device device, Form form) : base(device, form)
        {
        }

        protected override void InitializeTriangleInputAssembler()
        {
            _device.ImmediateContext.Rasterizer.State =
                DeviceSetting.DeviceDefine.GetRasterizerState(_device);
            _device.ImmediateContext.InputAssembler.InputLayout = _inputLayout;
            _device.ImmediateContext.InputAssembler.SetIndexBuffer(_indexBuffer, SlimDX.DXGI.Format.R32_UInt, 0);
            _device.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, VertexOutput.SizeInBytes, 0));
            _device.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;
        }
    }
}
