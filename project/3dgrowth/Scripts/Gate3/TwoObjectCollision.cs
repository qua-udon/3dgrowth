using SlimDX;
using SlimDX.Direct3D11;

namespace _3dgrowth
{
    public abstract class TwoObjectCollision
    {
        public enum ObjectType
        {
            Box,
            Sphere,
            Capsule,
        }

        protected RendererBase _baseObject;
        protected RendererBase _moveObject;

        protected KeyMover _objectMover;
        protected MouseRotator _rotator;

        protected Vector3 _cameraPosition = Vector3.UnitZ * -8;
        private Vector3 _cachedPosition;

        public TwoObjectCollision()
        {
            _rotator = new MouseRotator();
            _rotator.SetEvent();

            _objectMover = new KeyMover();
            _objectMover.OnLeftArrowAction = () => _moveObject.Move(Vector3.UnitX * -1);
            _objectMover.OnRightArrowAction = () => _moveObject.Move(Vector3.UnitX);
            _objectMover.OnDownArrowAction = () => _moveObject.Move(Vector3.UnitZ * -1);
            _objectMover.OnUpArrowAction = () => _moveObject.Move(Vector3.UnitZ);
            _objectMover.OnEKeyAction = () => _moveObject.Move(Vector3.UnitY);
            _objectMover.OnQKeyAction = () => _moveObject.Move(Vector3.UnitY * -1);
            _objectMover.OnWKeyAction = () =>
            {
                Vector3 delta = (_cachedPosition * -1);
                delta.Normalize();
                _cachedPosition += delta;
            };
            _objectMover.OnSKeyAction = () =>
            {
                Vector3 delta = _cachedPosition;
                delta.Normalize();
                _cachedPosition += delta;
            };
            _cachedPosition = _cameraPosition;
        }

        public virtual void SetObject(RendererBase baseObject, RendererBase moveObject)
        {
            _baseObject = baseObject;
            _moveObject = moveObject;
        }

        public void OnUpdate()
        {
            _rotator.OnUpdate();
            _cameraPosition = _cachedPosition.RotateByAxis(MathUtility.Axis.Y, -_rotator.AngleX).RotateByAxis(MathUtility.Axis.X, -_rotator.AngleY);

            _objectMover.OnUpdate();
            CheckCollision();

            _baseObject.InitializeContent();
            _baseObject.SetCamera(_cameraPosition);
            _baseObject.SetView();
            _baseObject.Draw();
            _baseObject.Dispose();

            _moveObject.InitializeContent();
            _moveObject.SetCamera(_cameraPosition);
            _moveObject.SetView();
            _moveObject.Draw();
            _moveObject.Dispose();
        }

        protected abstract void CheckCollision();
    }
}
