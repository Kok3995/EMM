using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;

namespace EMMUpdater
{
    public class EMMUpdater
    {
        private string defaultUpdateFileLocation = @"https://github.com/Kok3995/EMM/releases/latest/download/EMM.zip";

        private string updateFolder = Path.Combine(Environment.CurrentDirectory, "Update");

        private string updateZipPath = Path.Combine(Environment.CurrentDirectory, "Update", "Update.zip");

        private string updateTempFolder = Path.Combine(Environment.CurrentDirectory, "Update", "Temp");

        private string exePath = Path.Combine(Environment.CurrentDirectory, Environment.GetCommandLineArgs()[0]);
        private string backupPath = Path.Combine(Environment.CurrentDirectory, Environment.GetCommandLineArgs()[0] + ".bak");

        private void Update(string updateUrl)
        {
            Console.WriteLine("Download new version...");

            if (DownloadUpdate(updateUrl) == false)
            {
                Console.WriteLine("Failed to download new version. Please check your internet connection!");
                return;
            }

            Console.WriteLine("Download completed! Apply update...");

            ApplyUpdate();
        }

        private bool DownloadUpdate(string updateUrl)
        {         
            Directory.CreateDirectory(updateFolder);

            using (WebClient wc = new WebClient())
            {
                wc.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.BypassCache);
                try
                {
                    wc.DownloadFile(new Uri(updateUrl), updateZipPath);
                }
                catch
                {
                    return false;
                } //Download fail

                //Download completed
                return true;
            }
        }

        private bool ApplyUpdate()
        {           
            try
            {
                Directory.CreateDirectory(updateTempFolder);

                ZipFile.ExtractToDirectory(updateZipPath, updateTempFolder);

                if (File.Exists(backupPath))
                    File.Delete(backupPath);

                File.Move(exePath, backupPath);

                (new DirectoryInfo(updateTempFolder)).CopyAllTo(new DirectoryInfo(Environment.CurrentDirectory));

                Directory.Delete(updateFolder, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not access the update file. Try to run as an admin\n" + ex.Message);
                Directory.Delete(updateFolder, true);
                return false;
            }

            Console.WriteLine("Update successful!!!");
            return true;
        }

        private void WaitForMainProgramToClose()
        {
            Console.WriteLine("Waiting for EMM and AEMG to close...");
            while (Process.GetProcessesByName("EMM").Any() || Process.GetProcessesByName("AEMG").Any())
            {
                Thread.Sleep(100);
            }
            Console.WriteLine("All Clear!");
        }

        static void Main(string[] args)
        {
            var updater = new EMMUpdater();

            updater.WaitForMainProgramToClose();

            var commandLineArgs = Environment.GetCommandLineArgs();

            if (commandLineArgs.Length > 1)
                updater.Update(commandLineArgs[1]);
            else
            {
                updater.Update(updater.defaultUpdateFileLocation);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }


}
