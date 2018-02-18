using System;
using System.IO;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace Pick_Hack_lab1
{
    class Program
    {
        const string HackPath = "C:\\Users\\Public\\ForHacker";
        private static string PrivateHackPath = Path.Combine(Directory.GetCurrentDirectory(), "Hack");

        private static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            
            return false;
        }

        private static async void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);

            if (e.ChangeType == WatcherChangeTypes.Created)
                return;

            int k = 0;
            while (k <= 10)
            {
                if (!IsFileLocked(new FileInfo(e.FullPath)))
                    break;

                k++;
                await Task.Delay(1000);
            }

            if (k == 10)
                return;

            string data = File.ReadAllText(e.FullPath);

            if (!Directory.Exists(PrivateHackPath))
                Directory.CreateDirectory(PrivateHackPath);

            File.WriteAllText(Path.Combine(PrivateHackPath, e.Name), data);

            Console.WriteLine("Data in private directory.");
        }

        static void Main(string[] args)
        {
            Observe();

            Console.WriteLine("Enter \'Close\' for stop watching");
            while (Console.ReadLine() != "Close") ;
        }

        [PermissionSet(SecurityAction.Demand)]
        public static void Observe()
        {
            FileSystemWatcher myWatcher = new FileSystemWatcher(HackPath)
            {
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.txt"
            };

            myWatcher.Changed += new FileSystemEventHandler(OnChanged);
            myWatcher.Created += new FileSystemEventHandler(OnChanged);

            myWatcher.EnableRaisingEvents = true;
        }
    }
}
