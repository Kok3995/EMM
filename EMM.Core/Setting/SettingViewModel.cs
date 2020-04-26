using Data;
using EMM.Core.Converter;
using MTPExplorer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using WPF.Dialogs;

namespace EMM.Core.ViewModels
{
    /// <summary>
    /// ViewModel for all the settings in the app
    /// </summary>
    public class SettingViewModel : BaseViewModel, IViewModel
    {
        #region Ctor

        public SettingViewModel(IMTPManager mTPManager)
        {
            this.mTPManager = mTPManager;
        }

        public SettingViewModel(Settings settings, SimpleAutoMapper autoMapper, IMTPManager mTPManager)
        {
            this.autoMapper = autoMapper;
            this.settings = settings;
            this.mTPManager = mTPManager;

            this.autoMapper.SimpleAutoMap(this.settings, this);

            Messenger.Register((sender, e) =>
            {
                if (e.EventMessage != EventMessage.SaveSettings)
                    return;

                this.autoMapper.SimpleAutoMap<SettingViewModel, Settings>(this, this.settings);

                this.settings.Save();
            });

            InitializeCommands();
        }

        #endregion

        #region Private members

        private Settings settings;

        private SimpleAutoMapper autoMapper;

        private IMTPManager mTPManager;

        #endregion

        #region General settings

        /// <summary>
        /// Show group toolbar
        /// </summary>
        public virtual bool IsGroupToolBarVisible { get; set; }

        /// <summary>
        /// Show action toolbar
        /// </summary>
        public virtual bool IsActionToolBarVisible { get; set; }

        /// <summary>
        /// true if auto update at start up enable
        /// </summary>
        public virtual bool IsAutoUpdateEnable { get; set; }

        /// <summary>
        /// The location of Nox's record file
        /// </summary>
        public virtual string NoxScriptLocation { get; set; }

        /// <summary>
        /// The location to memu script folder
        /// </summary>
        public virtual string MemuScriptLocation { get; set; }

        /// <summary>
        /// Location to bluestack script folder
        /// </summary>
        public virtual string BlueStackScriptLocation { get; set; }

        /// <summary>
        /// Location to LDplayer script folder
        /// </summary>
        public virtual string LDPlayerScriptLocation { get; set; }

        /// <summary>
        /// Location to Hiro script folder
        /// </summary>
        public virtual string HiroMacroScriptLocation { get; set; }

        /// <summary>
        /// Location to AnkuLua script folder
        /// </summary>
        public virtual string AnkuLuaScriptLocation { get; set; }

        /// <summary>
        /// Location to Click Assistant script folder
        /// </summary>
        public virtual string RobotmonScriptLocation { get; set; }

        /// <summary>
        /// Location to AutoTouch script folder
        /// </summary>
        public virtual string AutoTouchScriptLocation { get; set; }

        #endregion

        #region Convert settings

        /// <summary>
        /// Number of pixels to randomize
        /// </summary>
        public virtual int Randomize { get; set; }

        /// <summary>
        /// Custom x resolution
        /// </summary>
        public virtual int CustomX { get; set; }

        /// <summary>
        /// Custom y resolution
        /// </summary>
        public virtual int CustomY { get; set; }

        /// <summary>
        /// The selected emulator
        /// </summary>
        public virtual Emulator SelectedEmulator { get; set; }

        /// <summary>
        /// Scale Mode
        /// </summary>
        public virtual ScaleMode ScaleMode { get; set; } = ScaleMode.Stretch;

        #endregion

        #region View Properties

        /// <summary>
        /// List of emulator to choose
        /// </summary>
        public virtual List<Emulator> EmulatorList { get; set; } = Enum.GetValues(typeof(Emulator)).Cast<Emulator>().ToList();

        /// <summary>
        /// List of scale mode available
        /// </summary>
        public virtual List<ScaleMode> ScaleModeList { get; set; } = Enum.GetValues(typeof(ScaleMode)).Cast<ScaleMode>().ToList();

        public virtual bool IsNox => ((SelectedEmulator == Emulator.Nox) ? true : false);
        public virtual bool IsMemu => ((SelectedEmulator == Emulator.Memu) ? true : false);
        public virtual bool IsBlueStack => ((SelectedEmulator == Emulator.BlueStack) ? true : false);
        public virtual bool IsLDPlayer => ((SelectedEmulator == Emulator.LDPlayer) ? true : false);
        public virtual bool IsHiroMacro => ((SelectedEmulator == Emulator.HiroMacro) ? true : false);
        public virtual bool IsAnkuLua => ((SelectedEmulator == Emulator.AnkuLua) ? true : false);
        public virtual bool IsRobotmon => ((SelectedEmulator == Emulator.Robotmon) ? true : false);
        public virtual bool IsAutoTouch => ((SelectedEmulator == Emulator.AutoTouch) ? true : false);

        public virtual string SelectedPath =>
            (SelectedEmulator == Emulator.Nox) ? NoxScriptLocation :
            (SelectedEmulator == Emulator.Memu) ? MemuScriptLocation :
            (SelectedEmulator == Emulator.BlueStack) ? BlueStackScriptLocation :
            (SelectedEmulator == Emulator.LDPlayer) ? LDPlayerScriptLocation :
            (SelectedEmulator == Emulator.HiroMacro) ? HiroMacroScriptLocation :
            (SelectedEmulator == Emulator.AnkuLua) ? AnkuLuaScriptLocation :
            (SelectedEmulator == Emulator.Robotmon) ? RobotmonScriptLocation :
            AutoTouchScriptLocation;

        public virtual ICommand OpenEmulatorFolderCommand { get; set; }
        public ICommand OpenFolderCommand { get; set; }
        public ICommand OpenMTPFolderCommand { get; set; }

        protected virtual void InitializeCommands()
        {
            OpenEmulatorFolderCommand = new RelayCommand(p =>
            {
                OpenFolderDialog openFolderDialog = new OpenFolderDialog();

                if (openFolderDialog.ShowDialog() == true)
                {
                    SetSelectedPath(openFolderDialog.SelectedPath);
                }
            });

            OpenFolderCommand = new RelayCommand(p =>
            {
                try
                {
                    var path = Path.GetFullPath(SelectedPath);
                    Process.Start(path);
                }
                catch
                {
                    Process.Start(Environment.CurrentDirectory);
                }
            });

            OpenMTPFolderCommand = new RelayCommand(p =>
            {
                var mtp = mTPManager.Open(SelectedPath);
                if (mtp.Result == true)
                {
                    SetSelectedPath(mtp.SelectedPath);
                }
            });
        }

        #endregion

        #region Helpers

        private void SetSelectedPath(string path)
        {
            switch (SelectedEmulator)
            {
                case Emulator.Nox:
                    NoxScriptLocation = path;
                    break;
                case Emulator.Memu:
                    MemuScriptLocation = path;
                    break;
                case Emulator.BlueStack:
                    BlueStackScriptLocation = path;
                    break;
                case Emulator.LDPlayer:
                    LDPlayerScriptLocation = path;
                    break;
                case Emulator.HiroMacro:
                    HiroMacroScriptLocation = path;
                    break;
                case Emulator.AnkuLua:
                    AnkuLuaScriptLocation = path;
                    break;
                case Emulator.Robotmon:
                    RobotmonScriptLocation = path;
                    break;
                case Emulator.AutoTouch:
                    AutoTouchScriptLocation = path;
                    break;
            }
        }

        #endregion
    }
}
