using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FileChangeWatcher.ScoreSystem
{
    class S3 : AbstractScoreSystem
    {

        private List<double> ShannonList = new List<double>();


        public override void Calculate()
        {
            TestCode();
        }


        /// <summary>
        /// 표준 편차 계산
        /// </summary>
        private double standardDeviation(IEnumerable<double> sequence)
        {
            double average = sequence.Average();
            double sum = sequence.Sum(d => Math.Pow(d - average, 2));
            return Math.Sqrt((sum) / (sequence.Count() - 1));
        }

        public void TestCode()
        {
            loadShannoninOriginCSV();
            
        }


        /// <summary>
        /// CSV로부터 Shaanon값을 읽어 표준편차를 구함
        /// </summary>
        private void loadShannoninOriginCSV()
        {
            StreamReader sr = new StreamReader("OriginFileInfo.csv");
            string str;
            string[] strItems;
            if (!sr.EndOfStream)
            {
                // 첫줄 읽어서 헤더부분 넘김
                sr.ReadLine();
            }
            while(!sr.EndOfStream)
            {
                str = sr.ReadLine();
                strItems = str.Split(new string[] { ", " }, StringSplitOptions.None);
                ShannonList.Add(Double.Parse(strItems[2]));
                //loadOriginInfoToHashTable(strItems[0], strItems[1], Double.Parse(strItems[2]));
                //Console.WriteLine(strItems[2]);
            }

            double standar_deviation = standardDeviation(ShannonList);
            Console.WriteLine("표준 편차는 " + standar_deviation);
        }

        private void subShannon(string Path)
        {
            string a = (string)CustomHashTable.ChangeAndOriginPath.GetValue(Path);
            if (CustomHashTable.OriginShannoCHT.GetValue(a) != null)
            {
                //부동소수점 계산 오류 방지
                decimal b = (decimal)(double)CustomHashTable.OriginShannoCHT.GetValue(a);
                decimal d = (decimal)(double)FuzzyShannon.Shannon(Path);
                Console.WriteLine(b - d);
            }
        }

    }

    public class CustomHashTable
    {
        //원본 퍼지 해시테이블
        public static CustomHashTable OriginFuzzyCHT = new CustomHashTable(30);
        //원본 샤넌 해시테이블
        public static CustomHashTable OriginShannoCHT = new CustomHashTable(30);
        //원본 샤넌 해시테이블
        public static CustomHashTable OriginAndChangePath = new CustomHashTable(30);
        //원본 샤넌 해시테이블
        public static CustomHashTable ChangeAndOriginPath = new CustomHashTable(30);

        private const int INITIAL_SIZE = 16;
        private int size;
        private Node[] buckets;


        /// <summary>
        /// 파라미터가 없으면 인덱스 16개로 구성
        /// </summary>
        public CustomHashTable()
        {
            this.size = INITIAL_SIZE;
            this.buckets = new Node[size];
        }


        /// <summary>
        /// 파라미터만큼 인덱스 구성
        /// </summary>
        public CustomHashTable(int capacity)
        {
            this.size = capacity;
            this.buckets = new Node[size];
        }

        /// <summary>
        /// 해시테이블에 추가
        /// </summary>
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
        /// 해시테이블에서 검색 후 value 리턴
        /// </summary>
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
                        Console.WriteLine("찾는중이에요...");
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 해시테이블에서 검색 후 존재 여부 확인
        /// </summary>
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
        /// 해시테이블 인덱스 계산
        /// </summary>
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
        public static void loadOriginInfoToHashTable(string key, string value1, double value2)
        {
            CustomHashTable.OriginFuzzyCHT.Put(key, value1);
            CustomHashTable.OriginShannoCHT.Put(key, value2);
        }
    }

}
