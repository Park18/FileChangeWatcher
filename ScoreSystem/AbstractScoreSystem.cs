using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChangeWatcher.ScoreSystem
{
    abstract class AbstractScoreSystem
    {
        private int _score;

        public int Score
        {
            get { return this._score; }
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
