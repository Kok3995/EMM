using EMM.Core.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EMM.Core.Tools
{
    public class ResolutionConverterToolViewModel : BaseViewModel
    {
        public ResolutionConverterToolViewModel(ResolutionConverterTool converterTool,  MacroManagerViewModel macroManager, IMessageBoxService messageBoxService)
        {
            this.converterTool = converterTool;
            this.macroManager = macroManager;
            this.messageBoxService = messageBoxService;

            InitilizeCommandsAndEvents();
        }

        private ResolutionConverterTool converterTool;
        private MacroManagerViewModel macroManager;
        private IMessageBoxService messageBoxService;
        private int convertX;
        private int convertY;
        private MacroViewModel backup;

        public string CurrentResolution => (macroManager.CurrentMacro != null) ? string.Format("{0}x{1}", macroManager.CurrentMacro?.OriginalX, macroManager.CurrentMacro?.OriginalY) : "No Macro Open";

        public int ConvertX
        {
            get => convertX;
            set
            {
                convertX = value;
                this.OnPropertyChanged(nameof(ScaleX));
            }
        }

        public double ScaleX => (macroManager.CurrentMacro?.OriginalX > 0) ?  Math.Round((double)ConvertX / macroManager.CurrentMacro.OriginalX, 2) : 0.00;

        public int ConvertY
        {
            get => convertY;
            set
            {
                convertY = value;
                this.OnPropertyChanged(nameof(ScaleY));
            }
        }

        public double ScaleY => (macroManager.CurrentMacro?.OriginalY > 0) ? Math.Round((double)ConvertY / macroManager.CurrentMacro.OriginalY, 2) : 0.00;

        public ObservableCollection<MidpointRounding> RoundModeOptions { get; set; } = new ObservableCollection<MidpointRounding>(Enum.GetValues(typeof(MidpointRounding)).Cast<MidpointRounding>());

        public MidpointRounding SelectedMode { get; set; }

        public ICommand OpenResolutionConverterToolCommand { get; set; }
        public ICommand ConvertResolutionCommand { get; set; }
        public ICommand UndoConvertResolutionCommand { get; set; }

        //public bool IsResolutionConverterToolOpen { get; set; }

        private void InitilizeCommandsAndEvents()
        {
            OpenResolutionConverterToolCommand = new RelayCommand(p =>
            {
                Messenger.Send(this, new ToolEventArgs(ToolMessage.OpenConverter));                    
            });

            ConvertResolutionCommand = new RelayCommand(p =>
            {
                if (ConvertX <= 0 || ConvertY <= 0)
                {
                    this.messageBoxService.ShowMessageBox("Please input a valid positive number for the new resolution", "ERROR", MessageButton.OK, MessageImage.Error);
                    return; 
                }

                if (this.macroManager.CurrentMacro == null)
                {
                    this.messageBoxService.ShowMessageBox("There's no macro opened", "ERROR", MessageButton.OK, MessageImage.Error);
                    return;
                }

                var newMacro = this.converterTool.ConvertResolution(macroManager.CurrentMacro, ConvertX, ConvertY, SelectedMode);
                
                if (newMacro == null)
                {
                    this.messageBoxService.ShowMessageBox("Can not convert the macro", "ERROR", MessageButton.OK, MessageImage.Error);
                    return;
                }
               
                newMacro.OriginalX = ConvertX;
                newMacro.OriginalY = ConvertY;
                this.backup = this.macroManager.GetCurrentMacro();
                this.macroManager.SetCurrentMacro(newMacro);
            }, p => ConvertX > 0 && ConvertY > 0);

            UndoConvertResolutionCommand = new RelayCommand(p =>
            {
                this.macroManager.SetCurrentMacro(this.backup);
                this.backup = null;
            }, p => this.backup != null);

            this.macroManager.CurrentMacroChanged += (sender, e) =>
            {
                this.OnPropertyChanged(nameof(CurrentResolution));
                HookResolutionChangeEvent(this.macroManager.GetCurrentMacro());
                OnPropertyChanged(nameof(ScaleX));
                OnPropertyChanged(nameof(ScaleY));
            };
        } 

        private void HookResolutionChangeEvent(MacroViewModel macro)
        {
            if (macro != null)
            {
                macro.ResolutionChanged += (sender, e) =>
                {
                    OnPropertyChanged(nameof(CurrentResolution));
                };
            }
        }
    }
}
