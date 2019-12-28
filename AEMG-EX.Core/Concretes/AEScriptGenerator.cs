using Data;
using EMM.Core;
using EMM.Core.Converter;
using EMM.Core.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace AEMG_EX.Core
{
    public class AEScriptGenerator
    {
        public AEScriptGenerator(ScriptApplyFactory scriptApplyFactory, AESettingViewModel setting, IMessageBoxService messageBoxService, SimpleAutoMapper autoMapper)
        {
            this.scriptApplyFactory = scriptApplyFactory;
            this.setting = setting;
            this.messageBoxService = messageBoxService;
            this.autoMapper = autoMapper;
        }

        private ScriptApplyFactory scriptApplyFactory;

        private AESettingViewModel setting;

        private IMessageBoxService messageBoxService;

        private SimpleAutoMapper autoMapper;

        /// <summary>
        /// Generate script
        /// </summary>
        /// <param name="macro">The macro template</param>
        /// <param name="aEActionList">AEAction list from user choice</param>
        /// <returns></returns>
        public bool? GenerateScript(MacroTemplate macro, IList<IAEActionViewModel> aEActionList, string customName = null)
        { 
            var timer = 200;

            if (!ApplyConvertSetting(macro))
                return false;

            var macroTemplate = this.ConstructCompleteMacro(macro, aEActionList);

            var script = macroTemplate.GenerateScript(ref timer);

            return scriptApplyFactory.GetScriptApplier(setting.SelectedEmulator).ApplyScriptTo(string.IsNullOrEmpty(customName) ? macroTemplate.MacroName : customName, setting.SelectedPath, script);

        }

        private MacroTemplate ConstructCompleteMacro(MacroTemplate template, IList<IAEActionViewModel> aEActionList)
        {
            //Copy the macro so subsequence convert start on fresh macro
            var copiedMacro = this.autoMapper.SimpleAutoMap<MacroTemplate, MacroTemplate>(template);

            copiedMacro.ActionGroupList = copiedMacro.ActionGroupList.Select(ag => (IAction)new ActionGroup { Repeat = (ag as ActionGroup).Repeat, ActionList = new List<IAction>((ag as ActionGroup).ActionList) } ).ToList();

            foreach (var action in aEActionList)
            {
                (copiedMacro.ActionGroupList[action.ActionGroupIndex] as ActionGroup).ActionList.InsertRange(action.ActionIndex, action.UserChoicesToActionList());
            }

            return copiedMacro;
        }

        private bool ApplyConvertSetting(MacroTemplate macro)
        {
            GlobalData.CustomX = setting.CustomX;
            GlobalData.CustomY = setting.CustomY;
            GlobalData.Emulator = setting.SelectedEmulator;
            GlobalData.Randomize = setting.Randomize;

            try
            {
                GlobalData.ScaleX = setting.CustomX / macro.OriginalX;
                GlobalData.ScaleY = setting.CustomY / macro.OriginalY;
            }
            catch
            {
                messageBoxService.ShowMessageBox("The macro doesnt have any resolution set. Please set in in EMM", "Error", MessageButton.OK, MessageImage.Error);
                return false;
            }

            return true;
        }
    }
}
