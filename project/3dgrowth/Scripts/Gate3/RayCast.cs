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
            CheckCollision();

            _baseObject.InitializeContent();
            _baseObject.SetCamera(_cameraPosition);
            _baseObject.SetView();
            _baseObject.Draw();
            _baseObject.Dispose();
        }

        private void CheckRayCast()
        {

        }
    }
}
