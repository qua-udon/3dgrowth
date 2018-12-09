using SlimDX;
using SlimDX.Direct3D11;

namespace _3dgrowth
{
    public class DrawCube : RendererBase
    {
        private MouseRotator _rotator;

        protected override int IndexSize => 36;
        protected override Vector3 EyePosition => base.EyePosition.RotateByAxis(MathUtility.Axis.Y, -_rotator.AngleX).RotateByAxis(MathUtility.Axis.X, -_rotator.AngleY);

        public DrawCube(Device device, System.Windows.Forms.Form form) : base(device, form)
        {
            _rotator = new MouseRotator();
            _rotator.SetEvent();
        }

        public override void SetView()
        {
            _rotator.OnUpdate();
            base.SetView();
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

        protected override System.Array VerticeList => new VertexPositionTexture[]
        {
            new VertexPositionTexture
                    {
                        Position = new Vector3(-1, 1, 0),
                        TextureCoordinate = new Vector2(0, 0)
                    },

                    new VertexPositionTexture
                    {
                        Position = new Vector3(1, -1, 0),
                        TextureCoordinate = new Vector2(1, 1)
                    },

                    new VertexPositionTexture
                    {
                        Position = new Vector3(-1, -1, 0),
                        TextureCoordinate = new Vector2(0, 1)
                    },

                    new VertexPositionTexture
                    {
                        Position = new Vector3(1, 1, 0),
                        TextureCoordinate = new Vector2(1, 0)
                    },

                    //

                    new VertexPositionTexture
                    {
                        Position = new Vector3(-1, 1, 2),
                        TextureCoordinate = new Vector2(0, 1)
                    },

                    new VertexPositionTexture
                    {
                        Position = new Vector3(1, -1, 2),
                        TextureCoordinate = new Vector2(1, 0)
                    },

                    new VertexPositionTexture
                    {
                        Position = new Vector3(-1, -1, 2),
                        TextureCoordinate = new Vector2(0, 0)
                    },

                    new VertexPositionTexture
                    {
                        Position = new Vector3(1, 1, 2),
                        TextureCoordinate = new Vector2(1, 1)
                    },

                    //

                    new VertexPositionTexture
                    {
                        Position = new Vector3(-1, 1, 2),
                        TextureCoordinate = new Vector2(1, 0)
                    },

                    new VertexPositionTexture
                    {
                        Position = new Vector3(1, -1, 2),
                        TextureCoordinate = new Vector2(0, 1)
                    },

                    new VertexPositionTexture
                    {
                        Position = new Vector3(-1, -1, 2),
                        TextureCoordinate = new Vector2(1, 1)
                    },

                    new VertexPositionTexture
                    {
                        Position = new Vector3(1, 1, 2),
                        TextureCoordinate = new Vector2(0, 0)
                    },
        };
    }
}
