using System;
using System.IO;
using System.Windows;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace Watcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            string[] pw = new string[16];
            for (int i = 0; i < pw.Length-1; i++)
            {
                byte[] buffer = new byte[4];
                rnd.NextBytes(buffer);
                buffer[0] = 0xfe;
                buffer[1] = 0xff;
                pw[i]= Encoding.Unicode.GetString(buffer);
            }

            StringBuilder builder = new StringBuilder();
            foreach (var item in pw)
                builder.Append(item);
            Console.WriteLine(builder.ToString());
            //var _ = new FileSystemWatcher(Environment.CurrentDirectory);
            //_.Changed += __Changed;
            //_.Deleted += __Deleted;
            //_.Error += __Error;
            //_.Renamed += __Renamed;
            //_.EnableRaisingEvents = true;
            //
            //_.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            //
            Console.ReadLine();
        }

        private static void __Renamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine(e.FullPath + "\t : \t"+ e.ChangeType);
        }

        private static void __Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.GetException()?.Message);
        }

        private static void __Deleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(e.FullPath + "\t : \t" + e.ChangeType);
        }

        private static void __Changed(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(e.FullPath + "\t : \t" + e.ChangeType);
        }
    }

}
