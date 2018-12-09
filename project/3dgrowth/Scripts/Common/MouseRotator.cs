using System;
using Microsoft.DirectX.DirectInput;

namespace _3dgrowth
{
    public class MouseRotator : MouseDetector
    {
        private const double DELTA_ANGLE = System.Math.PI / 360d;

        private double _angleX;
        public double AngleX => _angleX;

        private double _angleY;
        public double AngleY => _angleY;

        public override void OnMousePositionChanged(int x, int y, int z)
        {
            if(!IsPointerDown)
            {
                return;
            }

            _angleX += x * DELTA_ANGLE;
            _angleY += y * DELTA_ANGLE;
        }
    }
}
