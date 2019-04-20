using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using SlimDX;
using SlimDX.Direct3D11;
using Buffer = SlimDX.Direct3D11.Buffer;
using Device = SlimDX.Direct3D11.Device;

namespace _3dgrowth
{
    public class FBXRenderer : RendererBase
    {
        protected override string ShaderSource => Properties.Resource1.Lambert;
        protected FBXManager.Mesh[] _meshList;

        protected override bool UseModel => true;
        public override Vector3 ModelPosition => new Vector3(0f, -0.85f, 0f);

        public FBXManager.Skeleton[][] _poses;
        private bool _isPlay;
        private int _playFrame;

        public FBXRenderer(Device device, Form form, string fbxPath) : base(device, form)
        {
            SetPath(fbxPath);
            SetRotation(new Vector3(0f, (float)Math.PI, 0f));
        }

        public void SetPath(string path)
        {
            _meshList = FBXManager.Instance.CreateMeshList(path);
            _poses = FBXManager.Instance.GetPoseMatrixList(path);

            var pose = _poses[0];

            for (int i = 0; i < _poses[0].Length; i++)
            {
                var p = pose[i];
                Console.WriteLine("poseBone::" + p.Name);
            }
        }

        public void SetFrame(int frame)
        {
            _playFrame = frame;
            _isPlay = true;
        }

        private void InitializeMeshInputAssembler(FBXManager.Mesh mesh)
        {
            SetMeshVertex(mesh);
            SetMeshIndex(mesh);
            base.InitializeTriangleInputAssembler();
        }

        private void SetMeshVertex(FBXManager.Mesh mesh)
        {
            int length = mesh.Indicies.Length;
            uint[] indicies = new uint[length];
            for (int i = 0; i < length; ++i)
            {
                indicies[i] = (uint) mesh.Indicies[i];
            }
            _indexBuffer = CreateIndexBuffer(indicies);
        }

        protected override Buffer CreateIndexBuffer(System.Array indexes)
        {
            using (DataStream stream = new DataStream(indexes, true, true))
            {
                return new Buffer(
                    _device,
                    stream,
                    sizeof(uint) * indexes.Length,
                    ResourceUsage.Default,
                    BindFlags.IndexBuffer,
                    0,
                    0,
                    0
                );
            }
        }

        private void SetMeshIndex(FBXManager.Mesh mesh)
        {
            var outputs = new List<VertexOutput>();
            Matrix[] blendMats = new Matrix[mesh.VertexCount];
            Matrix[] sumMats = new Matrix[mesh.VertexCount];
            bool[] blendFlags = new bool[mesh.VertexCount];

            for (int i = 0; i < blendMats.Length; ++i)
            {
                blendMats[i] = Matrix.Identity;
                sumMats[i] = Matrix.Identity;
            }

            for (int i = 0; i < mesh.SkinListCount; ++i)
            {
                var skinList = mesh.SkinLists[i];

                for (int j = 0; j < skinList.BoneInfos.Length; ++j)
                {
                    var weightInfos = skinList.SkinWeights;
                    var bone = skinList.BoneInfos[j];
                    Matrix boneMatrix = Matrix.Identity;
                    if (!_isPlay)
                    {
                        continue;
                    }

                    var pose = _poses[_playFrame].FirstOrDefault(x => x.Name == bone.Name);
                    if (pose != null)
                    {
                        boneMatrix = bone.InvInitPoseMatrix;
                    }

                    List<int> vertList = new List<int>();
                    List<float> weightList = new List<float>();
                    for (int k = 0; k < weightInfos.Length; ++k)
                    {
                        var weightInfo = weightInfos[k];
                        for (int l = 0; l < weightInfo.WeightInfo.Count; ++l)
                        {
                            if (weightInfo.WeightInfo[l].BoneIndex == bone.BoneIndex && weightInfo.WeightInfo[l].Weight > 0f)
                            {
                                vertList.Add(weightInfo.VertexIndex);
                                weightList.Add(weightInfo.WeightInfo[l].Weight);
                            }
                        }
                    }
                    if (vertList.Count == 0)
                    {
                        continue;
                    }

                    for (int k = 0; k < vertList.Count; ++k)
                    {
                        int vertexIndex = vertList[k];
                        blendFlags[vertexIndex] = true;
                        blendMats[vertexIndex] = blendMats[vertexIndex] * pose.Matrix;
                        if (sumMats[vertexIndex].IsIdentity)
                        {
                            sumMats[vertexIndex] = boneMatrix * pose.Matrix * weightList[k];
                        }
                        else
                        {
                            sumMats[vertexIndex] = sumMats[vertexIndex] + boneMatrix * pose.Matrix * weightList[k];
                        }
                    }
                }
            }

            for (int i = 0; i < mesh.VertexCount; ++i)
            {
                var pos = mesh.Positions[i];

                var farMat = new Matrix();
                farMat.M11 = farMat.M21 = farMat.M31 = farMat.M41 = pos.X;
                farMat.M12 = farMat.M22 = farMat.M32 = farMat.M42 = pos.Y;
                farMat.M13 = farMat.M23 = farMat.M33 = farMat.M43 = pos.Z;
                farMat.M14 = farMat.M24 = farMat.M34 = farMat.M44 = 1;

                var mat = farMat * sumMats[i];
                pos.X = pos.X * mat.M11 + pos.Y * mat.M21 + pos.Z * mat.M31 + mat.M41;
                pos.Y = pos.X * mat.M12 + pos.Y * mat.M22 + pos.Z * mat.M32 + mat.M42;
                pos.Z = pos.X * mat.M13 + pos.Y * mat.M23 + pos.Z * mat.M33 + mat.M43;

                pos = new Vector3(mat.M11 / mat.M14, mat.M12 / mat.M14, mat.M13 / mat.M14);

                var vert = new VertexOutput();
                vert.Position = pos * 0.01f;
                vert.Normal = pos;
                vert.TextureCoordinate = mesh.uv0[i];
                outputs.Add(vert);
            }

            _vertexBuffer = CreateVertexBuffer(outputs.ToArray());
        }

        protected override InputLayout CreateInputLayout()
        {
            return new InputLayout(_device, _effect.GetTechniqueByIndex(0).GetPassByIndex(0).Description.Signature,
                new[]
                {
                    new InputElement
                    {
                        SemanticName = "SV_Position",
                        Format = SlimDX.DXGI.Format.R32G32B32_Float
                    },
                    new InputElement
                    {
                        SemanticName = "NORMAL",
                        Format = SlimDX.DXGI.Format.R32G32B32_Float,
                        AlignedByteOffset = InputElement.AppendAligned//自動的にオフセット決定
                    },
                    new InputElement
                    {
                        SemanticName = "TEXCOORD",
                        Format = SlimDX.DXGI.Format.R32G32_Float,
                        AlignedByteOffset = InputElement.AppendAligned//自動的にオフセット決定
                    },
                });
        }

        public override void InitializeContent()
        {
            _effect = CreateEffect();
            _inputLayout = CreateInputLayout();
        }

        public override void Draw()
        {
            InitializeMeshInputAssembler(_meshList[0]);
            SetTexture();
            _effect.GetTechniqueByIndex(0).GetPassByIndex(0).Apply(_device.ImmediateContext);
            _device.ImmediateContext.DrawIndexed(_meshList[0].Indicies.Length, 0, 0);
        }
    }
}
