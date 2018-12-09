using System;
using Microsoft.DirectX.DirectInput;

namespace _3dgrowth
{
    public class DirectInputDetector
    {
        private Device _device;
        private bool _isDownLeft;
        private bool _isDownRight;

        public Action<int, int, int> onMouseInputHandleCallback;

        public Action<bool> onMousePointerDownCallback;
        public Action<DirectionX, DirectionY> onKeyInputHandleCallback;

        public DirectInputDetector()
        {
            _device = new Device(SystemGuid.Keyboard);
            _device.Acquire();
        }

        public DirectInputDetector(Guid guid)
        {
            _device = new Device(guid);
            _device.Acquire();
        }

        public void CheckMouseInput()
        {
            var mouseState = _device.CurrentMouseState;
            var buttonState = mouseState.GetMouseButtons();
            onMousePointerDownCallback?.Invoke(buttonState[0] != 0);
            onMouseInputHandleCallback?.Invoke(mouseState.X, mouseState.Y, mouseState.Z);
        }

        public bool CheckKeyBoardInput(Key key)
        {
            var keyState = _device.GetCurrentKeyboardState();
            return keyState[key];
        }

        public bool CheckKeyBoardDownInputLeft(Key key)
        {
            var keyState = _device.GetCurrentKeyboardState();
            var down = keyState[key];
            if (_isDownLeft != down)
            {
                _isDownLeft = down;
                if (down)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckKeyBoardDownInputRight(Key key)
        {
            var keyState = _device.GetCurrentKeyboardState();
            var down = keyState[key];
            if (_isDownRight != down)
            {
                _isDownRight = down;
                if (down)
                {
                    return true;
                }
            }
            return false;
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
