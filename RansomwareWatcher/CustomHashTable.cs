using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChangeWatcher
{
    class CustomHashTable
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
    }
}
