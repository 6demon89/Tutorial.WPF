using System;
using System.Windows;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Xml.Serialization;
using System.Reflection.Metadata;
using System.IO;
using System.Data;

namespace Watcher
{
    class Program
    {
        static LocalFileData localData;
        static void Main(string[] args)
        {
            localData = new LocalFileData();
            FileSystemWatcher watcher = new FileSystemWatcher(localData.GetProcessFolderPath());
            watcher.EnableRaisingEvents = true;
            watcher.Created += Watcher_Created;
            watcher.Deleted += Watcher_Deleted;
            watcher.Renamed += Watcher_Renamed;
            watcher.Changed += Watcher_Changed;
            watcher.Error += Watcher_Error;

            watcher.NotifyFilter = NotifyFilters.LastWrite;

            Console.WriteLine($"Watching {watcher.Path} Folder");
            Console.ReadLine();
        }

        private static void Watcher_Error(object sender, ErrorEventArgs e) => Console.WriteLine($"Error Happened : {e.GetException().Message}");

        private async static void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                var reader = new FileProcessFactory().GetFileReader(new FileInfo(e.FullPath));
                Console.WriteLine(await reader.ReadFileAsync());
                if (new FileInfo(e.FullPath).Exists)
                    File.Delete(e.FullPath);
            }
            catch (NotImplementedException ex)
            {
                if (new FileInfo(e.FullPath).Exists)
                    File.Move(e.FullPath, Path.Combine(localData.GetFailFolderPath(), e.Name), true);
            }
            catch
            {
                Console.WriteLine("Can not access a file, since it is beild used by another process");
            }
            Console.WriteLine($"{e.Name} {e.ChangeType}");
        }

        private static void Watcher_Renamed(object sender, RenamedEventArgs e) => Console.WriteLine($"{e.Name} {e.ChangeType}");

        private static void Watcher_Deleted(object sender, FileSystemEventArgs e) => Console.WriteLine($"{e.Name} {e.ChangeType}");

        private static void Watcher_Created(object sender, FileSystemEventArgs e) => Console.WriteLine($"{e.Name} {e.ChangeType}");

    }
}
