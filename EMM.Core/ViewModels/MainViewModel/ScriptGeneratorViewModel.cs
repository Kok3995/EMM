using Data;
using EMM.Core.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EMM.Core.ViewModels
{
    public class ScriptGeneratorViewModel : BaseViewModel
    {
        public ScriptGeneratorViewModel(MacroManagerViewModel macroManager, ScriptGenerator scriptGenerator, IMessageBoxService messageBoxService)
        {
            this.macroManager = macroManager;
            this.scriptGenerator = scriptGenerator;
            this.messageBoxService = messageBoxService;

            InitializeCommands();
        }

        private MacroManagerViewModel macroManager;
        private ScriptGenerator scriptGenerator;
        private IMessageBoxService messageBoxService;

        public ICommand TestSelectedGroupCommand { get; set; }
        public virtual ICommand ConvertCommand { get; set; }
        public ICommand TestSelectedActionCommand { get; set; }

        private void InitializeCommands()
        {
            TestSelectedGroupCommand = new RelayCommand(p =>
            {
                this.scriptGenerator.GenerateScript(this.macroManager.CurrentMacro.ViewModelList.GetSelectedElement(), this.macroManager.CurrentMacro);
            }, p => this.macroManager.CurrentMacro.SelectedItem != null);

            ConvertCommand = new RelayCommand(async p =>
            {
                var result = await Task.Run(() => this.scriptGenerator.GenerateScript(this.macroManager.CurrentMacro));
                if (result == null)
                    return;

                if (result == true)
                    await Task.Run(() => this.messageBoxService.ShowMessageBox("Done", "Convert", MessageButton.OK, MessageImage.Information, MessageResult.OK));
                else
                    await Task.Run(() => this.messageBoxService.ShowMessageBox("Error. Check your setting or contact me for help", "Convert", MessageButton.OK, MessageImage.Error, MessageResult.OK));
            });

            TestSelectedActionCommand = new RelayCommand(p =>
            {
                this.scriptGenerator.GenerateScript((this.macroManager.CurrentMacro.SelectedItem as ActionGroupViewModel).ViewModelList.GetSelectedElement(), this.macroManager.CurrentMacro);
            }, p => (this.macroManager.CurrentMacro.SelectedItem as ActionGroupViewModel)?.SelectedItem != null);
        }
    }
}
