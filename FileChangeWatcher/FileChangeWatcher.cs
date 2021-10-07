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
        static extern double Shannon(string str);
        [DllImport("CLRFuzzyShannonDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern System.IntPtr computehash(string str);
        [DllImport("CLRFuzzyShannonDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int comparehash(string hash1, string hash2);

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
            //Console.WriteLine($"Changed: {e.FullPath} - time: {DateTime.Now.ToString()}");
            Console.WriteLine($"Changed: {e.Name}");
            this.CheckWork();
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath} - time: {DateTime.Now.ToString()}";
            Console.WriteLine(value);
            this.CheckWork();
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Deleted: {e.FullPath} - time: {DateTime.Now.ToString()}");
            this.CheckWork();
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            string filepath = e.FullPath;
            Console.WriteLine($"Renamed: - time: {DateTime.Now.ToString()}");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
            IntPtr p = computehash(filepath);
            string c = Marshal.PtrToStringAnsi(p);
            Console.WriteLine(c);
            //Marshal.FreeHGlobal(p);
            this.CheckWork();
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
            // Test Code
            //Console.WriteLine("Thread{0}: begin", Thread.CurrentThread.ManagedThreadId);

            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            this.timer = new Timer(this.TimerRun, autoResetEvent, WaitingTime, 0);


            autoResetEvent.WaitOne();
            this.timer.Dispose();

            // Test Code
            //Console.WriteLine("Thread{0}: end", Thread.CurrentThread.ManagedThreadId);
        }

        /// <summary>
        /// 연속된 작업의 끝을 알기 위한 타이머가 끝났을 때 작동하는 메소드
        /// </summary>
        private void TimerRun(Object stateInfo)
        {
            this.isFirstChange = true;
            s1.Run();

            AutoResetEvent autoResetEvent = (AutoResetEvent)stateInfo;
            autoResetEvent.Set();
        }
    }
}
