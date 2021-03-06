// stdafx.h : 標準のシステム インクルード ファイルのインクルード ファイル、
// または、参照回数が多く、かつあまり変更されない、プロジェクト専用のインクルード ファイル
// を記述します。
//

#pragma once

#include "targetver.h"

#define WIN32_LEAN_AND_MEAN             // Windows ヘッダーからほとんど使用されていない部分を除外する
// Windows ヘッダー ファイル
#include <windows.h>

// プログラムに必要な追加ヘッダーをここで参照してください
#define DllExport extern "C" __declspec(dllexport)

#define SAFE_FREE(p) { if (p != NULL) { free(p); p = NULL; } }
#define SAFE_DELETE(p) { delete p; p = NULL; }