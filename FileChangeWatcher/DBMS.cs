using System;
using System.IO;
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
        private static List<Data> _dataList = new List<Data>();

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

            // 관리 폴더 경로 이하 계층에 있는 파일을 순서대로 접근
            // 접근한 파일의 경로, Fuzzy, Shaanon 정보를 Data 구조체를 이용해 List에 추가
            // 모든 파일에 접근 완료 했으면 csv 파일에 정보 대입

            return true;
        }

        public void ResetChangeFileList()
            => _changeFileList.Clear();

        public void AddChangeFile(string path)
        {
            try
            {
                FileAttributes fileAttributes = File.GetAttributes(path);
                if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                    return;

                if(!_changeFileList.Contains(path))
                    _changeFileList.Add(path);
            }
            catch (Exception e)
            {
                Console.WriteLine($"예외처리: {e.Message}");
            }
        }

        public void TestCode()
        {
            this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\0.jajajajajajaj");
            this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\1.exe");
            this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\1.exe");
            this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\1.exe");
            this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\1.exe");
            this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\2.pdf");
            this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\3.hwp");
            this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\4.docx");
            this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\5.pdf");
            this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\6.hwp");
            this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\7.pdf");
            this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\8.asfasdf");
            this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\9.fasfasd");
            this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b");
            foreach (string path in _changeFileList)
                Console.WriteLine(path);
        }
    }
}
