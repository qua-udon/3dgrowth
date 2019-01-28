using System;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;

namespace _3dgrowth
{
    public class MouseDetector
    {
        private DirectInputDetector _detector;

        private bool _isPointerDown;
        public bool IsPointerDown => _isPointerDown;

        private Vector3 _pointer;
        public Vector3 Pointer => _pointer;

        public Action<int, int> onMousePointerChanged;

        public MouseDetector()
        {
            _detector = new DirectInputDetector(SystemGuid.Mouse);
        }

        public virtual void OnUpdate()
        {
            _detector.CheckMouseInput();
        }

        public virtual void OnMousePointerDowned(bool down)
        {
            _isPointerDown = down;
        }

        public virtual void OnMousePositionChanged(int x, int y, int z)
        {
            _pointer = new Vector3(x, y, z);
        }

        public virtual void SetEvent()
        {
            _detector.onMousePointerDownCallback = OnMousePointerDowned;
            _detector.onMouseInputHandleCallback = OnMousePositionChanged;
        }
    }
}
