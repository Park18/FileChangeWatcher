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
        /// <summary>
        /// 점수를 계산 메소드
        /// </summary>
        public override void Calculate()
        {
            this._isCompleteCalculate = true;

            int percentage = dbms.TotalFileNumbers / dbms.ChangeFileList.Count * 100;
        }
    }
}
