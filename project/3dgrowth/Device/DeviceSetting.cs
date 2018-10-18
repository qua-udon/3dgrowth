using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D11;

namespace _3dgrowth
{
    public class DeviceSetting
    {
        private Device _device;
        private SlimDX.DXGI.SwapChain _swapChain;

        public void InitializeDevice(Form form)
        {
            Device.CreateWithSwapChain(
                       DriverType.Hardware,
                       DeviceCreationFlags.None,
                       DeviceDefine.GetSwapChainDescriptionDefine(form),
                       out _device,
                       out _swapChain);
        }

        public void DisposeDevice()
        {
            _device.Dispose();
            _swapChain.Dispose();
        }

        private static class DeviceDefine
        {
            /// <summary>
            /// スワップチェーン情報
            /// </summary>
            /// <param name="form"ウィンドウフォーム></param>
            public static SlimDX.DXGI.SwapChainDescription GetSwapChainDescriptionDefine(Form form)
            {
                return new SlimDX.DXGI.SwapChainDescription
                {
                    BufferCount = 1,
                    OutputHandle = form.Handle,
                    IsWindowed = true,
                    SampleDescription = SampleDescriptionDefine,
                    ModeDescription = GetModeDescriptionDefine(form),
                    Usage = SlimDX.DXGI.Usage.RenderTargetOutput
                };
            }

            /// <summary>
            /// サンプル設定
            /// </summary>
            /// <param name="form">ウィンドウフォーム</param>
            private static SlimDX.DXGI.SampleDescription SampleDescriptionDefine
            {
                get
                {
                    return new SlimDX.DXGI.SampleDescription
                    {
                        Count = 1,
                        Quality = 0,
                    };
                }
            }

            /// <summary>
            /// 表示系設定
            /// </summary>
            /// <param name="form">ウィンドウフォーム</param>
            private static SlimDX.DXGI.ModeDescription GetModeDescriptionDefine(Form form)
            {
                return new SlimDX.DXGI.ModeDescription
                {
                    Width = form.ClientSize.Width,
                    Height = form.ClientSize.Height,
                    RefreshRate = RefreshRateDefine,
                    Format = SlimDX.DXGI.Format.R8G8B8A8_UNorm
                };
            }

            /// <summary>
            /// リフレッシュレート
            /// </summary>
            private static Rational RefreshRateDefine
            {
                get
                {
                    return new Rational(60, 1);
                }
            }
        }
    }
}
