using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChangeWatcher
{
    struct Data
    {
        string path;
        string fuzzy;
        string shannon;
    }

    class DBMS
    {
        private static int _totalFileNumbers = 0;
        private static List<string> _changeFileList = new List<string>();

        public int TotalFileNumbers
        {
            get { return _totalFileNumbers; }
        }

        public List<string> ChangeFileList
        {
            get { return _changeFileList; }
        }

        /// <summary>
        /// DB를 초기화하는 메소드
        /// </summary>
        /// <returns>DB 초기화 실패 여부</returns>
        public bool Init()
        {
            this.ResetChangeFileList();
            return true;
        }

        public void ResetChangeFileList() =>
            _changeFileList.Clear();

        public void AddChangeFile(string path)
            => _changeFileList.Add(path);
    }
}
