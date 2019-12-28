using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;

namespace EMM.Core.Extension
{
    public static class UsefulStaticMethod
    {
        public static string GetInstallationPath(string appName)
        {
            var path64 = $"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{appName}\\";
            var path32 = $"SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{appName}\\";

            List<string> pathFounded = new List<string>();
            string installPath64;
            string installPath32;

            using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (RegistryKey key = hklm.OpenSubKey(path64))
            {
                installPath64 = key?.GetValue("InstallLocation")?.ToString()?.NullTerminateString();

                pathFounded.Add(key?.GetValue("DisplayIcon")?.ToString()?.NullTerminateString() ?? string.Empty);
            }

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path32))
            {
                installPath32 = key?.GetValue("InstallLocation")?.ToString()?.NullTerminateString();

                pathFounded.Add(key?.GetValue("DisplayIcon")?.ToString()?.NullTerminateString() ?? string.Empty);
            }

            if (!string.IsNullOrEmpty(installPath64))
                return installPath64;

            if (!string.IsNullOrEmpty(installPath32))
                return installPath32;

            return GetFileDirectory(pathFounded.FirstOrDefault(i => !string.IsNullOrEmpty(i)));
        }


        /// <summary>
        /// Get file and folders directory
        /// </summary>
        /// <param name="directories">full path of file or folder you want the name of directory</param>
        /// <returns></returns>
        public static string GetFileDirectory(string directories)
        {
            if (string.IsNullOrEmpty(directories))
                return string.Empty;

            string uniPath = directories.Replace('/', '\\').Replace("\"", string.Empty);

            int lastIndex = uniPath.LastIndexOf('\\');

            if (lastIndex <= 0)
                return directories;

            return uniPath.Substring(0, lastIndex);
        }
    }
}
