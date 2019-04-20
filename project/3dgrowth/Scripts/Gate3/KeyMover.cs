using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using Action = System.Action;
using Device = SlimDX.Direct3D11.Device;

namespace _3dgrowth
{
    public class KeyMover
    {
        private DirectInputDetector _detector;

        private Vector3 _model;

        public Action OnLeftArrowAction;
        public Action OnRightArrowAction;
        public Action OnDownArrowAction;
        public Action OnUpArrowAction;
        public Action OnEKeyAction;
        public Action OnQKeyAction;
        public Action OnWKeyAction;
        public Action OnSKeyAction;
        public Action OnAKeyAction;

        public KeyMover()
        {
            _detector = new DirectInputDetector(SystemGuid.Keyboard);
            _detector.SetDownKey(Key.LeftArrow);
            _detector.SetDownKey(Key.RightArrow);
            _detector.SetDownKey(Key.UpArrow);
            _detector.SetDownKey(Key.DownArrow);
            _detector.SetDownKey(Key.E);
            _detector.SetDownKey(Key.Q);
            _detector.SetDownKey(Key.W);
            _detector.SetDownKey(Key.S);
            _detector.SetDownKey(Key.A);
        }

        public void OnUpdate()
        {
            if (_detector.CheckKeyBoardDownInputRegister(Key.LeftArrow))
            {
                OnLeftArrowAction?.Invoke();
            }

            if (_detector.CheckKeyBoardDownInputRegister(Key.RightArrow))
            {
                OnRightArrowAction?.Invoke();
            }

            if (_detector.CheckKeyBoardDownInputRegister(Key.DownArrow))
            {
                OnDownArrowAction?.Invoke();
            }

            if (_detector.CheckKeyBoardDownInputRegister(Key.UpArrow))
            {
                OnUpArrowAction?.Invoke();
            }

            if (_detector.CheckKeyBoardDownInputRegister(Key.E))
            {
                OnEKeyAction?.Invoke();
            }

            if (_detector.CheckKeyBoardDownInputRegister(Key.Q))
            {
                OnQKeyAction?.Invoke();
            }

            if (_detector.CheckKeyBoardDownInputRegister(Key.W))
            {
                OnWKeyAction?.Invoke();
            }

            if (_detector.CheckKeyBoardDownInputRegister(Key.S))
            {
                OnSKeyAction?.Invoke();
            }

            if (_detector.CheckKeyBoardDownInputRegister(Key.A))
            {
                OnAKeyAction?.Invoke();
            }
        }
    }
}
