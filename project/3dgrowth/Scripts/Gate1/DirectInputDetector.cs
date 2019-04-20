using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.DirectX.DirectInput;

namespace _3dgrowth
{
    public class DirectInputDetector
    {
        private Device _device;

        private Dictionary<Key, bool> _registerKeyMap;
        private bool _isDown;
        private bool _isDownLeft;
        private bool _isDownRight;

        public Action<int, int, int> onMouseInputHandleCallback;

        public Action<bool> onMousePointerDownCallback;
        public Action<DirectionX, DirectionY> onKeyInputHandleCallback;

        public DirectInputDetector()
        {
            _device = new Device(SystemGuid.Keyboard);
            _device.Acquire();
            _registerKeyMap = new Dictionary<Key, bool>();
        }

        public DirectInputDetector(Guid guid)
        {
            _device = new Device(guid);
            _device.Acquire();
            _registerKeyMap = new Dictionary<Key, bool>();
        }

        public void SetDownKey(Key key)
        {
            _registerKeyMap.Add(key, false);
        }

        public void CheckMouseInput()
        {
            var mouseState = _device.CurrentMouseState;
            var buttonState = mouseState.GetMouseButtons();
            onMousePointerDownCallback?.Invoke(buttonState[0] != 0);
            onMouseInputHandleCallback?.Invoke(mouseState.X, mouseState.Y, 0);
        }

        public bool CheckKeyBoardInput(Key key)
        {
            var keyState = _device.GetCurrentKeyboardState();
            return keyState[key];
        }

        public bool CheckKeyBoardDownInputRegister(Key key)
        {
            bool isDown;
            if (_registerKeyMap.TryGetValue(key, out isDown))
            {
                var keyState = _device.GetCurrentKeyboardState();
                var down = keyState[key];
                if (down)
                {
                    return true;
                }
            }
            return false;
        }

        /*
        public bool CheckKeyBoardDownInputRegister(Key key)
        {
            bool isDown;
            if(_registerKeyMap.TryGetValue(key, out isDown))
            {
                var keyState = _device.GetCurrentKeyboardState();
                var down = keyState[key];
                if (isDown != down)
                {
                    _registerKeyMap[key] = down;
                    if (down)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        */

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
