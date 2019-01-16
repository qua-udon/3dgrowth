using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private RendererBase _baseObject;
        private RendererBase _moveObject;

        private KeyMover _objectMover;

        public TwoObjectCollision(Device device, System.Windows.Forms.Form form)
        {

        }

        public void SetObjectType()
        {

        }

        public void OnUpdate()
        {
            _objectMover.OnUpdate();
            CheckCollision();

            _baseObject.InitializeContent();
            _baseObject.SetView();
            _baseObject.Draw();
            _baseObject.Dispose();

            _baseObject.InitializeContent();
            _baseObject.SetView();
            _baseObject.Draw();
            _baseObject.Dispose();
        }

        protected abstract void CheckCollision();
    }
}
