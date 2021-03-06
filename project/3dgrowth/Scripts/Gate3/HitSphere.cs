﻿using System;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D11;

namespace _3dgrowth
{
    public class HitSphere : Gate3Object
    {
        protected override int IndexSize => 6 * _separateX * _separateY;

        protected override System.Array IndexList => GetSphereIndexes();
        protected override System.Array VerticeList => GetSphereVertices();

        private int _separateX = 20;
        private int _separateY = 20;

        public HitSphere(Device device, Form form) : base(device, form)
        {

        }

        private System.Array GetSphereIndexes()
        {
            uint[] indexes = new uint[IndexSize];
            int indexCount = 0;

            for (int y = 0; y < _separateY; y++)
            {
                for (int x = 0; x < _separateX / 4; x++)
                {
                    if (x == _separateX - 1)
                    {

                    }

                    int next = x == _separateX - 1 ? 0 : x + 1;
                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);

                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);
                    indexes[indexCount++] = (uint)(y * _separateX + x + _separateX);
                }

                for (int x = _separateX - 1; x >= _separateX * 3 / 4; x--)
                {
                    if (x == _separateX - 1)
                    {

                    }

                    int next = x == _separateX - 1 ? 0 : x + 1;
                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);

                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);
                    indexes[indexCount++] = (uint)(y * _separateX + x + _separateX);
                }

                for (int x = _separateX / 2 + 1; x < _separateX * 3 / 4; x++)
                {
                    if (x == _separateX - 1)
                    {

                    }

                    int next = x == _separateX - 1 ? 0 : x + 1;
                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);

                    indexes[indexCount++] = (uint)(y * _separateX + x);
                    indexes[indexCount++] = (uint)(y * _separateX + next + _separateX);
                    indexes[indexCount++] = (uint)(y * _separateX + x + _separateX);
                }

                for (int x = _separateX / 2; x >= _separateX / 4; x--)
                {
                    if (x == _separateX - 1)
                    {

                    }

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

        private System.Array GetSphereVertices()
        {
            int vertexCount = _separateX * (_separateY + 1);
            VertexOutput[] vertices = new VertexOutput[vertexCount];

            for (int y = 0; y <= _separateY; y++)
            {
                for (int x = 0; x < _separateX; x++)
                {
                    double theta = Math.PI * (double)y / (double)_separateY;
                    double phi = Math.PI * 2d * (double)x / (double)_separateX;
                    double xPos = _scale * Math.Sin(theta) * Math.Cos(phi);
                    double yPos = _scale * Math.Cos(theta);
                    double zPos = _scale * Math.Sin(theta) * Math.Sin(phi);
                    double u = (x * 2) % _separateX == 0 ? 1d : (double)((x * 2) % _separateX) / _separateX;
                    double v = y == _separateY ? 1d : (double)(y % _separateY) / _separateY;
                    if (x > 10)
                    {
                        u = 1f - u;
                    }
                    if (x == 0 || x == 19)
                    {
                        u = 0f;
                    }
                    VertexOutput vert = new VertexOutput
                    {
                        Position = new SlimDX.Vector3((float)xPos, (float)yPos, (float)zPos),
                        Normal = new SlimDX.Vector3((float)xPos, (float)yPos, (float)zPos),
                        TextureCoordinate = new SlimDX.Vector2((float)u, (float)v)
                    };
                    vertices[_separateX * y + x] = vert;
                }
            }

            return vertices;
        }
    }
}
