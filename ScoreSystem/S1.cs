using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChangeWatcher.ScoreSystem
{
    class S1 : AbstractScoreSystem
    {
        private static int WaitingTime = 1000 * 5;

        private int count = 0;
        private bool isFirstChange = true;  // 첫 번째 변화인지 확인하는 플래그
        private Thread thread;

        public override void Run()
        {
            if (this.isFirstChange) 
            { 
                this.isFirstChange = false; 
            }

            // 타이머 생성
            // 1. 타이머 쓰레드 생성되어 있는지 확인
            //      1.1. 타이머 쓰레드가 생성되어 있으면 삭제
            // 2. 타이머 쓰레드 생성
            if (thread == null)
                thread = new Thread(SetTimer);

            else if (thread.ThreadState != ThreadState.Stopped)
            {
                Console.WriteLine("Thread.Abort()");
                thread.Abort();
                thread = new Thread(SetTimer);
            }

            thread.Start();
        }

        /// <summary>
        /// 타이머의 시간을 설정하는 메소드
        /// </summary>
        private void SetTimer()
        {
            // Test Code
            Console.WriteLine("Thread{0}: begin", Thread.CurrentThread.ManagedThreadId);

            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            Timer timer = new Timer(this.timer, autoResetEvent, WaitingTime, 0);

            autoResetEvent.WaitOne();
            timer.Dispose();

            Console.WriteLine("Thread{0}: end", Thread.CurrentThread.ManagedThreadId);
        }

        /// <summary>
        /// 설정된 시간에 작동하는 타이머 메소드
        /// </summary>
        /// <remarks></remarks>
        private void timer(Object stateInfo)
        {
            this.isFirstChange = true;
            this.Calculate();

            AutoResetEvent autoResetEvent = (AutoResetEvent)stateInfo;
            autoResetEvent.Set();
        }

        /// <summary>
        /// 점수를 계산하는 메소드
        /// </summary>
        protected override void Calculate()
        {
            Console.WriteLine("Calculate(): Begin");
        }
    }
}
