using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChangeWatcher.ScoreSystem.Core
{
    /// <summary>
    /// S1 점수 클래스
    /// </summary>
    /// <remarks>
    /// 전체 파일중에 변경된 파일의 개수에 따른 점수
    /// </remarks>
    class S1 : AbstractScoreSystem
    {
        private const double Threshold = 0.95;

        /// <summary>
        /// 점수를 계산 메소드
        /// </summary>
        public override void Calculate()
        {
            try
            {
                double percentage = (double)dbms.ChangeFileList.Count / dbms.TotalFilesCount;

                /// 파일 + .lock + 감염 txt 파일 
                if (dbms.ChangeFileList.Count > dbms.TotalFilesCount + 2)
                {
                    this._score = 0;
                    this.PrintResult();
                    return;
                }
                else if (percentage >= Threshold)
                    this._score = 3;
                else
                    this._score = 0;

                this.PrintResult(percentage);

            }
            catch(DivideByZeroException)
            {
                return;
            }
        }

        /// <summary>
        /// 예외 발생시 결과 출력 메소드
        /// </summary>
        private void PrintResult()
        {
            Console.WriteLine($"[System] S1 테스트 결과");
            Console.WriteLine($"[Error] 감지된 파일이 총 파일 개수보다 많습니다.");
            Console.WriteLine($"[System] 점수: {this._score}점");
        }

        protected override void PrintResult(double percentage)
        {
            Console.WriteLine($"[System] S1 테스트 결과");
            Console.WriteLine($"[System] 변화율 - {percentage * 100}%");
            Console.WriteLine($"[System] 점수 - {this._score}점");
        }
    }
}
