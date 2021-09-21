using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcher
{
    class ChangeWatcher
    {
        private string path = @"C:\Users\sowoo\Desktop\FileChangeDataset";
        private int count = 0;
        private bool isFirst = true;

        private DateTime startTime;
        private DateTime endTime;

        public void run()
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

        void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            //Console.WriteLine($"Changed: {e.FullPath} - time: {DateTime.Now.ToString()}");
            Console.WriteLine($"Changed: {e.Name}");
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath} - time: {DateTime.Now.ToString()}";
            Console.WriteLine(value);
        }

        private void OnDeleted(object sender, FileSystemEventArgs e) =>
            Console.WriteLine($"Deleted: {e.FullPath} - time: {DateTime.Now.ToString()}");

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed: - time: {DateTime.Now.ToString()}");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
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

        
    }
}
