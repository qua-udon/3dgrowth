using System;
using SlimDX;

namespace _3dgrowth
{
    public class BoxAABB : TwoObjectCollision
    {
        public override void SetObject(RendererBase baseObject, RendererBase moveObject)
        {
            base.SetObject(baseObject, moveObject);
            _baseObject.SetScale(2f);
            _moveObject.SetScale(1f);
            _moveObject.SetPosition(new Vector3(-3f, 0f, 0f));
        }

        protected override void CheckCollision()
        {
            bool isHit = false;
            float L, rA, rB;
            Vector3 interval = _baseObject.ModelPosition - _moveObject.ModelPosition;
            HitCube baseCube = _baseObject as HitCube;
            HitCube moveCube = _moveObject as HitCube;

            Vector3 NAe1 = baseCube.GetDirection(HitCube.BoxAxis.X);
            Vector3 Ae1 = NAe1 * baseCube.GetLength();
            Vector3 NAe2 = baseCube.GetDirection(HitCube.BoxAxis.Y);
            Vector3 Ae2 = NAe2 * baseCube.GetLength();
            Vector3 NAe3 = baseCube.GetDirection(HitCube.BoxAxis.Z);
            Vector3 Ae3 = NAe3 * baseCube.GetLength();
            Vector3 NBe1 = moveCube.GetDirection(HitCube.BoxAxis.X);
            Vector3 Be1 = NBe1 * moveCube.GetLength();
            Vector3 NBe2 = moveCube.GetDirection(HitCube.BoxAxis.Y);
            Vector3 Be2 = NBe2 * moveCube.GetLength();
            Vector3 NBe3 = moveCube.GetDirection(HitCube.BoxAxis.Z);
            Vector3 Be3 = NBe3 * moveCube.GetLength();

            //base Collision

            rA = Ae1.Length();
            rB = LenSegOnSeparateAxis(NAe1, Be1, Be2, Be3);
            L = Math.Abs(Vector3.Dot(interval, NAe1));
            if (L > rA + rB)
            {
                baseCube.SetHit(false);
                moveCube.SetHit(false);
                return;
            }

            rA = Ae2.Length();
            rB = LenSegOnSeparateAxis(NAe2, Be1, Be2, Be3);
            L = Math.Abs(Vector3.Dot(interval, NAe2));
            if (L > rA + rB)
            {
                baseCube.SetHit(false);
                moveCube.SetHit(false);
                return;
            }

            rA = Ae3.Length();
            rB = LenSegOnSeparateAxis(NAe3, Be1, Be2, Be3);
            L = Math.Abs(Vector3.Dot(interval, NAe3));
            if (L > rA + rB)
            {
                baseCube.SetHit(false);
                moveCube.SetHit(false);
                return;
            }

            //move Collision

            rA = Be1.Length();
            rB = LenSegOnSeparateAxis(NBe1, Ae1, Ae2, Ae3);
            L = Math.Abs(Vector3.Dot(interval, NBe1));
            if (L > rA + rB)
            {
                baseCube.SetHit(false);
                moveCube.SetHit(false);
                return;
            }

            rA = Be2.Length();
            rB = LenSegOnSeparateAxis(NBe2, Ae1, Ae2, Ae3);
            L = Math.Abs(Vector3.Dot(interval, NBe2));
            if (L > rA + rB)
            {
                baseCube.SetHit(false);
                moveCube.SetHit(false);
                return;
            }

            rA = Be3.Length();
            rB = LenSegOnSeparateAxis(NBe3, Ae1, Ae2, Ae3);
            L = Math.Abs(Vector3.Dot(interval, NBe3));
            if (L > rA + rB)
            {
                baseCube.SetHit(false);
                moveCube.SetHit(false);
                return;
            }

            // cross Collision

            // hit

            baseCube.SetHit(true);
            moveCube.SetHit(true);
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
