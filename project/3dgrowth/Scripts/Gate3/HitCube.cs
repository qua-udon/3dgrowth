using System;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D11;

namespace _3dgrowth
{
    public class HitCube : Gate3Object
    {
        public enum BoxAxis
        {
            X,
            Y,
            Z
        }

        public HitCube(Device device, Form form) : base(device, form)
        {

        }

        public Vector3 GetDirection(BoxAxis axis)
        {
            switch (axis)
            {
                case BoxAxis.X:
                    return Vector3.UnitX;
                case BoxAxis.Y:
                    return Vector3.UnitY;
                case BoxAxis.Z:
                    return Vector3.UnitZ;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }
        }

        public float GetLength()
        {
            return _scale / 2;
        }

        protected override System.Array IndexList => new uint[]
        {
            0, 1, 2, 0, 3, 1,
            4, 6, 5, 4, 5, 7,
            0, 2, 10, 0, 10, 8,
            3, 9, 1, 3, 11, 9,
            1, 6, 2, 1, 5, 6,
            3, 0, 4, 3, 4, 7,
        };

        protected override System.Array VerticeList => new VertexOutput[]
        {
            new VertexOutput
                    {
                        Position = new Vector3(-_scale / 2, _scale / 2, -_scale / 2),
                        TextureCoordinate = new Vector2(0, 0)
                    },

                    new VertexOutput
                    {
                        Position = new Vector3(_scale / 2, -_scale / 2, -_scale / 2),
                        TextureCoordinate = new Vector2(1, 1)
                    },

                    new VertexOutput
                    {
                        Position = new Vector3(-_scale / 2, -_scale / 2, -_scale / 2),
                        TextureCoordinate = new Vector2(0, 1)
                    },

                    new VertexOutput
                    {
                        Position = new Vector3(_scale / 2, _scale / 2, -_scale / 2),
                        TextureCoordinate = new Vector2(1, 0)
                    },

                    //

                    new VertexOutput
                    {
                        Position = new Vector3(-_scale / 2, _scale / 2, _scale / 2),
                        TextureCoordinate = new Vector2(0, 1)
                    },

                    new VertexOutput
                    {
                        Position = new Vector3(_scale / 2, -_scale / 2, _scale / 2),
                        TextureCoordinate = new Vector2(1, 0)
                    },

                    new VertexOutput
                    {
                        Position = new Vector3(-_scale / 2, -_scale / 2, _scale / 2),
                        TextureCoordinate = new Vector2(0, 0)
                    },

                    new VertexOutput
                    {
                        Position = new Vector3(_scale / 2, _scale / 2, _scale / 2),
                        TextureCoordinate = new Vector2(1, 1)
                    },

                    //

                    new VertexOutput
                    {
                        Position = new Vector3(-_scale / 2, _scale / 2, _scale / 2),
                        TextureCoordinate = new Vector2(1, 0)
                    },

                    new VertexOutput
                    {
                        Position = new Vector3(_scale / 2, -_scale / 2, _scale / 2),
                        TextureCoordinate = new Vector2(0, 1)
                    },

                    new VertexOutput
                    {
                        Position = new Vector3(-_scale / 2, -_scale / 2, _scale / 2),
                        TextureCoordinate = new Vector2(1, 1)
                    },

                    new VertexOutput
                    {
                        Position = new Vector3(_scale / 2, _scale / 2, _scale / 2),
                        TextureCoordinate = new Vector2(0, 0)
                    },
        };
    }
}
