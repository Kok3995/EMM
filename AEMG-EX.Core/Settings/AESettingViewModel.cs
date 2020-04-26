using Data;
using EMM.Core;
using EMM.Core.ViewModels;
using MTPExplorer;

namespace AEMG_EX.Core
{
    public class AESettingViewModel : SettingViewModel
    {
        public AESettingViewModel(AESetting aESetting, IMTPManager mTPManager) : base(mTPManager)
        {
            this.aESetting = aESetting;

            Messenger.Register((sender, e) =>
            {
                if (e.EventMessage != EventMessage.SaveSettings)
                    return;

                this.aESetting.Save();
            });

            base.InitializeCommands();
        }

        private AESetting aESetting;

        /// <summary>
        /// true if auto update at start up enable
        /// </summary>
        public override bool IsAutoUpdateEnable
        {
            get => aESetting.IsAutoUpdateEnable;
            set => aESetting.IsAutoUpdateEnable = value;         
        }

        /// <summary>
        /// The location of Nox's record file
        /// </summary>
        public override string NoxScriptLocation
        {
            get => aESetting.NoxScriptLocation;
            set => aESetting.NoxScriptLocation = value;
        }

        /// <summary>
        /// The location to memu script folder
        /// </summary>
        public override string MemuScriptLocation
        {
            get => aESetting.MemuScriptLocation;
            set => aESetting.MemuScriptLocation = value;
        }

        /// <summary>
        /// The location to memu script folder
        /// </summary>
        public override string BlueStackScriptLocation
        {
            get => aESetting.BlueStackScriptLocation;
            set => aESetting.BlueStackScriptLocation = value;
        }

        /// <summary>
        /// Location to LDplayer script folder
        /// </summary>
        public override string LDPlayerScriptLocation
        {
            get => aESetting.LDPlayerScriptLocation;
            set => aESetting.LDPlayerScriptLocation = value;
        }

        /// <summary>
        /// Location to LDplayer script folder
        /// </summary>
        public override string HiroMacroScriptLocation
        {
            get => aESetting.HiroMacroScriptLocation;
            set => aESetting.HiroMacroScriptLocation = value;
        }

        /// <summary>
        /// Location to AnkuLua script folder
        /// </summary>
        public override string AnkuLuaScriptLocation
        {
            get => aESetting.AnkuLuaScriptLocation;
            set => aESetting.AnkuLuaScriptLocation = value;
        }

        /// <summary>
        /// Location to Click Assistant script folder
        /// </summary>
        public override string RobotmonScriptLocation
        {
            get => aESetting.RobotmonScriptLocation;
            set => aESetting.RobotmonScriptLocation = value;
        }

        /// <summary>
        /// Location to AutoTouch script folder
        /// </summary>
        public override string AutoTouchScriptLocation
        {
            get => aESetting.AutoTouchScriptLocation;
            set => aESetting.AutoTouchScriptLocation = value;
        }

        /// <summary>
        /// Number of pixels to randomize
        /// </summary>
        public override int Randomize
        {
            get => aESetting.Randomize;
            set => aESetting.Randomize = value;
        }

        /// <summary>
        /// Custom x resolution
        /// </summary>
        public override int CustomX
        {
            get => aESetting.CustomX;
            set => aESetting.CustomX = value;
        }

        /// <summary>
        /// Custom y resolution
        /// </summary>
        public override int CustomY
        {
            get => aESetting.CustomY;
            set => aESetting.CustomY = value;
        }

        /// <summary>
        /// The selected emulator
        /// </summary>
        public override Emulator SelectedEmulator
        {
            get => aESetting.SelectedEmulator;
            set {
                aESetting.SelectedEmulator = value;
                OnPropertyChanged(nameof(IsNox));
                OnPropertyChanged(nameof(IsMemu));
                OnPropertyChanged(nameof(IsBlueStack));
                OnPropertyChanged(nameof(IsLDPlayer));
                OnPropertyChanged(nameof(IsHiroMacro));
                OnPropertyChanged(nameof(IsAnkuLua));
                OnPropertyChanged(nameof(IsRobotmon));
                OnPropertyChanged(nameof(IsAutoTouch));
            }
        }

        /// <summary>
        /// Scale Mode
        /// </summary>
        public override ScaleMode ScaleMode
        {
            get => aESetting.ScaleMode;
            set
            {
                aESetting.ScaleMode = value;
            }
        }

        /// <summary>
        /// For user to set custom name of the macro when convert
        /// </summary>
        public string CustomName { get; set; }
    }
}
