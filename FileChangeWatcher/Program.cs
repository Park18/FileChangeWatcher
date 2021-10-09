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
            // ScoreSystem 테스트
            S1 s1 = new S1();
            S2 s2 = new S2();
            DBMS dbms = new DBMS();

            //dbms.TestCode();
            //s2.TestCode();

            s1.Calculate();
            s2.Calculate();

            // Program 테스트
            FileChangeWatcher fileChangeWatcher = new FileChangeWatcher();
            fileChangeWatcher.Run();
        }
    }
}
