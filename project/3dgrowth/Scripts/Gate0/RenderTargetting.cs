using SlimDX.Direct3D11;
using SlimDX.DXGI;
using Device = SlimDX.Direct3D11.Device;
using Resource = SlimDX.Direct3D11.Resource;

namespace _3dgrowth
{
    public class RenderTargetting
    {
        private readonly Device _device;
        private readonly SlimDX.DXGI.SwapChain _swapChain;
        private readonly RenderTargetView _renderTargetView;
        private readonly DepthStencilView _depthStencil;
        private readonly int _width, _height;

        public RenderTargetting(Device device, SlimDX.DXGI.SwapChain swapChain, int width, int height)
        {
            _device = device;
            _swapChain = swapChain;
            _width = width;
            _height = height;

            using (Texture2D backBuffer = Resource.FromSwapChain<Texture2D>(swapChain, 0))
            {
                using (Texture2D depthBuffer = new Texture2D(device, GetDepthBufferDescription()))
                {
                    _depthStencil = new DepthStencilView(device, depthBuffer);
                    _renderTargetView = new RenderTargetView(device, backBuffer);
                    _device.ImmediateContext.OutputMerger.SetTargets(_depthStencil, _renderTargetView);
                }
            }
        }

        public void Clear()
        {
            _device.ImmediateContext.ClearDepthStencilView(_depthStencil, DepthStencilClearFlags.Depth, 1, 0);
            _device.ImmediateContext.ClearRenderTargetView(_renderTargetView, new SlimDX.Color4(1, 0, 0.6f, 0.2f));
        }

        public void PresentView()
        {
            _swapChain.Present(0, SlimDX.DXGI.PresentFlags.None);
        }

        public void Dispose()
        {
            _renderTargetView.Dispose();
        }

        private Texture2DDescription GetDepthBufferDescription()
        {
            return new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.DepthStencil,
                Format = Format.D32_Float,
                Width = _width,
                Height = _height,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0)
            };
        }
    }
}
