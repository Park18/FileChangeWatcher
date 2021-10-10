using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileChangeWatcher.ScoreSystem;

namespace FileChangeWatcher
{
    class FileChangeWatcher
    {
        /// <summary>
        /// Filesystem 관련
        /// </summary>
        private string path = @"C:\Users\NULL\Desktop\test";

        /// <summary>
        /// 타이머 관련
        /// </summary>
        private bool isFirstChange = true;
        private const int WaitingTime = 1000 * 10;
        private Thread thread;
        private Timer timer;

        /// <summary>
        /// ScoreSystem 관련
        /// </summary>
        private S1 s1 = new S1();
        private S2 s2 = new S2();
        private S3 s3 = new S3();

        /// <summary>
        /// DBMS 관련
        /// </summary>
        private DBMS dbms = new DBMS();

        public FileChangeWatcher()
        {
            dbms.Init();
        }

        public void Run()
        {
            var filesystemWatcher = new FileSystemWatcher(path);

            filesystemWatcher.NotifyFilter = NotifyFilters.Attributes
                                            | NotifyFilters.CreationTime
                                            | NotifyFilters.DirectoryName
                                            | NotifyFilters.FileName
                                            | NotifyFilters.LastAccess
                                            | NotifyFilters.LastWrite
                                            | NotifyFilters.Security
                                            | NotifyFilters.Size;

            filesystemWatcher.Changed += OnChanged;
            filesystemWatcher.Created += OnCreated;
            filesystemWatcher.Deleted += OnDeleted;
            filesystemWatcher.Renamed += OnRenamed;
            filesystemWatcher.Error += OnError;

            filesystemWatcher.IncludeSubdirectories = true;
            filesystemWatcher.EnableRaisingEvents = true;


            StreamReader sr = new StreamReader("OriginFileInfo.csv");
            string str;
            string[] strItems;
            if (!sr.EndOfStream)
            {
                // 첫줄 읽어서 헤더부분 넘김
                sr.ReadLine();
            }
            while (!sr.EndOfStream)
            {
                str = sr.ReadLine();
                strItems = str.Split(new string[] { ", " }, StringSplitOptions.None);
                CustomHashTable.loadOriginInfoToHashTable(strItems[0], strItems[1], Double.Parse(strItems[2]));
            }


            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"<Changed>: {e.FullPath}");
            Console.WriteLine($"[Time]: {DateTime.Now.ToString()}");

            this.CheckWork();
            this.dbms.AddChangeFile(e.FullPath);
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"<Created>: {e.FullPath}");
            Console.WriteLine($"[Time]: {DateTime.Now.ToString()}");

            this.CheckWork();
            this.dbms.AddChangeFile(e.FullPath);
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"<Deleted>: {e.FullPath}");
            Console.WriteLine($"[Time]: {DateTime.Now.ToString()}");

            this.CheckWork();

            // 삭제된 파일,폴더까지 변경점에 넣어야 하는지 의문
            //this.dbms.AddChangeFile(e.FullPath);
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            string filepath = e.FullPath;
            Console.WriteLine($"<Renamed>");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
            Console.WriteLine($"[Time]: {DateTime.Now.ToString()}");

            CustomHashTable.OriginAndChangePath.Put(e.OldFullPath, e.FullPath);
            CustomHashTable.ChangeAndOriginPath.Put(e.FullPath, e.OldFullPath);


            this.CheckWork();
            this.dbms.AddChangeFile(e.FullPath);
        }

        private void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private void PrintException(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"<Message>: {ex.Message}");
                Console.WriteLine("<Stacktrace>:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }

        /// <summary>
        /// 연속된 작업인지 확인하고 타이머를 생성하는 메소드
        /// </summary>
        private void CheckWork()
        {
            if (this.isFirstChange)
                this.isFirstChange = false;

            if (thread == null)
                thread = new Thread(SetTimer);

            else if (thread.ThreadState != ThreadState.Stopped || thread.ThreadState != ThreadState.Aborted)
            {
                thread.Abort();
                this.timer.Dispose();
                thread = new Thread(SetTimer);
            }

            thread.Start();
        }

        /// <summary>
        /// 연속된 작업의 끝을 알기 위한 타이머의 시간을 생성하는 메소드
        /// </summary>
        private void SetTimer()
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            this.timer = new Timer(this.TimerRun, autoResetEvent, WaitingTime, 0);

            autoResetEvent.WaitOne();
            this.timer.Dispose();
        }

        /// <summary>
        /// 연속된 작업의 끝을 알기 위한 타이머가 끝났을 때 작동하는 메소드
        /// </summary>
        private void TimerRun(Object stateInfo)
        {
            // 테스트 코드
            Console.WriteLine("Timer Run Start");

            // 플래그 초기화
            this.isFirstChange = true;

            // 계산
            //s1.Calculate();
            //s2.Calculate();
            s3.TestCode();

            // DB 초기화
            //dbms.Init();
            //dbms.ResetChangeFileList();

            // 타이머 초기화
            AutoResetEvent autoResetEvent = (AutoResetEvent)stateInfo;
            autoResetEvent.Set();

            // 테스트 코드
            Console.WriteLine("Timer Run End");
        }
    }
}