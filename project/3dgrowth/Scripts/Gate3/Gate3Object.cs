using System;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D11;
using Buffer = SlimDX.Direct3D11.Buffer;

namespace _3dgrowth
{
    public class Gate3Object : DrawCube
    {
        protected override bool UseModel => true;
        protected override string ShaderSource => Properties.Resource1.gate3;

        private Buffer _constantBuffer;
        private Constant _constant;

        public Gate3Object(Device device, Form form) : base(device, form)
        {
            _constant = new Constant();
        }

        public override void InitializeContent()
        {
            base.InitializeContent();
            _constantBuffer = new Buffer(
                _device,
                new BufferDescription
                {
                    SizeInBytes = sizeof(float) * 4,
                    BindFlags = BindFlags.ConstantBuffer
                });

            DataBox data = new DataBox(0, 0, new DataStream(new[] { _constant }, true, true));
            _device.ImmediateContext.UpdateSubresource(data, _constantBuffer, 0);
            _effect.GetConstantBufferByName("Constant").ConstantBuffer = _constantBuffer;
        }

        public void SetHit(bool isHit)
        {
            _constant = new Constant
            {
                IsHit = isHit ? 1 : 0
            };
        }

        public override void Dispose()
        {
            base.Dispose();
            _constantBuffer.Dispose();
        }

        private struct Constant
        {
            public int IsHit;
        }
    }
}
