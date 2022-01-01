using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileChangeWatcher.ScoreSystem;
using System.Threading;

namespace FileChangeWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            /// ScoreSystem 테스트
            //S1 s1 = new S1();
            //S2 s2 = new S2();
            //DBMS dbms = new DBMS();

            //dbms.TestCode();
            //s1.TestCode();
            //s2.TestCode();
            Console.WriteLine("Shannon 엔트로피의 계산 영역 Byte를 입력해주세요 :");
            string getShannonByte = Console.ReadLine();
            FuzzyShannon.setShannonByte(int.Parse(getShannonByte));
            /// Program 테스트
            FileChangeWatcher fileChangeWatcher = new FileChangeWatcher();
            fileChangeWatcher.Run();
        }
    }
}
