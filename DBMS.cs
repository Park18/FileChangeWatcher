using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChangeWatcher
{
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

        public bool Init()
        {
            return true;
        }

        public void ResetChangeFileList() =>
            _changeFileList.Clear();
    }
}
