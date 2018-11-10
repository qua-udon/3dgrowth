using System;
using SlimDX;
using Microsoft.DirectX.DirectInput;

namespace _3dgrowth
{
    public class DirectInputDetector
    {
        private Device _device;

        public Action<int, int, int> onMouseInputHandleCallback;
        public Action<DirectionX, DirectionY> onKeyInputHandleCallback;

        private Vector3 _cachedMousePos;

        public DirectInputDetector()
        {
            _device = new Device(SystemGuid.Mouse);
            _device.Acquire();
            _cachedMousePos = new Vector3();
        }

        public void CheckMouseInput()
        {
            var mouseState = _device.CurrentMouseState;
            if(mouseState.GetMouseButtons()[0] == 0)
            {
                return;
            }
            onMouseInputHandleCallback?.Invoke(mouseState.X, mouseState.Y, mouseState.Z);
            _cachedMousePos = new Vector3(mouseState.X, mouseState.Y, mouseState.Z);
        }

        public void CheckKeyBoardInput()
        {
            var keyState = _device.GetCurrentKeyboardState();
            DirectionX inputX = DirectionX.None;
            DirectionY inputY = DirectionY.None;

            if (keyState[Key.LeftArrow])
            {
                inputX = DirectionX.Left;
            }
            else if (keyState[Key.RightArrow])
            {
                inputX = DirectionX.Right;
            }

            if (keyState[Key.UpArrow])
            {
                inputY = DirectionY.Up;
            }
            else if(keyState[Key.DownArrow])
            {
                inputY = DirectionY.Down;
            }

            onKeyInputHandleCallback?.Invoke(inputX, inputY);
        }
    }

    public enum DirectionX
    {
        None,
        Left,
        Right,
    }

    public enum DirectionY
    {
        None,
        Up,
        Down,
    }
}
