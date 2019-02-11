using System;
using System.Drawing;
using System.Windows.Forms;
using SlimDX;

namespace _3dgrowth
{
    public class RayCast
    {
        private RendererBase _baseObject;

        protected KeyMover _objectMover;
        protected MouseRotator _rotator;
        protected MouseDetector _mouseDetector;

        protected Vector3 _cameraPosition = Vector3.UnitZ * -8;
        private Vector3 _cachedPosition;

        private float _moveScale = 0.1f;
        private D3D11Form _form;

        public RayCast()
        {
            _mouseDetector = new MouseDetector();
            _mouseDetector.SetEvent();

            _rotator = new MouseRotator();
            _rotator.SetEvent();

            _objectMover = new KeyMover();
            _objectMover.OnWKeyAction = () =>
            {
                Vector3 delta = (_cachedPosition * -1);
                delta.Normalize();
                _cachedPosition += delta * _moveScale;
            };
            _objectMover.OnSKeyAction = () =>
            {
                Vector3 delta = _cachedPosition;
                delta.Normalize();
                _cachedPosition += delta * _moveScale;
            };
            _cachedPosition = _cameraPosition;
        }

        public void SetForm(D3D11Form form)
        {
            _form = form;
            _form.MouseClick += new MouseEventHandler(_mouseDetector.MouseClick);
        }

        public void SetObject(RendererBase baseObject)
        {
            _baseObject = baseObject;
        }

        public void OnUpdate()
        {
            _mouseDetector.OnUpdate();

            _rotator.OnUpdate();
            _cameraPosition = _cachedPosition.RotateByAxis(MathUtility.Axis.Y, -_rotator.AngleX).RotateByAxis(MathUtility.Axis.X, -_rotator.AngleY);

            _objectMover.OnUpdate();
            _mouseDetector.onMousePointerDownStateChangedCallback += CheckRayCast;

            _baseObject.InitializeContent();
            _baseObject.SetCamera(_cameraPosition);
            _baseObject.SetView();
            _baseObject.Draw();
            _baseObject.Dispose();
        }

        private void CheckRayCast()
        {
            var cp = _form.PointToClient(_mouseDetector.Pointer);
            SlimDX.Vector3 mousePos = new Vector3(cp.X, cp.Y, 0f);
            var viewPortMat = new Matrix();
            viewPortMat.M11 = _form.ClientSize.Width / 2;
            viewPortMat.M12 = 0;
            viewPortMat.M13 = 0;
            viewPortMat.M14 = 0;
            viewPortMat.M21 = 0;
            viewPortMat.M22 = - _form.ClientSize.Height / 2;
            viewPortMat.M23 = 0;
            viewPortMat.M24 = 0;
            viewPortMat.M31 = 0;
            viewPortMat.M32 = 0;
            viewPortMat.M33 = 1;
            viewPortMat.M34 = 0;
            viewPortMat.M41 = _form.ClientSize.Width / 2;
            viewPortMat.M42 = _form.ClientSize.Height /2;
            viewPortMat.M43 = 0;
            viewPortMat.M44 = 1;

            Console.WriteLine(cp);
            Console.WriteLine(cp.X);
            Console.WriteLine(cp.Y);

            var tmp = Matrix.Invert(viewPortMat) * Matrix.Invert(_baseObject.ProjectionMat) * Matrix.Invert(_baseObject.ViewMat);
            var worldPos = Vector3.TransformCoordinate(mousePos, tmp);
            var vec = Vector3.UnitZ.RotateByAxis(MathUtility.Axis.Y, -_rotator.AngleX)
                .RotateByAxis(MathUtility.Axis.X, -_rotator.AngleY);

            worldPos += vec;
            worldPos = Vector3.Normalize(worldPos);

            var a = Vector3.Dot(worldPos, worldPos);
            var b = Vector3.Dot(worldPos, _baseObject.ModelPosition);
            var c = Vector3.Dot(_baseObject.ModelPosition, _baseObject.ModelPosition) - 0.25f;

            var sphere = _baseObject as HitSphere;

            sphere.SetHit(b * b - a * c >= 0);
        }
    }
}
