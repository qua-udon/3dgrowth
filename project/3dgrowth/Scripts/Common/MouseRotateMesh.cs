using SlimDX;
using SlimDX.Direct3D11;

namespace _3dgrowth
{
    public class MouseRotateMesh : RendererBase
    {
        private MouseRotator _rotator;

        protected override Vector3 EyePosition => base.EyePosition.RotateByAxis(MathUtility.Axis.Y, -_rotator.AngleX).RotateByAxis(MathUtility.Axis.X, -_rotator.AngleY);

        public MouseRotateMesh(Device device, System.Windows.Forms.Form form) : base(device, form)
        {
            _rotator = new MouseRotator();
            _rotator.SetEvent();
        }

        public override void SetView()
        {
            _rotator.OnUpdate();
            base.SetView();
        }
    }
}
