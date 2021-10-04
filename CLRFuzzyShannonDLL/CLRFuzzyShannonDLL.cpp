#include "pch.h"

#include "CLRFuzzyShannonDLL.h"

#include <iostream>
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
	extern "C" __declspec(dllexport) std::string computehash(const char* filepath)
	{
		return FuzzyShannon::FuzzyShannon().computehash(filepath);
	}
	extern "C" __declspec(dllexport) int comparehash(std::string hash1, std::string hash2)
	{
		return FuzzyShannon::FuzzyShannon().comparehash(hash1, hash2);
	}
}