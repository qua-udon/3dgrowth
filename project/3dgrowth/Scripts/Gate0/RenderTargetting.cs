using SlimDX.Direct3D11;

namespace _3dgrowth
{
    public class RenderTargetting
    {
        private Device _device;
        private SlimDX.DXGI.SwapChain _swapChain;
        private RenderTargetView _renderTargetView;

        public RenderTargetting(Device device, SlimDX.DXGI.SwapChain swapChain)
        {
            _device = device;
            _swapChain = swapChain;

            using (Texture2D backBuffer = Resource.FromSwapChain<Texture2D>(swapChain, 0))
            {
                _renderTargetView = new RenderTargetView(device, backBuffer);
                _device.ImmediateContext.OutputMerger.SetTargets(_renderTargetView);
            }
        }

        public void Clear()
        {
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
    }
}
