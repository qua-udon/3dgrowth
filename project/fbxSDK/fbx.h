#pragma once

#include "stdafx.h"
#include <fbxsdk.h>
#include <vector>

// Skin情報
typedef struct {
	int     Count;
	char*   pName;
	int*    pIndicies;
	float*  pWeights;
	float*  pInitMatrix; // 4*4の行列
} Skin;

// Skinリスト
typedef struct
{
	int Count;
	Skin* pSkins;
} SkinList;

// Mesh情報
typedef struct
{
	char*   pName;
	int     VertexCount;
	int     TriangleCount;
	int     SkinListCount;
	float*  pPositions;
	float*  pNormals;
	float*  pColors;
	float*  pUv0;
	float*  pUv1;
	int*    pIndicies;
	SkinList* pSkinLists;
} Mesh;

// Meshリスト
typedef struct {
	int Count;
	Mesh* pMeshes;
} MeshList;

// Skelton情報
typedef struct
{
	int     Count;
	char*   pName;
	float*  pMatrix;
} Skeleton;

// Skelton情報
typedef struct
{
	int     Count;
	Skeleton* pSkeletons;
} Pose;

// Skeltonリスト
typedef struct {
	int Count;
	Pose* pPoses;
} SkeletonList;

// MeshListの作成
DllExport bool CreateMeshList(MeshList* pMeshList, const char* pFileName);
// MeshListの作成
DllExport bool CreateSkeletonList(SkeletonList* poseList, const char* pFileName);
// MeshListの削除
DllExport bool DestroyMeshList(MeshList* pMeshList);
// MeshListの削除
DllExport bool DestroySkeletonList(SkeletonList* pSkeletonList);