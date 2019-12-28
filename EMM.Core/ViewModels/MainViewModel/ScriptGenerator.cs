using Data;
using System.Collections.Generic;
using System.Text;

namespace EMM.Core.ViewModels
{
    public class ScriptGenerator
    {
        public ScriptGenerator(ScriptApplyFactory scriptApplyFactory, SettingViewModel setting, IMessageBoxService messageBoxService)
        {
            this.scriptApplyFactory = scriptApplyFactory;
            this.setting = setting;
            this.messageBoxService = messageBoxService;
        }

        private ScriptApplyFactory scriptApplyFactory;
        private SettingViewModel setting;
        private IMessageBoxService messageBoxService;


        /// <summary>
        /// Convert all action to script
        /// </summary>
        /// <param name="macroViewModel">The macro to generate script</param>
        /// <returns></returns>
        public virtual bool? GenerateScript(MacroViewModel macroViewModel)
        {
            var timer = 200;

            if (!ApplyConvertSetting(macroViewModel))
                return false;

            var macroTemplate = macroViewModel.ConvertBack();

            var script = macroTemplate.GenerateScript(ref timer);

            return scriptApplyFactory.GetScriptApplier(setting.SelectedEmulator).ApplyScriptTo(macroTemplate.MacroName, setting.SelectedPath, script);
        }

        /// <summary>
        /// Generate script only selected
        /// </summary>
        /// <param name="actionViewModelList">The selected action viewmodel</param>
        /// <returns></returns>
        public virtual bool? GenerateScript(IList<IActionViewModel> actionViewModelList, MacroViewModel currentMacro)
        {
            StringBuilder script = new StringBuilder();

            var timer = 200;

            if (!ApplyConvertSetting(currentMacro))
                return false;

            foreach (var actionViewModel in actionViewModelList)
            {
                script.Append(actionViewModel.ConvertBackToAction().GenerateAction(ref timer));
            }

            return scriptApplyFactory.GetScriptApplier(setting.SelectedEmulator).ApplyScriptTo(currentMacro.MacroName + "_Test", setting.SelectedPath, script, false);
        }

        private bool ApplyConvertSetting(MacroViewModel macroViewModel)
        {
            GlobalData.CustomX = setting.CustomX;
            GlobalData.CustomY = setting.CustomY;
            GlobalData.Emulator = setting.SelectedEmulator;
            GlobalData.Randomize = setting.Randomize;

            try
            {
                GlobalData.ScaleX = setting.CustomX / macroViewModel.OriginalX;
                GlobalData.ScaleY = setting.CustomY / macroViewModel.OriginalY;
            }
            catch
            {
                messageBoxService.ShowMessageBox("Please set the Macro resolution", "Error", MessageButton.OK, MessageImage.Error);
                return false;
            }

            return true;
        }
    }
}
