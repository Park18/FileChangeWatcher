#include "pch.h"

#include <Windows.h>
#include <iostream>
#include <vector>

#include "CLRFuzzyShannonDLL.h"

using namespace System;
using namespace System::Collections;

namespace CLRFuzzyShannonDLL
{
	FuzzyShannon::FuzzyShannon() : m_pLibFuzzyShannon(new LibFuzzyShannon)
	{
	}

	FuzzyShannon::~FuzzyShannon()
	{
		if (m_pLibFuzzyShannon)
		{
			delete m_pLibFuzzyShannon;
			m_pLibFuzzyShannon = 0;
		}
	}

	double FuzzyShannon::Shannon(const char* filepath)
	{
		return (m_pLibFuzzyShannon->Shannon(filepath));
	}

	std::string FuzzyShannon::computehash(const char* filepath)
	{
		return (m_pLibFuzzyShannon->computehash(filepath));
	}

	int FuzzyShannon::comparehash(std::string hash1, std::string hash2)
	{
		return (m_pLibFuzzyShannon->comparehash(hash1, hash2));
	}


	extern "C" __declspec(dllexport) double Shannon(const char* filepath)
	{
		return FuzzyShannon::FuzzyShannon().Shannon(filepath);
	}
	extern "C" __declspec(dllexport) char* computehash(const char* filepath)
	{

		std::string str = FuzzyShannon::FuzzyShannon().computehash(filepath);
		const char* pctr = str.c_str();
		char* result = (char*)LocalAlloc(LPTR, strlen(pctr) + 1);

		strcpy(result, pctr);

		LocalFree(result);

		return result;
	}
	extern "C" __declspec(dllexport) int comparehash(const char* chash1, const char* chash2)
	{
		std::string hash1 = chash1;
		std::string hash2 = chash2;
		return FuzzyShannon::FuzzyShannon().comparehash(hash1, hash2);
	}
}