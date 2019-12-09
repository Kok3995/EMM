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
        public MainWindowViewModel(MacroManagerViewModel macroManager, SettingViewModel setting, IAutoUpdater autoUpdater, TimerToolViewModel timerTool, ScriptGeneratorViewModel scriptGenerator, CustomActionManager customActionManager)
        {
            this.MacroManager = macroManager;
            this.CurrentSettings = setting;
            this.AutoUpdater = autoUpdater;
            this.TimerTool = timerTool;
            this.ScriptGenerator = scriptGenerator;
            this.CustomActionManager = customActionManager;

            InitializeCommands();


            if (CurrentSettings.IsAutoUpdateEnable == true)         
                this.AutoUpdater.CheckForUpdate();
        }

        #endregion

        #region Private member


        #endregion

        #region Public Properties

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

        public ScriptGeneratorViewModel ScriptGenerator { get; set; }

        public CustomActionManager CustomActionManager { get; set; }

        #endregion

        #region Commands
        public ICommand TestClickCommand { get; set; }

        /// <summary>
        /// Initilize all commands in this viewmodel
        /// </summary>
        private void InitializeCommands()
        {           
            TestClickCommand = new RelayCommand(p =>
            {
                //var test = new TestClass();

                //var timer = 200;

                //GlobalData.Emulator = Emulator.MEMU;

                //File.WriteAllText("Test", test.ReturnTestTemplate().GenerateScript(ref timer).ToString());

                //var time = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'hh':'mm':'ss");
                //var str = Uri.EscapeDataString(time);

                var memu = new MemuScriptApply(new MessageBoxService());

                memu.ApplyScriptTo("test", CurrentSettings.MemuScriptLocation, new System.Text.StringBuilder("hello test here"));


            });          
        }

        #endregion

        
    }
}
