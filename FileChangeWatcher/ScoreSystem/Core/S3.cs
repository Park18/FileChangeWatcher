﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FileChangeWatcher.ScoreSystem.Core
{
    class S3 : AbstractScoreSystem
    {

        private List<double> ShannonList = new List<double>();
        private List<double> ChangeShannonList = new List<double>();
        private List<double> ChangeFuzzyHashList = new List<double>();
        List<DataInfo> TdataInfoList = new List<DataInfo>();
        List<string> TchangeFileList = new List<string> ();
        private const double FuzzyhashThreshold = 0.8;
        private const double ShannonThreshold = 0.25;
        private double Origin_standard_deviation;
        private double Change_standard_deviation;
        int NumZeroFuzzyhash = 0;


        public override void Calculate()
        {
            OriginShannonStandardDeviation();
            loadShannoninChangeAndOriginPath();
            double percentage = (double)NumZeroFuzzyhash / TchangeFileList.Count();
            if (percentage >= FuzzyhashThreshold || Change_standard_deviation <= ShannonThreshold)
                _score = 5;
            else
                _score = 0;
            this.PrintResult(percentage);

        }

        protected override void PrintResult(double percentage)
        {
            Console.WriteLine($"[System] S3 테스트 결과");
            Console.WriteLine($"[System] FuzzyHash 0 비율: {percentage * 100}%");
            Console.WriteLine($"[System] 표준 편차: {Change_standard_deviation}");
            Console.WriteLine($"[System] 점수: {this._score}점");

        }

        /// <summary>
        /// 표준 편차 계산
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        private double standardDeviation(IEnumerable<double> sequence)
        {
            double average = sequence.Average();
            double sum = sequence.Sum(d => Math.Pow(d - average, 2));
            return Math.Sqrt((sum) / (sequence.Count() - 1));
        }

        public void TestCode()
        {
            
            


        }


        /// <summary>
        /// DataInfoList로부터 Shaanon값을 읽어 표준편차를 구함
        /// 이 과정에서 원본 퍼지 해시테이블(OriginFuzzyCHT), 원본 사년 해시테이블(OriginShannoCHT)이 채워짐
        /// </summary>
        private void OriginShannonStandardDeviation()
        {
            TdataInfoList = dbms.DataInfoList;
            foreach (var num in TdataInfoList)
            {
                CustomHashTable.loadOriginInfoToHashTable(num.Path, num.Fuzzy, num.Shannon);
            }
            ShannonList = TdataInfoList.Select(x => x.Shannon).ToList();
            Origin_standard_deviation = standardDeviation(ShannonList);
            //Console.WriteLine("초기 파일들의 Shannon 표준 편차는 " + Origin_standard_deviation);
        }


        /// <summary>
        /// ChangeFileList로부터 Shaanon값을 읽어 표준편차를 구함
        /// </summary>
        private void loadShannoninChangeAndOriginPath()
        {

            int flagChange = 0;

            TchangeFileList = dbms.ChangeFileList;
            foreach (string ChangeFilePath in TchangeFileList)
            {
                //object a = CustomHashTable.ChangeGetOriginPath.GetValue(ChangeFilePath);
                if (ChangeFilePath != null)
                {
                    double CFileShannon = FuzzyShannon.Shannon(ChangeFilePath);
                    if(CFileShannon != 10)
                    {
                        ChangeShannonList.Add(CFileShannon);
                        flagChange = 1;
                    }
                }
            }
            if(flagChange == 1)
            {
                Change_standard_deviation = standardDeviation(ChangeShannonList);
                //Console.WriteLine("변조 파일들의 Shannon 표준 편차는 " + Change_standard_deviation);

                subNums();
            }
        }



        /// <summary>
        /// 샤년과 퍼지값의 변화를 파악
        /// </summary>
        private void subNums()
        {
            
            foreach (string ChangeFilePath in TchangeFileList)
            {
                object a = CustomHashTable.ChangeGetOriginPath.GetValue(ChangeFilePath);
                if (a != null)
                {
                    subShannonNum(ChangeFilePath, (string)a);
                    string b = FuzzyShannon.ComputeFuzzyHash(ChangeFilePath);
                    if(b != "open error")
                    {
                        int score = FuzzyShannon.CompareFuzzyHash((string)CustomHashTable.OriginFuzzyCHT.GetValue(a), b);
                        if(score != 101)
                        {
                            ChangeFuzzyHashList.Add(score);
                            if (score == 0)
                            {
                                NumZeroFuzzyhash++;
                            }
                        }                        
                    }                    
                }
                else
                {
                    if (ChangeFilePath != null)
                    {
                        subShannonNum(ChangeFilePath, ChangeFilePath);
                        string b = FuzzyShannon.ComputeFuzzyHash(ChangeFilePath);
                        if(b != "open error")
                        {
                            if (CustomHashTable.OriginFuzzyCHT.GetValue(ChangeFilePath) != null)
                            {
                                int score = FuzzyShannon.CompareFuzzyHash((string)CustomHashTable.OriginFuzzyCHT.GetValue(ChangeFilePath), b);
                                if(score != 101)
                                {
                                    ChangeFuzzyHashList.Add(score);
                                    if (score == 0)
                                    {
                                        NumZeroFuzzyhash++;
                                    }
                                }                                
                            }
                        }
                    }
                }
            }
            //Console.WriteLine("FuzzyHash 연산 결과가 0인 파일 수 : " + NumZeroFuzzyhash);

            CreateFuzzyHashCSV();
            CreateShannonCSV();
        }

        /// <summary>
        /// 샤넌 변화 확인
        /// </summary>
        /// <param name="ChangePath"></param>
        /// <param name="OriginPath"></param>
        private void subShannonNum(string ChangePath, string OriginPath)
        {
            if (CustomHashTable.OriginShannoCHT.GetValue(OriginPath) != null)
            {
                //부동소수점 계산 오류 방지
                decimal b = (decimal)(double)CustomHashTable.OriginShannoCHT.GetValue(OriginPath);
                decimal d = (decimal)(double)FuzzyShannon.Shannon(ChangePath);
                decimal c = Math.Abs(b - d);
            }
        }

        public void CreateShannonCSV()
        {
            using (StreamWriter writeFile = new StreamWriter((@"Shannon.csv"), false, System.Text.Encoding.GetEncoding("utf-8")))
            {
                foreach (double List in ChangeShannonList)
                {
                    writeFile.WriteLine(List);
                }
            }
        }

        public void CreateFuzzyHashCSV()
        {
            using (StreamWriter writeFile = new StreamWriter((@"FuzzyHash.csv"), false, System.Text.Encoding.GetEncoding("utf-8")))
            {
                foreach (double List in ChangeFuzzyHashList)
                {
                    writeFile.WriteLine(List);
                }
            }
        }

    }

    /// 별도로 클래스 만듬
    /// 이유: 
    ///     1. ScoreSystem 구조 변경 -> FileChangeWatcher에서 에러 발생
    ///     2. S3라는 클래스랑 맞지 않는거 같음, 클래스명과 비슷한 기능을 하는게 있어야 한다고 생각함
    /*public class CustomHashTable
    {
        //원본 퍼지 해시테이블
        public static CustomHashTable OriginFuzzyCHT = new CustomHashTable(30);
        //원본 샤넌 해시테이블
        public static CustomHashTable OriginShannoCHT = new CustomHashTable(30);
        //원본 경로로 변경된 경로를 얻어오기 위한 해시테이블
        public static CustomHashTable OriginGetChangePath = new CustomHashTable(30);
        //변경된 경로로 원본 경로를 얻어오기 위한 해시테이블
        public static CustomHashTable ChangeGetOriginPath = new CustomHashTable(30);

        private const int INITIAL_SIZE = 16;
        private int size;
        private Node[] buckets;


        /// <summary>
        /// 파라미터가 없으면 기본 인덱스 16개
        /// </summary>
        public CustomHashTable()
        {
            this.size = INITIAL_SIZE;
            this.buckets = new Node[size];
        }



        /// <summary>
        /// 파라미터만큼 인덱스 구성
        /// </summary>
        /// <param name="capacity"></param>
        public CustomHashTable(int capacity)
        {
            this.size = capacity;
            this.buckets = new Node[size];
        }


        /// <summary>
        /// 해시테이블에 추가
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Put(object key, object value)
        {
            int index = HashFunction(key);
            if (buckets[index] == null)
            {
                buckets[index] = new Node(key, value);
            }
            else
            {
                if (buckets[index].Key == key && buckets[index].Value == value)
                {
                    Console.WriteLine("이전 테이블과 중복데이터입니다.");
                }
                else
                {
                    Node newNode = new Node(key, value);
                    newNode.Next = buckets[index];
                    buckets[index] = newNode;

                }
            }
        }


        /// <summary>
        /// 해시테이블에서 검색후 Value 반환
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetValue(object key)
        {
            int index = HashFunction(key);

            if (buckets[index] != null)
            {
                for (Node n = buckets[index]; n != null; n = n.Next)
                {
                    if ((string)n.Key == (string)key)
                    {
                        return n.Value;
                    }
                    else
                    {

                    }
                }
            }
            return null;
        }


        /// <summary>
        /// 해시테이블에 존재여부 파악
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(object key)
        {
            int index = HashFunction(key);
            if (buckets[index] != null)
            {
                for (Node n = buckets[index]; n != null; n = n.Next)
                {
                    if (n.Key == key)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// 해시테이블 인덱스 배정
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual int HashFunction(object key)
        {
            return Math.Abs(key.GetHashCode() + 1 + (((key.GetHashCode() >> 5) + 1) % (size))) % size;
        }

        private class Node
        {
            public object Key { get; set; }
            public object Value { get; set; }
            public Node Next { get; set; }

            public Node(object key, object value)
            {
                this.Key = key;
                this.Value = value;
                this.Next = null;
            }
        }


        /// <summary>
        /// key 받아서 value1, value2를 각 OriginFuzzyCHT, OriginShannoCHT에 넣음
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        public static void loadOriginInfoToHashTable(string key, string value1, double value2)
        {
            CustomHashTable.OriginFuzzyCHT.Put(key, value1);
            CustomHashTable.OriginShannoCHT.Put(key, value2);
        }
    }*/

}
