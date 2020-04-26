using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMM.Core.ViewModels
{
    public class ScriptGenerator
    {
        public ScriptGenerator(ScriptApplyFactory scriptApplyFactory, SettingViewModel setting, IMessageBoxService messageBoxService, IEmulatorToScriptFactory emulatorToScriptFactory, IActionToScriptFactory actionToScriptFactory)
        {
            this.scriptApplyFactory = scriptApplyFactory;
            this.setting = setting;
            this.messageBoxService = messageBoxService;
            this.emulatorToScriptFactory = emulatorToScriptFactory;
            this.actionToScriptFactory = actionToScriptFactory;
        }

        private ScriptApplyFactory scriptApplyFactory;
        private SettingViewModel setting;
        private IMessageBoxService messageBoxService;
        private IEmulatorToScriptFactory emulatorToScriptFactory;
        private IActionToScriptFactory actionToScriptFactory;


        /// <summary>
        /// Convert all action to script
        /// </summary>
        /// <param name="macroViewModel">The macro to generate script</param>
        /// <returns></returns>
        public virtual bool? GenerateScript(MacroViewModel macroViewModel)
        {
            if (!ApplyConvertSetting(macroViewModel))
                return false;

            var macroTemplate = macroViewModel.ConvertBack();

            var script = this.emulatorToScriptFactory.GetEmulatorScriptGenerator(setting.SelectedEmulator).MacroToScript(macroTemplate);

            return scriptApplyFactory.GetScriptApplier(setting.SelectedEmulator).ApplyScriptTo(macroTemplate.MacroName, setting.SelectedPath, script);
        }

        /// <summary>
        /// Generate script only selected
        /// </summary>
        /// <param name="actionViewModelList">The selected action viewmodel</param>
        /// <returns></returns>
        public virtual bool? GenerateScript(IList<IActionViewModel> actionViewModelList, MacroViewModel currentMacro)
        {
            if (!ApplyConvertSetting(currentMacro))
                return false;

            var script = this.emulatorToScriptFactory.GetEmulatorScriptGenerator(setting.SelectedEmulator).MacroToScript(actionViewModelList.Select(a => a.ConvertBackToAction()).ToList());

            return scriptApplyFactory.GetScriptApplier(setting.SelectedEmulator).ApplyScriptTo(currentMacro.MacroName + "_Test", setting.SelectedPath, script, false);
        }

        private bool ApplyConvertSetting(MacroViewModel macro)
        {
            if (macro.OriginalX == 0 || double.IsNaN(macro.OriginalX) || macro.OriginalY == 0 || double.IsNaN(macro.OriginalY))
            {
                messageBoxService.ShowMessageBox("The macro doesnt have any resolution set. Please set in in EMM", "Error", MessageButton.OK, MessageImage.Error);
                return false;
            }

            GlobalData.CustomX = setting.CustomX;
            GlobalData.CustomY = setting.CustomY;
            GlobalData.OriginalX = macro.OriginalX;
            GlobalData.OriginalY = macro.OriginalY;
            GlobalData.Emulator = setting.SelectedEmulator;
            GlobalData.Randomize = setting.Randomize;
            GlobalData.ScaleMode = setting.ScaleMode;

            return true;
        }
    }
}
