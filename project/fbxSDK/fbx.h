#pragma once

#include "stdafx.h"
#include <fbxsdk.h>
#include <vector>

// Skin���
typedef struct {
	int     Count;
	char*   pName;
	int*    pIndicies;
	float*  pWeights;
	float*  pInitMatrix; // 4*4�̍s��
} Skin;

// Skin���X�g
typedef struct
{
	int Count;
	Skin* pSkins;
} SkinList;

// Mesh���
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

// Mesh���X�g
typedef struct {
	int Count;
	Mesh* pMeshes;
} MeshList;

// Skelton���
typedef struct
{
	int     Count;
	char*   pName;
	float*  pMatrix;
} Skeleton;

// Skelton���
typedef struct
{
	int     Count;
	Skeleton* pSkeletons;
} Pose;

// Skelton���X�g
typedef struct {
	int Count;
	Pose* pPoses;
} SkeletonList;

// MeshList�̍쐬
DllExport bool CreateMeshList(MeshList* pMeshList, const char* pFileName);
// MeshList�̍쐬
DllExport bool CreateSkeletonList(SkeletonList* poseList, const char* pFileName);
// MeshList�̍폜
DllExport bool DestroyMeshList(MeshList* pMeshList);
// MeshList�̍폜
DllExport bool DestroySkeletonList(SkeletonList* pSkeletonList);