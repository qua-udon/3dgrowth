using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D11;

namespace _3dgrowth
{
    /// <summary>
    /// ゲート0: 初期化
    /// </summary>
    public class DeviceSetting : System.IDisposable
    {
        public Device _device;
        public Device Device => _device;

        private SlimDX.DXGI.SwapChain _swapChain;
        public SlimDX.DXGI.SwapChain SwapChain => _swapChain;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="form">ウィンドウフォーム</param>
        public void InitializeDevice(Form form)
        {
            Device.CreateWithSwapChain(
                       DriverType.Hardware,
                       DeviceCreationFlags.None,
                       DeviceDefine.GetSwapChainDescriptionDefine(form),
                       out _device,
                       out _swapChain);

            _device.ImmediateContext.Rasterizer.State = DeviceDefine.GetRasterizerState(_device);
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Dispose()
        {
            _device.Dispose();
            _swapChain.Dispose();
        }

        /// <summary>
        /// 初期化に必要な構造体の設定をまとめたもの
        /// </summary>
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

            public static RasterizerState GetRasterizerState(Device device)
            {
                return RasterizerState.FromDescription(device,
                    new RasterizerStateDescription
                    {
                        CullMode = CullMode.None,
                        FillMode = FillMode.Solid
                    });
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
