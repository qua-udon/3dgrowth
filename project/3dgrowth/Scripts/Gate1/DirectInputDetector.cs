using System;
using Microsoft.DirectX.DirectInput;

namespace _3dgrowth
{
    public class DirectInputDetector
    {
        private Device _device;

        public Action<int, int, int> onMouseInputHandleCallback;
        public Action<DirectionX, DirectionY> onKeyInputHandleCallback;

        public DirectInputDetector()
        {
            _device = new Device(SystemGuid.Keyboard);
            _device.Acquire();
        }

        public void CheckMouseInput()
        {
            var mouseState = _device.CurrentMouseState;
            onMouseInputHandleCallback?.Invoke(mouseState.X, mouseState.Y, mouseState.Z);
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
