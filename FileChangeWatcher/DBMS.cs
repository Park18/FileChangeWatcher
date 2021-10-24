using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChangeWatcher
{
    /// <summary>
    /// 파일 정보
    /// </summary>
    struct DataInfo
    {
        public string Path { get; }
        public string Fuzzy { get; }
        public double Shannon { get; }

        public DataInfo(string path, string fuzzy, double shannon)
        {
            Path = path;
            Fuzzy = fuzzy;
            Shannon = shannon;
        }
    }

    class DBMS
    {
        private string iniFile = "Setting.ini";
        private string _rootPath = @"C:\Users\admin";
        private string _originFileInfoPath = @"OriginFileInfo.csv";
        private static List<string> _changeFileList = new List<string>();
        private static List<DataInfo> dataInfoList = new List<DataInfo>();

        public DBMS()
        {
            try
            {
                var iniFile = new IniFile();
                iniFile.Load(this.iniFile);
                _rootPath = iniFile["Setting"]["RootPath"].ToString();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("[Error]: Setting.ini 파일을 찾을 수 없습니다.");
                Console.WriteLine("[System]: Setting.ini 파일을 재생성합니다.");

                this.InitSettingFile(_rootPath);

                Environment.Exit(0);
            }

            this.Init();
        }

        public string RootPath 
        {
            get { return _rootPath; }
        }

        public int TotalFilesCount
        {
            get { return dataInfoList.Count; }
        }

        public List<string> ChangeFileList
        {
            get { return _changeFileList; }
        }

        public List<DataInfo> DataInfoList
        {
            get { return dataInfoList; }
        }

        /// <summary>
        /// DB를 초기화하는 메소드
        /// </summary>
        /// <returns>DB 초기화 실패 여부</returns>
        public bool Init()
        {
            dataInfoList.Clear();

            /// 관리 폴더 경로 이하 계층에 있는 파일을 순서대로 접근
            /// 접근한 파일의 경로, Fuzzy, Shaanon 정보를 Data 구조체를 이용해 List에 추가
            /// 모든 파일에 접근 완료 했으면 csv 파일에 정보 대입
            DFS(_rootPath);
            using (StreamWriter writeFile = new StreamWriter(_originFileInfoPath, false, System.Text.Encoding.GetEncoding("utf-8")))
            {
                writeFile.WriteLine("Path, Fuzzy, Shannon");

                foreach (DataInfo data in dataInfoList)
                {
                    writeFile.WriteLine($"{data.Path}, {data.Fuzzy}, {data.Shannon}");
                }
            }

            return true;
        }

        /// <summary>
        /// 변경된 파일 리스트 초기화
        /// </summary>
        /// <remarks>
        /// 변경된 파일 리스트가 인스턴스변수로 할지, 클래스변수로 할지 고민중
        /// 실행 위치도 고민중
        /// </remarks>
        public void ResetChangeFileList()
            => _changeFileList.Clear();

        /// <summary>
        /// 변경된 파일 경로를 DBMS에 추가시키는 메소드
        /// </summary>
        /// <param name="path">파일 경로</param>
        public void AddChangeFile(string path)
        {
            try
            {
                if (this.IsDirectory(path))
                    return;

                if(!_changeFileList.Contains(path))
                    _changeFileList.Add(path);
            }
            catch (Exception e)
            {
                Console.WriteLine($"예외처리: {e.Message}");
            }
        }

        /// <summary>
        /// 파일 트리를 DFS 알고리즘을 사용하여 file data 초기화
        /// </summary>
        /// <param name="directoryPath">디렉토리 경로</param>
        private void DFS(string directoryPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            if(directoryInfo.Exists)
            {
                foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
                {
                    DFS(directory.FullName);
                }

                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    dataInfoList.Add(
                        new DataInfo(
                            file.FullName,
                            FuzzyShannon.ComputeFuzzyHash(file.FullName),
                            FuzzyShannon.Shannon(file.FullName)
                        )
                    );
                }
            }
        }

        /// <summary>
        /// 매개변수로 받은 경로가 디렉토리 인지 확인하는 메소드
        /// </summary>
        /// <param name="path">경로</param>
        /// <returns>해당 경로가 디렉토리: true, 해당 경로가 디렉토리가 아님: false</returns>
        private bool IsDirectory(string path)
        {
            FileAttributes fileAttributes = File.GetAttributes(path);
            if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                return true;

            return false;
        }

        public void InitSettingFile(string rootPath)
        {
            var iniFile = new IniFile();
            iniFile["Setting"]["RootPath"] = rootPath;
            iniFile.Save("Setting.ini");
        }

        public void TestCode()
        {
            //this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\0.jajajajajajaj");
            //this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\1.exe");
            //this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\1.exe");
            //this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\1.exe");
            //this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\1.exe");
            //this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\2.pdf");
            //this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\3.hwp");
            //this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\4.docx");
            //this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\5.pdf");
            //this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\6.hwp");
            //this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\7.pdf");
            //this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\8.asfasdf");
            //this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b\9.fasfasd");
            //this.AddChangeFile(@"D:\Code\Capstone\FileChangeWatcher\FileChangeDataset\b");
            //foreach (string path in _changeFileList)
            //    Console.WriteLine(path);

            this.Init();
        }
    }
}
