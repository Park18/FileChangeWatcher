#pragma once
#include <string>

using namespace System;

namespace FuzzyShannon {
	//파일의 Shannon 엔트로피 계산, 엔트로피 값 리턴
	extern "C" __declspec(dllexport) double Shannon(const char* filepath);

	//해쉬값 계산, 해쉬값 반환
	extern "C" __declspec(dllexport) std::string computehash(const char* filepath);

	//두 퍼지값 비교, 유사도 스코어 반환
	extern "C" __declspec(dllexport) int comparehash(std::string hash1, std::string hash2);
}
