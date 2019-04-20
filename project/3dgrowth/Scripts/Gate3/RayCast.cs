using System;
using System.Drawing;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

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

        private SlimDX.Direct3D11.Device _device;

        public RayCast()
        {
            _mouseDetector = new MouseDetector();
            _mouseDetector.SetEvent();
            _mouseDetector.onMousePointerDownStateChangedCallback += CheckRayCast;

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

        public void SetDevice(SlimDX.Direct3D11.Device device)
        {
            _device = device;
        }

        public void SetForm(D3D11Form form)
        {
            _form = form;
            _form.MouseDown += new MouseEventHandler(_mouseDetector.MouseClick);
        }

        public void SetObject(RendererBase baseObject)
        {
            _baseObject = baseObject;
        }

        public void OnUpdate()
        {
            _mouseDetector.OnUpdate();

            _rotator.OnUpdate();
            _cameraPosition = _cachedPosition.RotateByAxis(MathUtility.Axis.Y, -_rotator.AngleX)
                .RotateByAxis(MathUtility.Axis.X, -_rotator.AngleY);

            _objectMover.OnUpdate();

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
            viewPortMat.M22 = -_form.ClientSize.Height / 2;
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

            var nearMat = new Matrix();
            nearMat.M11 = nearMat.M21 = nearMat.M31 = nearMat.M41 = mousePos.X;
            nearMat.M12 = nearMat.M22 = nearMat.M32 = nearMat.M42 = mousePos.Y;
            nearMat.M13 = nearMat.M23 = nearMat.M33 = nearMat.M43 = 0;
            nearMat.M14 = nearMat.M24 = nearMat.M34 = nearMat.M44 = 1;

            var farMat = new Matrix();
            farMat.M11 = farMat.M21 = farMat.M31 = farMat.M41 = mousePos.X;
            farMat.M12 = farMat.M22 = farMat.M32 = farMat.M42 = mousePos.Y;
            farMat.M13 = farMat.M23 = farMat.M33 = farMat.M43 = 1;
            farMat.M14 = farMat.M24 = farMat.M34 = farMat.M44 = 1;

            var nearTmp = nearMat * Matrix.Invert(viewPortMat) * Matrix.Invert(_baseObject.ProjectionMat) * Matrix.Invert(_baseObject.ViewMat);
            var farTmp = farMat * Matrix.Invert(viewPortMat) * Matrix.Invert(_baseObject.ProjectionMat) * Matrix.Invert(_baseObject.ViewMat);

            var nearPos = new Vector3(nearTmp.M11 / nearTmp.M14, nearTmp.M12 / nearTmp.M14, nearTmp.M13 / nearTmp.M14);
            var farPos = new Vector3(farTmp.M11 / farTmp.M14, farTmp.M12 / farTmp.M14, farTmp.M13 / farTmp.M14);
            var vec = (farPos - nearPos);
            vec.Normalize();

            var a = Vector3.Dot(vec, vec);
            var b = Vector3.Dot(vec,  nearPos - _baseObject.ModelPosition);
            var c = Vector3.Dot( nearPos - _baseObject.ModelPosition, nearPos - _baseObject.ModelPosition) - 1.0f;

            var sphere = _baseObject as HitSphere;

            sphere.SetHit(b * b - a * c >= 0);
        }


    }
}
