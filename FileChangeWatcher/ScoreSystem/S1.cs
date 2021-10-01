using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChangeWatcher.ScoreSystem
{
    /// <summary>
    /// S1 점수 클래스
    /// </summary>
    /// <remarks>
    /// 전체 파일중에 변경된 파일의 개수에 따른 점수
    /// </remarks>
    class S1 : AbstractScoreSystem
    {
        private static int WaitingTime = 1000 * 10;

        private bool isFirstChange = true;  // 첫 번째 변화인지 확인하는 플래그
        private Thread thread;
        private Timer timer;

        /// <summary>
        /// S1 시스템 실행 메소드
        /// </summary>
        public override void Run()
        {
            this.Calculate();
        }

        /// <summary>
        /// 점수를 계산하는 메소드
        /// </summary>
        protected override void Calculate()
        {
            this._isCompleteCalculate = true;
            Console.WriteLine("Calculate(): Begin");


        }
    }
}
