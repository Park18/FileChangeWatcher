using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChangeWatcher.ScoreSystem
{
    abstract class AbstractScoreSystem
    {
        protected int _score = 0;
        protected bool _isCompleteCalculate = false; // 변수명 바꾸고 싶음
        public int Score
        {
            get 
            {
                if (this._isCompleteCalculate)
                {
                    this._isCompleteCalculate = false;
                    return this._score;
                }
                else
                    return -1;
            }
        }

        /// <summary>
        /// ScoreSystem을 실행시키는 메소드
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// 점수를 계산하는 메소드
        /// </summary>
        protected abstract void Calculate();
    }
}
