using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileChangeWatcher.ScoreSystem.Core;

namespace FileChangeWatcher.ScoreSystem
{
    class ScoreSystem
    {
        private S1 s1 = new S1();
        private S2 s2 = new S2();
        private S3 s3 = new S3();


        public void Run()
        {
            s1.Calculate();
            s2.Calculate();
            s3.Calculate();

            this.PrintResult();
        }

        private void PrintResult()
        {
            int sum = s1.Score + s2.Score + s3.Score;

            if(sum >=7)
            {
                Console.WriteLine("[System] 랜섬웨어 감염 확신");
            }
            else if(sum >= 5)
            {
                Console.WriteLine("[System] 랜섬웨어 감염 경고");
            }
            else if(sum >= 3)
            {
                Console.WriteLine("[System] 랜섬웨어 감염 의심");
            }
            else
            {
                Console.WriteLine("[System] 랜섬웨어 감염 아님");
            }
        }
    }
}
