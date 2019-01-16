using System;
using SlimDX.Direct3D11;

namespace _3dgrowth
{
    public class DrawCapsule : MouseRotateMesh
    {
        protected override int IndexSize => 6 * _separateX * (_separateY + 1);

        protected override System.Array IndexList => GetCapsuleIndexes();
        protected override System.Array VerticeList => GetCapsuleVertices();

        private double _radius = 0.5d;
        private double _height = 1d;
        private int _separateX = 20;
        private int _separateY = 20;

        public DrawCapsule(Device device, System.Windows.Forms.Form form) : base(device, form)
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
                        double cylOffset = y > 15 ? - _height / 2d : _height / 2d;
                        double cylHeight = y > 15 ? -Math.Cos(theta) : Math.Cos(theta);
                        xPos = _radius * Math.Sin(theta) * Math.Cos(phi);
                        yPos = _radius * cylHeight + cylOffset;
                        zPos = _radius * Math.Sin(theta) * Math.Sin(phi);
                        u = (x * 2) % _separateX == 0 ? 1d : (double)((x * 2) % _separateX) / _separateX;
                        v = (double)(y % _separateY) / _separateY;

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
                        xPos = _radius * Math.Cos(phi);
                        yPos = _height / 2d - ((y - 4) * (_height * 2 / _separateY));
                        zPos = _radius * Math.Sin(phi);
                        u = (x * 2) % _separateX == 0 ? 1d : (double)((x * 2) % _separateX) / _separateX;
                        v = (double)(y % _separateY) / _separateY;

                        VertexOutput vertCylinder = new VertexOutput
                        {
                            Position = new SlimDX.Vector3((float)xPos, (float)yPos, (float)zPos),
                            TextureCoordinate = new SlimDX.Vector2((float)u, (float)v)
                        };

                        if (y == 14)
                        {
                            Console.WriteLine("14: " + xPos + "," + yPos + "," + zPos);
                        }

                        if (y == 15)
                        {
                            Console.WriteLine("15: " + xPos + "," + yPos + "," + zPos);
                        }
                        vertices[_separateX * y + x] = vertCylinder;
                    }
                }
            }

            return vertices;
        }
    }
}
