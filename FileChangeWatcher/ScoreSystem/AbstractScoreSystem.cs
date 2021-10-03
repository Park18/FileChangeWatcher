using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileChangeWatcher;

namespace FileChangeWatcher.ScoreSystem
{
    abstract class AbstractScoreSystem
    {
        protected int _score = 0;
        protected bool _isCompleteCalculate = false; // 변수명 바꾸고 싶음
        protected DBMS dbms = new DBMS();

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
        /// 점수를 계산하는 메소드
        /// </summary>
        public abstract void Calculate();
    }
}
