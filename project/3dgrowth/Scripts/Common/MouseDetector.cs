using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using Action = System.Action;

namespace _3dgrowth
{
    public class MouseDetector
    {
        private DirectInputDetector _detector;

        private bool _isPointerDown;
        public bool IsPointerDown => _isPointerDown;

        private Point _pointer;
        public Point Pointer => _pointer;

        public Action onMousePointerDownStateChangedCallback;

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

        }

        public virtual void SetEvent()
        {
            _detector.onMousePointerDownCallback += OnMousePointerDowned;
            _detector.onMouseInputHandleCallback += OnMousePositionChanged;
        }

        public void MouseClick(object sender, MouseEventArgs e)
        {
            _pointer = Cursor.Position;
            onMousePointerDownStateChangedCallback?.Invoke();
        }
    }
}
