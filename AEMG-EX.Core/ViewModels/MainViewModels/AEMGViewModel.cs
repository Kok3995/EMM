using EMM;
using EMM.Core;
using System.Windows.Input;

namespace AEMG_EX.Core
{
    public class AEMGViewModel : BaseViewModel
    {
        public AEMGViewModel(IMacroManager macroManager, AEActionListViewModel AEActionList, AESettingViewModel Settings, AEScriptGenerator scriptGenerator, IMessageBoxService messageBoxService, IAutoUpdater AutoUpdater)
        {
            this.MacroManager = macroManager;
            this.AEActionListViewModel = AEActionList;
            this.Settings = Settings;
            this.scriptGenerator = scriptGenerator;
            this.messageBoxService = messageBoxService;
            this.AutoUpdater = AutoUpdater;
            this.MacroManager.ScanForMacroes();

            InitializeCommandAndEvents();
        }

        private AEScriptGenerator scriptGenerator;
        private IMessageBoxService messageBoxService;

        public IMacroManager MacroManager { get; set; }

        public AEActionListViewModel AEActionListViewModel { get; set; }

        public AESettingViewModel Settings { get; set; }

        public IAutoUpdater AutoUpdater { get; set; }

        public ICommand ConvertCommand { get; set; }
        public ICommand TestSelectedCommand { get; set; }

        private void InitializeCommandAndEvents()
        {
            ConvertCommand = new RelayCommand(p =>
            {
                var result = this.scriptGenerator.GenerateScript(MacroManager.GetCurrentTemplate(), AEActionListViewModel.AEActionList);

                if (result == null)
                    return;

                if (result == true)
                    this.messageBoxService.ShowMessageBox("Done", "Convert", MessageButton.OK, MessageImage.Information, MessageResult.OK);
                else
                    this.messageBoxService.ShowMessageBox("Error. Check your setting or contact me for help", "Convert", MessageButton.OK, MessageImage.Error, MessageResult.OK);
            });

            TestSelectedCommand = new RelayCommand(p =>
            {
                this.scriptGenerator.GenerateScript(MacroManager.GetCurrentTemplate(), aEAction: AEActionListViewModel.GetSelected());
            });

            MacroManager.SelectChanged += (sender, e) =>
            {
                Settings.CustomName = MacroManager.GetCurrentTemplate()?.MacroName;
            };
        }             
    }
}
