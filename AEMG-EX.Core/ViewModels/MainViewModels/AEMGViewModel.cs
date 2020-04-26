using EMM;
using EMM.Core;
using System;
using System.IO;
using System.Windows.Input;

namespace AEMG_EX.Core
{
    public class AEMGViewModel : BaseViewModel
    {
        public AEMGViewModel(AEMacroManagerViewModel macroManagerViewModel, AEActionListViewModel AEActionList, AESettingViewModel Settings, AEScriptGenerator scriptGenerator, IMessageBoxService messageBoxService, IAutoUpdater AutoUpdater, AEMacroSaveManagerViewModel saveManager, IMacroManager macroManager)
        {
            this.MacroManager = macroManagerViewModel;
            this.AEActionListViewModel = AEActionList;
            this.Settings = Settings;
            this.scriptGenerator = scriptGenerator;
            this.messageBoxService = messageBoxService;
            this.AutoUpdater = AutoUpdater;
            this.SaveManager = saveManager;
            this.macroManager = macroManager;
            this.MacroManager.ScanForMacroes();

            InitializeCommandAndEvents();

            if (Settings.IsAutoUpdateEnable == true)
                this.AutoUpdater.CheckForUpdate();
        }

        private AEScriptGenerator scriptGenerator;
        private IMessageBoxService messageBoxService;
        private IMacroManager macroManager;

        public AEMacroManagerViewModel MacroManager { get; set; }

        public AEActionListViewModel AEActionListViewModel { get; set; }

        public AESettingViewModel Settings { get; set; }

        public IAutoUpdater AutoUpdater { get; set; }

        public AEMacroSaveManagerViewModel SaveManager { get; set; }

        public ICommand ConvertCommand { get; set; }
        public ICommand TestSelectedCommand { get; set; }
        public ICommand PreviewInEMMCommand { get; set; }

        private void InitializeCommandAndEvents()
        {
            ConvertCommand = new RelayCommand(p =>
            {
                var result = this.scriptGenerator.GenerateScript(macroManager.GetCurrentTemplate(), AEActionListViewModel.AEActionList);

                if (result == null)
                    return;

                if (result == true)
                    this.messageBoxService.ShowMessageBox("Done", "Convert", MessageButton.OK, MessageImage.Information, MessageResult.OK);
                else
                    this.messageBoxService.ShowMessageBox("Error. Check your setting or contact me for help", "Convert", MessageButton.OK, MessageImage.Error, MessageResult.OK);
            });

            TestSelectedCommand = new RelayCommand(p =>
            {
                this.scriptGenerator.GenerateScript(macroManager.GetCurrentTemplate(), aEAction: AEActionListViewModel.GetSelected());
            });

            PreviewInEMMCommand = new RelayCommand(p =>
            {
                //check if EMM exist
                if (!File.Exists(Path.Combine(Environment.CurrentDirectory, AEMGStatic.EMM_NAME)))
                {
                    this.messageBoxService.ShowMessageBox("Cannot find EMM application. Put this application to the same folder as EMM.exe", "ERROR", MessageButton.OK, MessageImage.Error);
                    return;
                }

                var path = this.scriptGenerator.GenerateTempScript(macroManager.GetCurrentTemplate(), AEActionListViewModel.AEActionList);

                AEMGHelpers.StartEMM(path, StaticVariables.NO_SAVE_AGRS);
            });

            macroManager.SelectChanged += (sender, e) =>
            {
                Settings.CustomName = e.NewMacro?.MacroName;
            };
        }             
    }
}
