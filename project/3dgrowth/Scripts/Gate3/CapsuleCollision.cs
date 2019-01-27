using System;
using SlimDX;

namespace _3dgrowth
{
    public class CapsuleCollision : TwoObjectCollision
    {
        public override void SetObject(RendererBase baseObject, RendererBase moveObject)
        {
            base.SetObject(baseObject, moveObject);
            _baseObject.SetScale(0.5f);
            _baseObject.SetRotation(new Vector3(0, 0, (float)(Math.PI * (float)(30f / 180f))));
            _moveObject.SetScale(0.5f);
            _moveObject.SetPosition(new Vector3(-3f, 0f, 0f));
        }

        protected override void CheckCollision()
        {
            bool isHit = false;
            float L, rA, rB;
            Vector3 interval = _baseObject.ModelPosition - _moveObject.ModelPosition;
            HitCapsule baseCapsule = _baseObject as HitCapsule;
            HitCapsule moveCapsule = _moveObject as HitCapsule;

            if(Vector3.Distance(_baseObject.ModelPosition, _moveObject.ModelPosition) > 1f)
            {
                baseCapsule.SetHit(false);
                moveCapsule.SetHit(false);
                return;
            }

            // hit

            baseCapsule.SetHit(true);
            moveCapsule.SetHit(true);
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
