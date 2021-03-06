﻿using System;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D11;

namespace _3dgrowth
{
    public class HitCapsule : Gate3Object
    {
        protected override int IndexSize => 6 * _separateX * (_separateY + 1);

        protected override System.Array IndexList => GetCapsuleIndexes();
        protected override System.Array VerticeList => GetCapsuleVertices();

        private double _height = 1d;
        private int _separateX = 20;
        private int _separateY = 20;

        public Vector3 Bottom => ModelPosition - Vector3.UnitY.RotateByAxis(MathUtility.Axis.Z, - ModelEulerAngle.Z) * ((float) _height * 0.5f);
        public Vector3 Top => ModelPosition + Vector3.UnitY.RotateByAxis(MathUtility.Axis.Z, - ModelEulerAngle.Z) * ((float)_height * 0.5f);
        public Vector3 AxisVec => Top - Bottom;

        public HitCapsule(Device device, Form form) : base(device, form)
        {

        }

        private System.Array GetCapsuleIndexes()
        {
            uint[] indexes = new uint[IndexSize];
            int indexCount = 0;

            for (int y = 5; y < 16; y++)
            {
                for (int x = 0; x < _separateX; x++)
                {
                    int next = x == _separateX - 1 ? 0 : x + 1;
                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);

                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);
                    indexes[indexCount++] = (uint)(y * _separateX + x + _separateX);
                }
            }

            for (int y = 16; y <= _separateY; y++)
            {
                for (int x = 0; x < _separateX; x++)
                {
                    int next = x == _separateX - 1 ? 0 : x + 1;
                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);

                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);
                    indexes[indexCount++] = (uint)(y * _separateX + x + _separateX);
                }
            }

            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < _separateX; x++)
                {
                    int next = x == _separateX - 1 ? 0 : x + 1;
                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);

                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);
                    indexes[indexCount++] = (uint)(y * _separateX + x + _separateX);
                }
            }

            return indexes;
        }

        private System.Array GetCapsuleVertices()
        {
            int vertexCount = _separateX * (_separateY + 1);
            VertexOutput[] vertices = new VertexOutput[vertexCount];

            for (int y = 0; y <= _separateY; y++)
            {
                for (int x = 0; x < _separateX; x++)
                {
                    double theta, phi, xPos, yPos, zPos, u, v;

                    if (y < 5 || y > 15)
                    {
                        double sphereY = y > 15 ? (double)y - 10d : (double)y;
                        theta = Math.PI * y / (double)(_separateY / 2);
                        phi = Math.PI * 2d * (double)x / (double)_separateX;
                        double cylOffset = y > 15 ? -_height / 2d : _height / 2d;
                        double cylHeight = y > 15 ? -Math.Cos(theta) : Math.Cos(theta);
                        xPos = _scale * Math.Sin(theta) * Math.Cos(phi);
                        yPos = _scale * cylHeight + cylOffset;
                        zPos = _scale * Math.Sin(theta) * Math.Sin(phi);
                        u = (x * 2) % _separateX == 0 ? 1d : (double)((x * 2) % _separateX) / _separateX;
                        v = (double)(y % _separateY) / _separateY;

                        if (x > 10)
                        {
                            u = 1f - u;
                        }
                        if (x == 0 || x == 19)
                        {
                            u = 0f;
                        }

                        if (y > 15)
                        {
                            xPos *= -1d;
                            zPos *= -1d;
                        }
                        VertexOutput vertSphere = new VertexOutput
                        {
                            Position = new SlimDX.Vector3((float)xPos, (float)yPos, (float)zPos),
                            TextureCoordinate = new SlimDX.Vector2((float)u, (float)v)
                        };

                        vertices[_separateX * y + x] = vertSphere;
                    }
                    else
                    {
                        phi = Math.PI * 2d * (double)x / (double)_separateX;
                        xPos = _scale * Math.Cos(phi);
                        yPos = _height / 2d - ((y - 4) * (_height * 2 / _separateY));
                        zPos = _scale * Math.Sin(phi);
                        u = (x * 2) % _separateX == 0 ? 1d : (double)((x * 2) % _separateX) / _separateX;
                        v = (double)(y % _separateY) / _separateY;

                        if (x > 10)
                        {
                            u = 1f - u;
                        }
                        if (x == 0 || x == 19)
                        {
                            u = 0f;
                        }

                        VertexOutput vertCylinder = new VertexOutput
                        {
                            Position = new SlimDX.Vector3((float)xPos, (float)yPos, (float)zPos),
                            TextureCoordinate = new SlimDX.Vector2((float)u, (float)v)
                        };

                        vertices[_separateX * y + x] = vertCylinder;
                    }
                }
            }

            return vertices;
        }
    }
}
