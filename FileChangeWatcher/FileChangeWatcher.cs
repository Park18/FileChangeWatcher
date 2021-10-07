using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileChangeWatcher.ScoreSystem;
using System.Runtime.InteropServices;

namespace FileChangeWatcher
{
    class FileChangeWatcher
    {
        
        [DllImport("CLRFuzzyShannonDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern double computeShannon(string str);
        [DllImport("CLRFuzzyShannonDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern System.IntPtr computeHash(string str);
        [DllImport("CLRFuzzyShannonDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int compareHash(string hash1, string hash2);

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

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"Changed: {e.FullPath} - time: {DateTime.Now.ToString()}");

            this.CheckWork();
            this.dbms.AddChangeFile(e.FullPath);
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Created: {e.FullPath} - time: {DateTime.Now.ToString()}");

            this.CheckWork();
            this.dbms.AddChangeFile(e.FullPath);
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Deleted: {e.FullPath} - time: {DateTime.Now.ToString()}");

            this.CheckWork();

            // 삭제된 파일,폴더까지 변경점에 넣어야 하는지 의문
            //this.dbms.AddChangeFile(e.FullPath);
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            string filepath = e.FullPath;
            Console.WriteLine($"Renamed: - time: {DateTime.Now.ToString()}");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
            double sp = computeShannon(filepath);
            IntPtr p = computeHash(filepath);
            string c = Marshal.PtrToStringAnsi(p);
            Marshal.FreeHGlobal(p);
            int ph = compareHash(c, c);
            this.CheckWork();
            this.dbms.AddChangeFile(e.FullPath);
        }

        private void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private void PrintException(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
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
