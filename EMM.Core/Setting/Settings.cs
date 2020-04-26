using Data;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using EMM.Core.Extension;

namespace EMM.Core
{
    /// <summary>
    /// This class is used to store app's settings
    /// </summary>
    public class Settings
    {
        public Settings()
        {
            Load();
        }

        private static readonly string fileName = Path.Combine(Environment.CurrentDirectory,"Setting","EMMSettings");

        private static Settings settings;

        #region General settings

        /// <summary>
        /// Show group toolbar
        /// </summary>
        public bool IsGroupToolBarVisible { get; set; } = true;

        /// <summary>
        /// Show action toolbar
        /// </summary>
        public bool IsActionToolBarVisible { get; set; } = true;

        /// <summary>
        /// true if auto update at start up enable
        /// </summary>
        public bool IsAutoUpdateEnable { get; set; } = true;

        /// <summary>
        /// The location of Nox's record file
        /// </summary>
        public string NoxScriptLocation { get; set; } = StaticVariables.DEFAULT_NOX_FOLDER;

        /// <summary>
        /// The location to memu script folder
        /// </summary>
        public string MemuScriptLocation { get; set; } = StaticVariables.DEFAULT_MEMU_FOLDER;

        /// <summary>
        /// Location to bluestack script folder
        /// </summary>
        public string BlueStackScriptLocation { get; set; } = StaticVariables.DEFAULT_BLUESTACKS_FOLDER;

        /// <summary>
        /// Location to LDplayer script folder
        /// </summary>
        public string LDPlayerScriptLocation { get; set; } = StaticVariables.DEFAULT_LDPlayer_FOLDER;

        /// <summary>
        /// Location to Hiro script folder
        /// </summary>
        public string HiroMacroScriptLocation { get; set; } = StaticVariables.DEFAULT_MOBILE_FOLDER;

        /// <summary>
        /// Location to AnkuLua script folder
        /// </summary>
        public string AnkuLuaScriptLocation { get; set; } = StaticVariables.DEFAULT_MOBILE_FOLDER;

        /// <summary>
        /// Location to Robotmon script folder
        /// </summary>
        public string RobotmonScriptLocation { get; set; } = StaticVariables.DEFAULT_MOBILE_FOLDER;

        /// <summary>
        /// Location to AutoTouch script folder
        /// </summary>
        public string AutoTouchScriptLocation { get; set; } = StaticVariables.DEFAULT_MOBILE_FOLDER;

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

        /// <summary>
        /// Scale Mode
        /// </summary>
        public ScaleMode ScaleMode { get; set; } = ScaleMode.Stretch;

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
            } catch
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.Save();
        }

        #endregion

        #region Helpers

        public static Settings Default()
        {
            return settings ?? (settings = new Settings());
        }

        #endregion
    }
}
