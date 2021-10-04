#pragma once

using namespace System;

#include "../FuzzyShannonStaticLib/FuzzyShannonStaticLib.h"

namespace CLRFuzzyShannonDLL {
	public ref class FuzzyShannon
	{
	protected:
		LibFuzzyShannon* m_pLibFuzzyShannon;
	public:
		FuzzyShannon();
		virtual ~FuzzyShannon();
		double Shannon(const char* filepath);
		std::string computehash(const char* filepath);
		int comparehash(std::string hash1, std::string hash2);

	};


	extern "C" {
		__declspec(dllexport) double Shannon(const char* filepath);
		__declspec(dllexport) std::string computehash(const char* filepath);
		__declspec(dllexport) int comparehash(std::string hash1, std::string hash2);
	}

}
