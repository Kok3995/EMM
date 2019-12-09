using Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using EMM.Core;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Linq;

namespace AEMG_EX.Core
{
    public class AESetting
    {
        public AESetting()
        {
            Load();
        }

        private static readonly string fileName = Path.Combine(Environment.CurrentDirectory, "Setting", "AEMGSettings");

        private static AESetting settings;

        private static object sync = new object();

        #region General settings

        /// <summary>
        /// true if auto update at start up enable
        /// </summary>
        public bool IsAutoUpdateEnable { get; set; } = true;

        /// <summary>
        /// The location of Nox's record file
        /// </summary>
        public string NoxScriptLocation { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Nox", "record");

        /// <summary>
        /// The location to memu script folder
        /// </summary>
        public string MemuScriptLocation { get; set; } = Path.Combine(GetInstallationPath("MEmu"), "scripts");

        #endregion

        #region Convert settings

        /// <summary>
        /// Number of pixels to randomize
        /// </summary>
        public int Randomize { get; set; } = GlobalData.Randomize;

        /// <summary>
        /// Custom x resolution
        /// </summary>
        public int CustomX { get; set; } = GlobalData.CustomX;

        /// <summary>
        /// Custom y resolution
        /// </summary>
        public int CustomY { get; set; } = GlobalData.CustomY;

        /// <summary>
        /// The selected emulator
        /// </summary>
        public Emulator SelectedEmulator { get; set; } = Emulator.Nox;

        #endregion

        #region Methods

        /// <summary>
        /// Persist setting change
        /// </summary>
        public void Save()
        {
            //Check if file exists
            if (!File.Exists(fileName))
            {
                (new FileInfo(fileName)).Directory.Create();
                File.Create(fileName).Close();
            }

            string settingText = JsonConvert.SerializeObject(this, Formatting.Indented);

            File.WriteAllText(fileName, settingText);
        }

        /// <summary>
        /// Load the settings
        /// </summary>
        public void Load()
        {
            if (!File.Exists(fileName))
            {
                Save();
            }

            JObject settingJObject;

            try
            {
                settingJObject = JObject.Parse(File.ReadAllText(fileName));
            }
            catch
            {
                this.RestoreDefaultSettings();
                settingJObject = JObject.Parse(File.ReadAllText(fileName));
            }

            var serializer = new JsonSerializer();
            serializer.Populate(settingJObject.CreateReader(), this);
        }

        /// <summary>
        /// Restore settings to the default value
        /// </summary>
        public void RestoreDefaultSettings()
        {
            if (!File.Exists(fileName))
                return;
            try
            {
                File.Delete(fileName);
            }
            catch
            {
                //TO DO: Add some handler here
            }

            this.Save();
        }

        #endregion

        #region Helpers

        private static string GetInstallationPath(string appName)
        {
            var path64 = $"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{appName}\\";
            var path32 = $"SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{appName}\\";

            List<string> pathFounded = new List<string>();
            string installPath64;
            string installPath32;

            using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (RegistryKey key = hklm.OpenSubKey(path64))
            {
                installPath64 = key?.GetValue("InstallLocation")?.ToString();

                pathFounded.Add(key?.GetValue("DisplayIcon")?.ToString() ?? string.Empty);
            }

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path32))
            {
                installPath32 = key?.GetValue("InstallLocation")?.ToString();

                pathFounded.Add(key?.GetValue("DisplayIcon")?.ToString() ?? string.Empty);
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

        public static AESetting Default()
        {
            lock(sync)
            {
                return settings ?? (settings = new AESetting());
            }          
        }

        #endregion
    }
}
