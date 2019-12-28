using Data;
using EMM.Core.Converter;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using System;
using Newtonsoft.Json;
using EMM.Core.Update;
using EMM.Core.Tools;
using EMM.Core.Service;
using System.Reflection;

namespace EMM.Core.ViewModels
{
    /// <summary>
    /// ViewModel for the MainWindow
    /// </summary>
    public class MainWindowViewModel : BaseViewModel
    {
        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainWindowViewModel(MacroManagerViewModel macroManager, SettingViewModel setting, IAutoUpdater autoUpdater, TimerToolViewModel timerTool, ResolutionConverterToolViewModel resolutionConverterTool, ScriptGeneratorViewModel scriptGenerator, CustomActionManager customActionManager, AutoLocationViewModel AutoLocation)
        {
            this.MacroManager = macroManager;
            this.CurrentSettings = setting;
            this.AutoUpdater = autoUpdater;
            this.TimerTool = timerTool;
            this.ScriptGenerator = scriptGenerator;
            this.CustomActionManager = customActionManager;
            this.ResolutionConverterTool = resolutionConverterTool;
            this.AutoLocation = AutoLocation;

            InitializeCommands();


            if (CurrentSettings.IsAutoUpdateEnable == true)         
                this.AutoUpdater.CheckForUpdate();
        }

        #endregion

        #region Private member


        #endregion

        #region Public Properties

        public string TitleWithVersion => string.Format("Easy Macro Maker {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString(3));

        /// <summary>
        /// Managed loaded macro
        /// </summary>
        public MacroManagerViewModel MacroManager { get; set; }

        /// <summary>
        /// Current settings
        /// </summary>
        public SettingViewModel CurrentSettings { get; set; }

        /// <summary>
        /// Auto update the program
        /// </summary>
        public IAutoUpdater AutoUpdater { get; set; }

        /// <summary>
        /// The timer tool
        /// </summary>
        public TimerToolViewModel TimerTool { get; set; }

        public ResolutionConverterToolViewModel ResolutionConverterTool { get; set; }

        public ScriptGeneratorViewModel ScriptGenerator { get; set; }

        public CustomActionManager CustomActionManager { get; set; }

        public AutoLocationViewModel AutoLocation { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Initilize all commands in this viewmodel
        /// </summary>
        private void InitializeCommands()
        {           
      
        }

        #endregion

        
    }
}
