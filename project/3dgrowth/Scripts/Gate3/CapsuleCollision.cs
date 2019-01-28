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
            float axisLength;
            HitCapsule baseCapsule = _baseObject as HitCapsule;
            HitCapsule moveCapsule = _moveObject as HitCapsule;
            Vector3 point1, point2;
            float t1, t2;

            axisLength = LineLineDistance(baseCapsule.Bottom, baseCapsule.AxisVec, moveCapsule.Bottom,
                moveCapsule.AxisVec, out point1, out point2, out t1, out t2);

            if (Math.Abs(t1) <= 1.0f && Math.Abs(t2) <= 1.0f)
            {
                baseCapsule.SetHit(axisLength <= 1.0f);
                moveCapsule.SetHit(axisLength <= 1.0f);
                return;
            }

            t1 = t1 < 0f ? 0f : 1f;
            axisLength = PointSegmentDistance(baseCapsule.Bottom + baseCapsule.AxisVec * t1, moveCapsule.Bottom,
                moveCapsule.Top, out point2, out t2);
            if (Math.Abs(t2) <= 1f)
            {
                baseCapsule.SetHit(axisLength <= 1.0f);
                moveCapsule.SetHit(axisLength <= 1.0f);
                return;
            }

            t2 = t2 < 0f ? 0f : 1f;
            axisLength = PointSegmentDistance(moveCapsule.Bottom + moveCapsule.AxisVec * t2, baseCapsule.Bottom,
                baseCapsule.Top, out point1, out t1);
            if (Math.Abs(t1) <= 1f)
            {
                baseCapsule.SetHit(axisLength <= 1.0f);
                moveCapsule.SetHit(axisLength <= 1.0f);
                return;
            }

            t1 = t1 < 0f ? 0f : 1f;
            if((point2 - (baseCapsule.Bottom + baseCapsule.AxisVec * t1)).Length() > 1.0f)
            {
                baseCapsule.SetHit(false);
                moveCapsule.SetHit(false);
                return;
            }
            else
            {
                baseCapsule.SetHit(true);
                moveCapsule.SetHit(true);
                return;
            }
        }

        private float PointLineDistance(Vector3 point, Vector3 begin, Vector3 dir, out Vector3 h, out float t)
        {
            t = 0f;
            if (dir.LengthSquared() > 0f)
            {
                t = Vector3.Dot(point - begin, dir) / dir.LengthSquared();
            }

            h = begin + t * dir;
            return (h - point).Length();
        }

        private float PointSegmentDistance(Vector3 point, Vector3 begin, Vector3 end, out Vector3 h, out float t)
        {
            float len = PointLineDistance(point, begin, end - begin, out h, out t);
            if (!IsSharp(point, begin, end))
            {
                h = begin;
                return (begin - point).Length();
            }
            else if(!IsSharp(point, end, begin))
            {
                h = end;
                return (end - point).Length();
            }

            return len;
        }

        private float LineLineDistance(Vector3 begin1, Vector3 dir1, Vector3 begin2, Vector3 dir2, out Vector3 point1, out Vector3 point2, out float t1, out float t2)
        {
            float dot = Vector3.Dot(dir1, dir2);
            float distance1 = dir1.LengthSquared();
            float distance2 = dir2.LengthSquared();
            Vector3 vecBegin = begin1 - begin2;
            t1 = (dot * Vector3.Dot(dir2, vecBegin) - distance2 * Vector3.Dot(dir1, vecBegin)) /
                 (distance1 * distance2 - dot * dot);
            point1 = begin1 + t1 * dir1;
            t2 = Vector3.Dot(dir2, (point1 - begin2)) / distance2;
            point2 = begin2 + t2 * dir2;

            return (point2 - point1).Length();
        }

        private bool IsSharp(Vector3 point, Vector3 begin, Vector3 end)
        {
            return Vector3.Dot(point - begin, end - begin) >= 0;
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
