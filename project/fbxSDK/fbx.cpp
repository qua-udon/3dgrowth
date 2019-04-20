#include "stdafx.h"
#include "fbx.h"
#include "SDK/FBX/include/fbxsdk.h"
#include <iostream>
#include <assert.h>

// MeshList�̍\�z
void BuildMeshList(MeshList* pMeshList, const std::vector<FbxMesh*>& meshAttributes);
// MeshList�̍\�z
void BuildSkeletonList(SkeletonList* pSkeletonList, const std::vector<FbxSkeleton*>& skeletonAttributes);
// ���W�z��̐���
float* CreatePositionList(const FbxMesh* pMesh, const std::vector<int>& indicies);
/// �@���z��̐���
float* CreateNormalList(const FbxMesh* pMesh, const std::vector<int>& indicies, int normalIndex);
// �J���[�z��̐���
float* CreateColorList(const FbxMesh* pMesh, const std::vector<int>& indicies, int colorIndex);
// UV�z��̐���
float* CreateUvList(const FbxMesh* pMesh, const std::vector<int>& indicies, int uvIndex);
// �X�L�����X�g�̐���
SkinList* CreateSkinList(const FbxMesh* pMesh, int skinListCount, std::vector<int>**& ppIndexTable);
// �X�L�����X�g�̒���
int GetSkinListCount(const FbxMesh* pMesh);

/// MeshList�̐��� (�������m��)
bool CreateMeshList(MeshList* pMeshList, const char* pFileName)
{
	if (pFileName == NULL) {
		std::cout << "FileName is Null." << std::endl;
		return false;
	}
	std::cout << pFileName << std::endl;

	// FBX�}�l�[�W���[�̏�����
	auto manager = FbxManager::Create();

	// FBX�t�@�C���̓��o�͐ݒ�
	auto ioSetting = FbxIOSettings::Create(manager, IOSROOT);
	manager->SetIOSettings(ioSetting);

	// FBX�C���|�[�^�̏�����
	FbxImporter* importer = FbxImporter::Create(manager, "");
	// FBX�t�@�C���̓ǂݍ��݂̎��s
	if (!importer->Initialize(pFileName, -1, manager->GetIOSettings())) {
		// �t�@�C���ǂݍ��ݎ��s
		std::cout << "Invalid Fbx File." << std::endl;
		manager->Destroy();
		return false;
	}

	// FBX�V�[�� (3D��Ԃ��\������I�u�W�F�N�g���)��������
	FbxScene* scene = FbxScene::Create(manager, "");
	if (!importer->Import(scene)) {
		// �V�[���̓ǂݍ��ݎ��s
		manager->Destroy();
		return false;
	}

	// �V�[���̓ǂݍ��݊�������΃C���|�[�^�͕s�v�ƂȂ�
	importer->Destroy();

	std::cout << "Open Fbx File" << pFileName << std::endl;

	// ���������₷���悤�ɎO�p�|���S���ւ̕ϊ����s��
	FbxGeometryConverter geometryConverter(manager);
	geometryConverter.Triangulate(scene, true);

	// Mesh�ꗗ�擾
	auto meshCount = scene->GetMemberCount<FbxMesh>();
	std::vector<FbxMesh*> meshAttributes;
	for (auto i = 0; i < meshCount; i++) {
		auto pMesh = scene->GetMember<FbxMesh>(i);
		if (pMesh == NULL) {
			continue;
		}
		pMesh->GenerateTangentsDataForAllUVSets(false);
		meshAttributes.push_back(pMesh);
	}

	// MeshList�̍\�z
	BuildMeshList(pMeshList, meshAttributes);

	// �������
	manager->Destroy();

	std::cout << "End Create Mesh List" << std::endl;

	return true;
}

/// SkeltonList�̐��� (�������m��)
bool CreateSkeletonList(SkeletonList* poseList, const char* pFileName)
{
	if (pFileName == NULL) {
		std::cout << "FileName is Null." << std::endl;
		return false;
	}
	std::cout << pFileName << std::endl;

	// FBX�}�l�[�W���[�̏�����
	auto manager = FbxManager::Create();

	// FBX�t�@�C���̓��o�͐ݒ�
	auto ioSetting = FbxIOSettings::Create(manager, IOSROOT);
	manager->SetIOSettings(ioSetting);

	// FBX�C���|�[�^�̏�����
	FbxImporter* importer = FbxImporter::Create(manager, "");
	// FBX�t�@�C���̓ǂݍ��݂̎��s
	if (!importer->Initialize(pFileName, -1, manager->GetIOSettings())) {
		// �t�@�C���ǂݍ��ݎ��s
		std::cout << "Invalid Fbx File." << std::endl;
		manager->Destroy();
		return false;
	}

	// FBX�V�[�� (3D��Ԃ��\������I�u�W�F�N�g���)��������
	FbxScene* scene = FbxScene::Create(manager, "");
	if (!importer->Import(scene)) {
		// �V�[���̓ǂݍ��ݎ��s
		manager->Destroy();
		return false;
	}

	// �V�[���̓ǂݍ��݊�������΃C���|�[�^�͕s�v�ƂȂ�
	importer->Destroy();

	std::cout << "Open Fbx File" << pFileName << std::endl;

	// ���������₷���悤�ɎO�p�|���S���ւ̕ϊ����s��
	FbxGeometryConverter geometryConverter(manager);
	geometryConverter.Triangulate(scene, true);

	// Skelton�ꗗ�擾
	auto skeltonCount = scene->GetMemberCount<FbxSkeleton>();
	std::vector<FbxSkeleton*> skeletonAttributes;
	for (auto i = 0; i < skeltonCount; i++) {
		auto pSkeleton = scene->GetMember<FbxSkeleton>(i);
		if (pSkeleton == NULL) {
			continue;
		}
		skeletonAttributes.push_back(pSkeleton);
	}

	BuildSkeletonList(poseList, skeletonAttributes);

	// �������
	manager->Destroy();

	std::cout << "End Create Skelton List" << std::endl;

	return true;
}

/// MeshList�̉��
bool DestroySkeletonList(SkeletonList* pSkeletonList)
{
	if (pSkeletonList == NULL) {
		std::cout << "Skeleton List is Null." << std::endl;
		return false;
	}

	for (auto i = 0; i < 120; i++)
	{
		auto pose = pSkeletonList->pPoses[i];
		for (auto j = 0; j < pose.pSkeletons->Count; j++) {
			auto skeleton = pose.pSkeletons[i];
			SAFE_FREE(skeleton.pName);
			SAFE_FREE(skeleton.pMatrix);
		}
	}
	SAFE_FREE(pSkeletonList->pPoses);

	return true;
}

/// MeshList�̉��
bool DestroyMeshList(MeshList* pMeshList)
{
	if (pMeshList == NULL) {
		std::cout << "Mesh List is Null." << std::endl;
		return false;
	}

	for (auto i = 0; i < pMeshList->Count; i++) {
		auto mesh = pMeshList->pMeshes[i];
		SAFE_FREE(mesh.pName);
		SAFE_FREE(mesh.pPositions);
		SAFE_FREE(mesh.pNormals);
		SAFE_FREE(mesh.pColors);
		SAFE_FREE(mesh.pUv0);
		SAFE_FREE(mesh.pUv1);
		SAFE_FREE(mesh.pIndicies);
	}
	SAFE_FREE(pMeshList->pMeshes);

	return true;
}

/// MeshList�̍\�z
void BuildMeshList(MeshList* pMeshList, const std::vector<FbxMesh*>& meshAttributes)
{
	pMeshList->Count = (int)meshAttributes.size();
	pMeshList->pMeshes = (Mesh*)malloc(pMeshList->Count * sizeof(Mesh));

	auto index = 0;

	std::cout << "Start:" << pMeshList->Count << std::endl;
	for (auto itr = meshAttributes.begin(); itr != meshAttributes.end(); ++itr, index++) {
		auto pMeshAttribute = *itr;
		if (pMeshAttribute == NULL) {
			continue;
		}

		std::cout << "Begin:" << pMeshAttribute->GetNode()->GetName() << std::endl;

		// ���O�擾
		auto pName = pMeshAttribute->GetNode()->GetName();
		auto nameLength = strlen(pName);
		pMeshList->pMeshes[index].pName = (char*)malloc((nameLength + 1) * sizeof(char));
		strcpy_s(pMeshList->pMeshes[index].pName, nameLength + 1, pName);

		// Polygon�̉�� (Index���)
		pMeshList->pMeshes[index].TriangleCount = pMeshAttribute->GetPolygonCount();
		auto indiciesCount = pMeshList->pMeshes[index].TriangleCount * 3;

		std::vector<int> indicies;
		std::vector<int>** ppIndexTable = (std::vector<int>**)malloc(indiciesCount * sizeof(std::vector<int>*));
		for (auto i = 0; i < indiciesCount; i++) {
			ppIndexTable[i] = new std::vector<int>();
		}

		for (auto i = 0; i < pMeshList->pMeshes[index].TriangleCount; i++) {
			indicies.push_back(pMeshAttribute->GetPolygonVertex(i, 0));
			indicies.push_back(pMeshAttribute->GetPolygonVertex(i, 1));
			indicies.push_back(pMeshAttribute->GetPolygonVertex(i, 2));
		}

		pMeshList->pMeshes[index].pIndicies = (int*)malloc(indiciesCount * sizeof(int));
		for (auto i = 0; i < indiciesCount; i++) {
			pMeshList->pMeshes[index].pIndicies[i] = i;
			ppIndexTable[indicies[i]]->push_back(i);
		}

		// ���_���̉��
		pMeshList->pMeshes[index].VertexCount = indiciesCount;
		pMeshList->pMeshes[index].pPositions = CreatePositionList(pMeshAttribute, indicies);
		pMeshList->pMeshes[index].pNormals = CreateNormalList(pMeshAttribute, indicies, 0);
		pMeshList->pMeshes[index].pColors = CreateColorList(pMeshAttribute, indicies, 0);
		pMeshList->pMeshes[index].pUv0 = CreateUvList(pMeshAttribute, indicies, 0);
		pMeshList->pMeshes[index].pUv1 = CreateUvList(pMeshAttribute, indicies, 1);

		// �X�L�����̉��
		int skinListCount = GetSkinListCount(pMeshAttribute);
		pMeshList->pMeshes[index].SkinListCount = skinListCount;
		pMeshList->pMeshes[index].pSkinLists = CreateSkinList(pMeshAttribute, skinListCount, ppIndexTable);

		indicies.clear();
		for (auto i = 0; i < indiciesCount; i++) {
			(ppIndexTable[i])->clear();
			SAFE_DELETE(ppIndexTable[i]);
		}
		SAFE_FREE(ppIndexTable);
	}
	std::cout << "End Build Mesh" << std::endl;
}

/// SkeletonList�̍\�z
void BuildSkeletonList(SkeletonList* pSkeletonList, const std::vector<FbxSkeleton*>& skeletonAttributes)
{
	pSkeletonList->Count = 120;
	pSkeletonList->pPoses = (Pose*)malloc(pSkeletonList->Count * sizeof(Pose));

	std::cout << "Start:" << pSkeletonList->Count << std::endl;
	for (auto i = 0; i < 120; ++i)
	{
		auto index = 0;
		auto pPose = pSkeletonList->pPoses[i];
		pPose.Count = (int)skeletonAttributes.size();
		pPose.pSkeletons = (Skeleton*)malloc(pPose.Count * sizeof(Skeleton));
		for (auto itr = skeletonAttributes.begin(); itr != skeletonAttributes.end(); ++itr, index++) {
			auto pSkeletonAttribute = *itr;
			if (pSkeletonAttribute == NULL) {
				continue;
			}

			// ���O�擾
			auto pName = pSkeletonAttribute->GetNode()->GetName();
			auto nameLength = strlen(pName);
			pPose.pSkeletons[index].pName = (char*)malloc((nameLength + 1) * sizeof(char));
			strcpy_s(pPose.pSkeletons[index].pName, nameLength + 1, pName);

			// ���莞�Ԃł̎p���s��
			FbxTime fbxTime;
			auto targetFrame = FbxTime::GetOneFrameValue(FbxTime::eFrames30);
			fbxTime.Set(targetFrame * i);
			auto transform = pSkeletonAttribute->GetNode()->EvaluateGlobalTransform(fbxTime);
			int matrixIndex = 0;
			pPose.pSkeletons[index].pMatrix = (float*)malloc(sizeof(float) * 16);
			for (int row = 0; row < 4; row++) {
				for (int col = 0; col < 4; col++) {
					pPose.pSkeletons[index].pMatrix[matrixIndex] = (float)transform.Get(row, col);
					matrixIndex++;
				}
			}
		}
		pSkeletonList->pPoses[i] = pPose;
	}

	std::cout << "End Build Skeleton" << std::endl;
}

int GetSkinListCount(const FbxMesh* pMesh)
{
	auto skinCount = 0;
	for (int i = 0; i < pMesh->GetDeformerCount(); i++) {
		auto pDeformer = pMesh->GetDeformer(i);
		if (pDeformer->GetDeformerType() != FbxDeformer::eSkin) {
			continue;
		}
		skinCount++;
	}

	return skinCount;
}

/// SkinList�z��̐���
SkinList* CreateSkinList(const FbxMesh* pMesh, int skinListCount, std::vector<int>**& ppIndexTable)
{
	std::cout << skinListCount << std::endl;

	/* Bone���̒��o */
	auto skinList = (SkinList*)malloc(skinListCount * sizeof(SkinList));
	skinList->Count = skinListCount;
	int skinIndex = 0;
	auto tmpCount = 0;
	for (int i = 0; i < pMesh->GetDeformerCount(); i++) {
		auto pDeformer = pMesh->GetDeformer(i);
		if (pDeformer->GetDeformerType() != FbxDeformer::eSkin) {
			continue;
		}

		auto pSkin = static_cast<FbxSkin*>(pDeformer);
		int skinCount = pSkin->GetClusterCount();

		std::cout << "Mesh:" << pMesh->GetNode()->GetName() << " SkinCount: " << skinCount << std::endl;
		auto pSkins = (Skin*)malloc(skinCount * sizeof(Skin));
		for (int j = 0; j < skinCount; j++) {
			auto pCluster = pSkin->GetCluster(j);
			// ���O�擾
			auto pName = pCluster->GetLink()->GetName();
			auto nameLength = strlen(pName);
			pSkins[j].pName = (char*)malloc((nameLength + 1) * sizeof(char));
			strcpy_s(pSkins[j].pName, nameLength + 1, pName);

			int controlPointCount = pCluster->GetControlPointIndicesCount();

			/* Index��Weight�̏�� */
			auto indicies = std::vector<int>();
			auto weights = std::vector<float>();
			for (auto k = 0; k < controlPointCount; k++) {
				auto index = pCluster->GetControlPointIndices()[k];
				auto weight = (float)pCluster->GetControlPointWeights()[k];
				for (auto l = 0; l < ppIndexTable[index]->size(); l++) {
					indicies.push_back((*ppIndexTable[index])[l]);
					weights.push_back(weight);
				}
			}

			pSkins[j].Count = indicies.size();
			pSkins[j].pIndicies = (int*)malloc(indicies.size() * sizeof(int));
			pSkins[j].pWeights = (float*)malloc(weights.size() * sizeof(float));
			for (auto k = 0; k < indicies.size(); k++) {
				pSkins[j].pIndicies[k] = indicies[k];
				pSkins[j].pWeights[k] = weights[k];
				tmpCount++;
			}
			indicies.clear();
			weights.clear();

			/* �����p���̏�� */
			FbxAMatrix linkMatrix;
			pCluster->GetTransformLinkMatrix(linkMatrix);
			int matrixIndex = 0;
			auto pInitMatrix = (float*)malloc(sizeof(float) * 16);
			for (int row = 0; row < 4; row++) {
				for (int col = 0; col < 4; col++) {
					pInitMatrix[matrixIndex] = (float)linkMatrix.Get(row, col);
					matrixIndex++;
				}
			}
			pSkins[j].pInitMatrix = pInitMatrix;
		}
		skinList[skinIndex].Count = skinCount;
		skinList[skinIndex].pSkins = pSkins;
		skinIndex++;
	}

	std::cout << "COUNT: " << tmpCount << std::endl;
	return skinList;
}

/// ���_���W�z��̐���
float* CreatePositionList(const FbxMesh* pMesh, const std::vector<int>& indicies)
{
	auto positionCount = indicies.size();
	auto pPositions = (float*)malloc(positionCount * sizeof(float) * 3);

	for (auto i = 0; i < positionCount; i++) {
		auto index = indicies[i];
		auto position = pMesh->GetControlPointAt(index);
		pPositions[i * 3 + 0] = (float)position[0];
		pPositions[i * 3 + 1] = (float)position[1];
		pPositions[i * 3 + 2] = (float)position[2];
	}

	return pPositions;
}

/// ���_�J���[�z��̐���
float* CreateColorList(const FbxMesh* pMesh, const std::vector<int>& indicies, int colorIndex)
{
	// �v�f1�̂ݑΉ�
	auto elementCount = pMesh->GetElementVertexColorCount();
	if (colorIndex >= elementCount) {
		return NULL;
	}

	auto element = pMesh->GetElementVertexColor(colorIndex);
	// eDirect or eIndexToDirect�̂ݑΉ�
	auto referenceMode = element->GetReferenceMode();
	assert((referenceMode == FbxGeometryElement::eDirect) || (referenceMode == FbxGeometryElement::eIndexToDirect));

	auto colorCount = indicies.size();
	auto pColors = (float*)malloc(colorCount * sizeof(float) * 4);

	// ControlPoint�}�b�s���O
	const auto& indexArray = element->GetIndexArray();
	const auto& directArray = element->GetDirectArray();
	auto mappingMode = element->GetMappingMode();
	if (mappingMode == FbxGeometryElement::eByControlPoint) {
		for (auto i = 0; i < colorCount; i++) {
			auto index = indicies[i];
			auto colorIndex = referenceMode == FbxGeometryElement::eDirect ? index : indexArray.GetAt(index);
			auto color = directArray.GetAt(colorIndex);
			pColors[i * 2 + 0] = color[0];
			pColors[i * 2 + 1] = color[1];
			pColors[i * 2 + 2] = color[2];
			pColors[i * 2 + 3] = color[3];
		}
	}
	// PolygonVertex�}�b�s���O
	else if (mappingMode == FbxGeometryElement::eByPolygonVertex) {
		for (auto i = 0; i < colorCount; i++) {
			auto colorIndex = referenceMode == FbxGeometryElement::eDirect ? i : indexArray.GetAt(i);
			auto color = directArray.GetAt(colorIndex);
			pColors[i * 2 + 0] = color[0];
			pColors[i * 2 + 1] = color[1];
			pColors[i * 2 + 2] = color[2];
			pColors[i * 2 + 3] = color[3];
		}
	}

	return pColors;
}

/// UV�z��̐���
float* CreateUvList(const FbxMesh* pMesh, const std::vector<int>& indicies, int uvIndex)
{
	// �v�f1�̂ݑΉ�
	auto elementCount = pMesh->GetElementUVCount();
	if (uvIndex >= elementCount) {
		return NULL;
	}

	auto element = pMesh->GetElementUV(uvIndex);

	// eDirect or eIndexToDirect�̂ݑΉ�
	auto referenceMode = element->GetReferenceMode();
	assert((referenceMode == FbxGeometryElement::eDirect) || (referenceMode == FbxGeometryElement::eIndexToDirect));

	auto uvCount = indicies.size();
	auto pUvs = (float*)malloc(uvCount * sizeof(float) * 2);

	// ControlPoint�}�b�s���O
	const auto& indexArray = element->GetIndexArray();
	const auto& directArray = element->GetDirectArray();
	auto mappingMode = element->GetMappingMode();
	if (mappingMode == FbxGeometryElement::eByControlPoint) {
		for (auto i = 0; i < uvCount; i++) {
			auto index = indicies[i];
			auto uvIndex = referenceMode == FbxGeometryElement::eDirect ? index : indexArray.GetAt(index);
			auto uv = directArray.GetAt(uvIndex);
			pUvs[i * 2 + 0] = (float)uv[0];
			pUvs[i * 2 + 1] = 1.0f - (float)uv[1];
		}
	}
	// PolygonVertex�}�b�s���O
	else if (mappingMode == FbxGeometryElement::eByPolygonVertex) {
		for (auto i = 0; i < uvCount; i++) {
			auto uvIndex = referenceMode == FbxGeometryElement::eDirect ? i : indexArray.GetAt(i);
			auto uv = directArray.GetAt(uvIndex);
			pUvs[i * 2 + 0] = (float)uv[0];
			pUvs[i * 2 + 1] = 1.0f - (float)uv[1];
		}
	}

	return pUvs;
}

/// �@���z��̐���
float* CreateNormalList(const FbxMesh* pMesh, const std::vector<int>& indicies, int normalIndex)
{
	// �v�f1�̂ݑΉ�
	auto elementCount = pMesh->GetElementNormalCount();
	if (normalIndex >= elementCount) {
		return NULL;
	}

	auto element = pMesh->GetElementNormal(normalIndex);

	// eDirect or eIndexToDirect�̂ݑΉ�
	auto referenceMode = element->GetReferenceMode();
	assert((referenceMode == FbxGeometryElement::eDirect) || (referenceMode == FbxGeometryElement::eIndexToDirect));

	auto normalCount = indicies.size();
	auto pNormals = (float*)malloc(normalCount * sizeof(float) * 2);

	// ControlPoint�}�b�s���O
	const auto& indexArray = element->GetIndexArray();
	const auto& directArray = element->GetDirectArray();
	auto mappingMode = element->GetMappingMode();
	if (mappingMode == FbxGeometryElement::eByControlPoint) {
		for (auto i = 0; i < normalCount; i++) {
			auto index = indicies[i];
			auto normalIndex = referenceMode == FbxGeometryElement::eDirect ? index : indexArray.GetAt(index);
			auto normal = directArray.GetAt(normalIndex);
			pNormals[i * 2 + 0] = (float)normal[0];
			pNormals[i * 2 + 1] = (float)normal[1];
			pNormals[i * 2 + 2] = (float)normal[2];
		}
	}
	// PolygonVertex�}�b�s���O
	else if (mappingMode == FbxGeometryElement::eByPolygonVertex) {
		for (auto i = 0; i < normalCount; i++) {
			auto normalIndex = referenceMode == FbxGeometryElement::eDirect ? i : indexArray.GetAt(i);
			auto normal = directArray.GetAt(normalIndex);
			pNormals[i * 2 + 0] = (float)normal[0];
			pNormals[i * 2 + 1] = (float)normal[1];
			pNormals[i * 2 + 2] = (float)normal[2];
		}
	}

	return pNormals;
}