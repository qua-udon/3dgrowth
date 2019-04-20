using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using SlimDX;

/// <summary>
/// FBXManager
/// </summary>
public class FBXManager
{
    #region define

    /// C++のメッシュ構造体
    [StructLayout(LayoutKind.Sequential)]
    public struct CPP_Mesh
    {
        public IntPtr pName;
        public int VertexCount;
        public int TriangleCount;
        public int SkinListCount;
        public IntPtr pPositions;
        public IntPtr pNormals;
        public IntPtr pColors;
        public IntPtr pUv0;
        public IntPtr pUv1;
        public IntPtr pIndicies;
        public IntPtr pSkinLists;
    }

    /// メッシュリストの構造体
    [StructLayout(LayoutKind.Sequential)]
    public struct CPP_MeshList
    {
        public int Count;
        public IntPtr pMeshes;
    }

    /// C++のスキン構造体
    [StructLayout(LayoutKind.Sequential)]
    public struct CPP_Skin
    {
        public int Count;
        public IntPtr pName;
        public IntPtr pIndicies;
        public IntPtr pWeights;
        public IntPtr pInitMatrix;
    }

    /// C++のスキンリスト
    [StructLayout(LayoutKind.Sequential)]
    public struct CPP_SkinList
    {
        public int Count;
        public IntPtr pSkins;
    }

    /// C++のスキン構造体
    [StructLayout(LayoutKind.Sequential)]
    public struct CPP_Skeleton
    {
        public int Count;
        public IntPtr pName;
        public IntPtr pMatrix;
    }

    /// C++のスキン構造体
    [StructLayout(LayoutKind.Sequential)]
    public struct CPP_Pose
    {
        public int Count;
        public IntPtr pSkeletons;
    }

    /// C++のスキンリスト
    [StructLayout(LayoutKind.Sequential)]
    public struct CPP_SkeletonList
    {
        public int Count;
        public IntPtr pPoses;
    }

    public class Mesh
    {
        public string Name;
        public int VertexCount;
        public int TriangleCount;
        public int SkinListCount;
        public Vector3[] Positions;
        public Vector3[] Normals;
        public Vector4[] Colors;
        public Vector2[] uv0;
        public Vector2[] uv1;
        public int[] Indicies;
        public SkinList[] SkinLists;
    }

    public class Skeleton
    {
        public string Name;
        public Matrix Matrix;
    }

    public class SkinList
    {
        public BoneInfo[] BoneInfos;
        public VertexWeight[] SkinWeights;
    }

    public class BoneInfo
    {
        public int BoneIndex;
        public string Name;
        public Matrix InitPoseMatrix;
        public Matrix InvInitPoseMatrix;
    }

    public class VertexWeight
    {
        public int VertexIndex;
        public List<WeightInfo> WeightInfo;

        public VertexWeight(int index, List<WeightInfo> info)
        {
            VertexIndex = index;
            WeightInfo = info;
        }
    }

    public class WeightInfo
    {
        public int BoneIndex;
        public float Weight;
    }

    #endregion

    #region variable

    /// シングルトンインスタンス
    private static FBXManager _instance;
    public static FBXManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new FBXManager();
            }
            return _instance;
        }
    }

    #endregion

    #region method

    /// 静的コンストラクタ
    static FBXManager()
    {
        _instance = new FBXManager();
    }

    [DllImport("fbxSDK.dll", EntryPoint = "CreateMeshList", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool DllCreateMeshList(ref CPP_MeshList meshList, string pFileName);

    [DllImport("fbxSDK.dll", EntryPoint = "CreateSkeletonList", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool DllCreatePoseList(ref CPP_SkeletonList poseList, string pFileName);

    [DllImport("fbxSDK.dll", EntryPoint = "DestroyMeshList", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool DllDestroyMeshList(ref CPP_MeshList meshList);

    [DllImport("fbxSDK.dll", EntryPoint = "DestroySkeletonList", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool DllDestroySkeletonList(ref CPP_SkeletonList skeletonList);

    /// <summary>
    /// メッシュを生成する
    /// </summary>
    public Mesh[] CreateMeshList(string filePath)
    {
        Console.WriteLine("Create Mesh");
        // CppMeshリスト情報の生成
        var cppMeshList = new CPP_MeshList();
        if (!DllCreateMeshList(ref cppMeshList, filePath))
        {
            Console.WriteLine("Create Mesh Is Failed");
            return null;
        }
        Console.WriteLine("Create Mesh Is Success");

        // Mesh配列の生成
        var meshes = new Mesh[cppMeshList.Count];
        int size = Marshal.SizeOf(typeof(CPP_Mesh));
        for (int i = 0; i < meshes.Length; i++)
        {
            IntPtr adderss = cppMeshList.pMeshes + size * i;
            var cppMesh = Marshal.PtrToStructure<CPP_Mesh>(adderss);

            var mesh = new Mesh();
            mesh.VertexCount = cppMesh.VertexCount;
            mesh.TriangleCount = cppMesh.TriangleCount;
            mesh.SkinListCount = cppMesh.SkinListCount;
            mesh.Name = Marshal.PtrToStringAnsi(cppMesh.pName);
            mesh.Indicies = new int[cppMesh.VertexCount];
            Marshal.Copy(cppMesh.pIndicies, mesh.Indicies, 0, cppMesh.VertexCount);

            var positions = new float[cppMesh.VertexCount * 3];
            mesh.Positions = new Vector3[cppMesh.VertexCount];
            Marshal.Copy(cppMesh.pPositions, positions, 0, cppMesh.VertexCount * 3);
            for (int j = 0; j < cppMesh.VertexCount; j++)
            {
                var index = j * 3;
                mesh.Positions[j] = new Vector3(positions[index], positions[index + 1], positions[index + 2]);
            }

            if (cppMesh.pColors != IntPtr.Zero)
            {
                var colors = new float[cppMesh.VertexCount * 4];
                mesh.Colors = new Vector4[cppMesh.VertexCount];
                Marshal.Copy(cppMesh.pColors, colors, 0, cppMesh.VertexCount * 4);
                for (int j = 0; j < cppMesh.VertexCount; j++)
                {
                    var index = j * 4;
                    mesh.Colors[j] = new Vector4(colors[index], colors[index + 1], colors[index + 2], colors[index + 3]);
                }
            }

            if (cppMesh.pUv0 != IntPtr.Zero)
            {
                var uv0 = new float[cppMesh.VertexCount * 2];
                mesh.uv0 = new Vector2[cppMesh.VertexCount];
                Marshal.Copy(cppMesh.pUv0, uv0, 0, cppMesh.VertexCount * 2);
                for (int j = 0; j < cppMesh.VertexCount; j++)
                {
                    var index = j * 2;
                    mesh.uv0[j] = new Vector2(uv0[index], uv0[index + 1]);
                }
            }

            if (cppMesh.pUv1 != IntPtr.Zero)
            {
                var uv1 = new float[cppMesh.VertexCount * 2];
                mesh.uv1 = new Vector2[cppMesh.VertexCount];

                Marshal.Copy(cppMesh.pUv1, uv1, 0, cppMesh.VertexCount * 2);
                for (int j = 0; j < cppMesh.VertexCount; j++)
                {
                    var index = j * 2;
                    mesh.uv1[j] = new Vector2(uv1[index], uv1[index + 1]);
                }
            }

            if (cppMesh.pUv1 != IntPtr.Zero)
            {
                var uv1 = new float[cppMesh.VertexCount * 2];
                mesh.uv1 = new Vector2[cppMesh.VertexCount];

                Marshal.Copy(cppMesh.pUv1, uv1, 0, cppMesh.VertexCount * 2);
                for (int j = 0; j < cppMesh.VertexCount; j++)
                {
                    var index = j * 2;
                    mesh.uv1[j] = new Vector2(uv1[index], uv1[index + 1]);
                }
            }

            if (cppMesh.pSkinLists != IntPtr.Zero)
            {
                var skinList = new SkinList[cppMesh.SkinListCount];
                int skinListSize = Marshal.SizeOf(typeof(CPP_SkinList));
                for (int j = 0; j < cppMesh.SkinListCount; j++)
                {
                    skinList[j] = new SkinList();
                    IntPtr skinListAddress = cppMesh.pSkinLists + skinListSize * j;
                    var cppSkinList = Marshal.PtrToStructure<CPP_SkinList>(skinListAddress);

                    skinList[j].BoneInfos = new BoneInfo[cppSkinList.Count];
                    int skinSize = Marshal.SizeOf(typeof(CPP_Skin));
                    var skinMeshDict = new Dictionary<int, List<WeightInfo>>();
                    for (int k = 0; k < cppSkinList.Count; k++)
                    {
                        // スキン情報
                        IntPtr skinAddress = cppSkinList.pSkins + skinSize * k;
                        var cppSkin = Marshal.PtrToStructure<CPP_Skin>(skinAddress);
                        int[] indicies = new int[cppSkin.Count];
                        Marshal.Copy(cppSkin.pIndicies, indicies, 0, cppSkin.Count);
                        float[] weights = new float[cppSkin.Count];
                        Marshal.Copy(cppSkin.pWeights, weights, 0, cppSkin.Count);
                        for (int l = 0; l < cppSkin.Count; l++)
                        {
                            var weightInfo = new WeightInfo();
                            weightInfo.BoneIndex = k;
                            weightInfo.Weight = weights[l];

                            var vertexIndex = indicies[l];
                            if (!skinMeshDict.ContainsKey(vertexIndex))
                            {
                                skinMeshDict[vertexIndex] = new List<WeightInfo>();
                            }
                            skinMeshDict[vertexIndex].Add(weightInfo);
                        }

                        // 骨の姿勢情報
                        var boneInfo = new BoneInfo();
                        var initMatrix = new float[16];
                        Marshal.Copy(cppSkin.pInitMatrix, initMatrix, 0, initMatrix.Length);
                        var matrix = new Matrix();
                        for (int row = 0; row < 4; row++)
                        {
                            var offset = 4 * row;
                            matrix.set_Rows(row, new Vector4(initMatrix[offset], initMatrix[offset + 1], initMatrix[offset + 2], initMatrix[offset + 3]));
                        }
                        boneInfo.BoneIndex = k;
                        boneInfo.Name = Marshal.PtrToStringAnsi(cppSkin.pName);

                        boneInfo.InitPoseMatrix = matrix;
                        boneInfo.InvInitPoseMatrix = matrix;
                        boneInfo.InvInitPoseMatrix.Invert();
                        skinList[j].BoneInfos[k] = boneInfo;
                    }
                    var temp = skinMeshDict.Select(x => new VertexWeight(x.Key, x.Value)).ToArray();
                    skinList[j].SkinWeights = temp;
                }
                mesh.SkinLists = skinList;
            }

            meshes[i] = mesh;
        }

        DllDestroyMeshList(ref cppMeshList);

        return meshes;
    }

    /// <summary>
    /// メッシュを生成する
    /// </summary>
    public Skeleton[][] GetPoseMatrixList(string filePath)
    {
        Console.WriteLine("Get Pose");
        // CppMeshリスト情報の生成
        var cppSkeletonList = new CPP_SkeletonList();
        if (!DllCreatePoseList(ref cppSkeletonList, filePath))
        {
            Console.WriteLine("Create Pose Is Failed");
            return null;
        }
        Console.WriteLine("Get Pose Is Succes");

        // Mesh配列の生成
        var skeletons = new Skeleton[120][];
        int poseSize = Marshal.SizeOf(typeof(CPP_Pose));
        int skeletonSize = Marshal.SizeOf(typeof(CPP_Skeleton));
        for (int i = 0; i < 120; i++)
        {
            IntPtr poseAdderss = cppSkeletonList.pPoses + poseSize * i;
            var cppPose = Marshal.PtrToStructure<CPP_Pose>(poseAdderss);
            skeletons[i] = new Skeleton[cppPose.Count];
            for (int j = 0; j < cppPose.Count; j++)
            {
                IntPtr skeletonAdderss = cppPose.pSkeletons + skeletonSize * j;
                var cppSkeleton = Marshal.PtrToStructure<CPP_Skeleton>(skeletonAdderss);

                var skeleton = new Skeleton();
                skeleton.Name = Marshal.PtrToStringAnsi(cppSkeleton.pName);
                var mat = new float[16];
                Marshal.Copy(cppSkeleton.pMatrix, mat, 0, mat.Length);
                var matrix = new Matrix();
                for (int row = 0; row < 4; row++)
                {
                    var offset = 4 * row;
                    matrix.set_Rows(row, new Vector4(mat[offset], mat[offset + 1], mat[offset + 2], mat[offset + 3]));
                }

                skeleton.Matrix = matrix;
                skeletons[i][j] = skeleton;
            }
        }

        DllDestroySkeletonList(ref cppSkeletonList);

        return skeletons;
    }

    #endregion
}