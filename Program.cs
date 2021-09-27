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
            // S1 테스트
            // S1 s1 = new S1();
            // s1.Run();
            // 
            // Thread.Sleep(1000 * 3);
            // s1.Run();

            // S2 테스트
            // S2 s2 = new S2();
            // s2.Run();

            FileChangeWatcher fileChangeWatcher = new FileChangeWatcher();
            fileChangeWatcher.Run();
        }
    }
}
