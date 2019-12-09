using Data;
using EMM.Core.Converter;
using System;
using System.Collections.Generic;
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

        public SettingViewModel()
        {           
        }

        public SettingViewModel(Settings settings, SimpleAutoMapper autoMapper)
        {
            this.autoMapper = autoMapper;
            this.settings = settings;

            this.autoMapper.SimpleAutoMap<Settings, SettingViewModel>(this.settings, this);

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

        public virtual string SelectedPath => (SelectedEmulator == Emulator.Nox) ? NoxScriptLocation : MemuScriptLocation;

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

        #endregion

        #region View Properties

        /// <summary>
        /// List of emulator to choose
        /// </summary>
        public virtual List<Emulator> EmulatorList { get; set; } = Enum.GetValues(typeof(Emulator)).Cast<Emulator>().ToList();

        public virtual bool IsNox => ((SelectedEmulator == Emulator.Nox) ? true : false);

        public virtual ICommand OpenEmulatorFolderCommand { get; set; }

        protected virtual void InitializeCommands()
        {
            OpenEmulatorFolderCommand = new RelayCommand(p =>
            {
                OpenFolderDialog openFolderDialog = new OpenFolderDialog();

                if (openFolderDialog.ShowDialog() == true)
                {
                    switch(SelectedEmulator)
                    {
                        case Emulator.Nox:
                            NoxScriptLocation = openFolderDialog.SelectedPath;
                            break;
                        case Emulator.Memu:
                            MemuScriptLocation = openFolderDialog.SelectedPath;
                            break;
                    }
                }
            });
        }

        #endregion
    }
}
