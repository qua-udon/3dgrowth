using System;
using SlimDX;

namespace _3dgrowth
{
    public class SphereCollision : TwoObjectCollision
    {
        public override void SetObject(RendererBase baseObject, RendererBase moveObject)
        {
            base.SetObject(baseObject, moveObject);
            _baseObject.SetScale(0.5f);
            _moveObject.SetScale(0.5f);
            _moveObject.SetPosition(new Vector3(-3f, 0f, 0f));
        }

        protected override void CheckCollision()
        {
            bool isHit = false;
            float L, rA, rB;
            Vector3 interval = _baseObject.ModelPosition - _moveObject.ModelPosition;
            HitSphere baseSphere = _baseObject as HitSphere;
            HitSphere moveSphere = _moveObject as HitSphere;

            if(Vector3.Distance(_baseObject.ModelPosition, _moveObject.ModelPosition) > 1f)
            {
                baseSphere.SetHit(false);
                moveSphere.SetHit(false);
                return;
            }

            // hit

            baseSphere.SetHit(true);
            moveSphere.SetHit(true);
        }

        private float LenSegOnSeparateAxis(Vector3 sep, Vector3 e1, Vector3 e2)
        {
            float r1 = Math.Abs(Vector3.Dot(sep, e1));
            float r2 = Math.Abs(Vector3.Dot(sep, e2));
            return r1 + r2;
        }

        private float LenSegOnSeparateAxis(Vector3 sep, Vector3 e1, Vector3 e2, Vector3 e3)
        {
            float r3 = Math.Abs(Vector3.Dot(sep, e3));
            return LenSegOnSeparateAxis(sep, e1, e2) + r3;
        }
    }
}
