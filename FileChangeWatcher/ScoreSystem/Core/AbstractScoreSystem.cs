using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileChangeWatcher;

namespace FileChangeWatcher.ScoreSystem.Core
{
    abstract class AbstractScoreSystem
    {
        protected int _score = 0;
        protected DBMS dbms = new DBMS();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 점수 반환이 다소 난잡?한거 같음 좀 더 생각 필요
        /// </remarks>
        public int Score
        {
            get { return this._score; }
        }

        /// <summary>
        /// 점수를 계산하는 메소드
        /// </summary>
        public abstract void Calculate();

        /// <summary>
        /// 결과 출력 메소드
        /// </summary>
        /// <param name="percentage">백분율</param>
        protected abstract void PrintResult(double percentage);
    }
}
