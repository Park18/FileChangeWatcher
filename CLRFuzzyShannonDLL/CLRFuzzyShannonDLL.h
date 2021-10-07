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


	/*
	c#에서 사용법
	using System.Runtime.InteropServices;
	쓰고 시작하세요

	사용할 클래스 내에 
	[DllImport("CLRFuzzyShannonDLL.dll", CallingConvention = CallingConvention.Cdecl)]
	static extern double computeShannon(string str);
	[DllImport("CLRFuzzyShannonDLL.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern System.IntPtr computeHash(string str);
	[DllImport("CLRFuzzyShannonDLL.dll", CallingConvention = CallingConvention.Cdecl)]
	static extern int compareHash(string hash1, string hash2);
	선언 후 사용

	double computeShannon(string filepath);			filepath의 Shannon 값 double로 리턴



	IntPtr computeHash(string filepath);		filepath의 Fuzzy 값 IntPtr로 리턴
	사용법
	IntPtr p = computeHash(filepath);
    string c = Marshal.PtrToStringAnsi(p);
    Marshal.FreeHGlobal(p);



	int compareHash(string hash1, string hash2);	두 hash값의 스코어 값 반환

	*/

	extern "C" {
		__declspec(dllexport) double computeShannon(const char* filepath);
		__declspec(dllexport) char* computeHash(const char* filepath);
		__declspec(dllexport) int compareHash(const char* hash1, const char* hash2);
	}


}
