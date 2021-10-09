using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace FileChangeWatcher
{
    class FuzzyShannon
    {
        [DllImport("CLRFuzzyShannonDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern double computeShannon(string str);
        [DllImport("CLRFuzzyShannonDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern System.IntPtr computeHash(string str);
        [DllImport("CLRFuzzyShannonDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int compareHash(string hash1, string hash2);

        /// <summary>
        /// Shannon을 반환하는 메소드
        /// </summary>
        /// <param name="filePath">Shannon을 계산할 파일 경로</param>
        /// <returns></returns>
        public static double Shannon(string filePath)
        {
            return computeShannon(filePath);
        }

        /// <summary>
        /// FuzzyHash를 반환하는 메소드
        /// </summary>
        /// <param name="filePath">FuzzyHash를 계산할 파일 경로</param>
        /// <returns></returns>
        public static string ComputeFuzzyHash(string filePath)
        {
            IntPtr intPtrFuzzyHash = computeHash(filePath);
            string strFuzzyHash = Marshal.PtrToStringAnsi(intPtrFuzzyHash);
            Marshal.FreeHGlobal(intPtrFuzzyHash);

            return strFuzzyHash;
        }

        /// <summary>
        /// FuzzyHash를 비교하는 메소드
        /// </summary>
        /// <param name="fuzzyHash1">Fuzzh Hash 1</param>
        /// <param name="fuzzyHash2">Fuzzy Hash 2</param>
        /// <returns></returns>
        public static int CompareFuzzyHash(string fuzzyHash1, string fuzzyHash2)
        {
            return compareHash(fuzzyHash1, fuzzyHash2);
        }
    }
}
