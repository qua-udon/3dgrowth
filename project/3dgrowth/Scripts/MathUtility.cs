using SlimDX;
using System;

namespace _3dgrowth
{
    public static class MathUtility
    {
        public enum Axis
        {
            X,
            Y,
            Z,
        }

        public static Vector3 RotateByAxis(this Vector3 vector, Axis axis, double theta)
        {
            float x = vector.X;
            float y = vector.Y;
            float z = vector.Z;
            float cos = (float) Math.Cos(theta);
            float sin = (float)Math.Sin(theta);

            switch (axis)
            {
                case Axis.X:
                    return new Vector3(x, y * cos + z * sin, -y * sin + z * cos);
                case Axis.Y:
                    return new Vector3(x * cos - z * sin, y, x * sin + z * cos);
                case Axis.Z:
                    return new Vector3(x * cos + y * sin, -x * sin + y * cos, z);
                default:
                    return vector;
            }
        }
    }
}
