using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul16PR
{
    
        class FileMonitor
        {
            static FileSystemWatcher watcher;
            static string logFilePath;

            static void Main()
            {
                Console.WriteLine("File Monitor Application");
                ConfigureMonitoring();
                StartMonitoring();
            }

            static void ConfigureMonitoring()
            {
                Console.Write("Enter the path of directory to monitor: ");
                string directoryPath = Console.ReadLine();
                Console.Write("Enter the path for the log file: ");
                logFilePath = Console.ReadLine();

                watcher = new FileSystemWatcher(directoryPath)
                {
                    NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Size
                                 | NotifyFilters.Security
                };

                watcher.Changed += OnChanged;
                watcher.Created += OnCreated;
                watcher.Deleted += OnDeleted;
                watcher.Renamed += OnRenamed;
                watcher.Error += OnError;
            }

            static void StartMonitoring()
            {
                watcher.EnableRaisingEvents = true;
                Console.WriteLine("Monitoring started. Press 'q' to quit.");
                while (Console.Read() != 'q') ;
            }

            static void OnChanged(object sender, FileSystemEventArgs e)
            {
                LogChange($"Changed: {e.FullPath}");
            }

            static void OnCreated(object sender, FileSystemEventArgs e)
            {
                LogChange($"Created: {e.FullPath}");
            }

            static void OnDeleted(object sender, FileSystemEventArgs e)
            {
                LogChange($"Deleted: {e.FullPath}");
            }

            static void OnRenamed(object sender, RenamedEventArgs e)
            {
                LogChange($"Renamed: {e.OldFullPath} to {e.FullPath}");
            }

            static void OnError(object sender, ErrorEventArgs e)
            {
                LogChange($"Error: {e.GetException().Message}");
            }

            static void LogChange(string message)
            {
                try
                {
                    File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Logging failed: {ex.Message}");
                }
            }
        }
    }
}
