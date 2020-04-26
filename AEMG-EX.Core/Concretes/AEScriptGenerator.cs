using Data;
using EMM.Core;
using EMM.Core.Converter;
using EMM.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AEMG_EX.Core
{
    public class AEScriptGenerator
    {
        public AEScriptGenerator(ScriptApplyFactory scriptApplyFactory, AESettingViewModel setting, IMessageBoxService messageBoxService, SimpleAutoMapper autoMapper, IEmulatorToScriptFactory emulatorToScriptFactory, IActionToScriptFactory actionToScriptFactory, DataIO dataIO, IPredefinedActionProvider actionProvider)
        {
            this.scriptApplyFactory = scriptApplyFactory;
            this.setting = setting;
            this.messageBoxService = messageBoxService;
            this.autoMapper = autoMapper;
            this.emulatorToScriptFactory = emulatorToScriptFactory;
            this.actionToScriptFactory = actionToScriptFactory;
            this.dataIO = dataIO;
            this.actionProvider = actionProvider;
        }

        private ScriptApplyFactory scriptApplyFactory;

        private AESettingViewModel setting;

        private IMessageBoxService messageBoxService;

        private SimpleAutoMapper autoMapper;
        private IEmulatorToScriptFactory emulatorToScriptFactory;
        private IActionToScriptFactory actionToScriptFactory;
        private DataIO dataIO;
        private IPredefinedActionProvider actionProvider;

        /// <summary>
        /// Generate script
        /// </summary>
        /// <param name="macro">The macro template</param>
        /// <param name="aEActionList">AEAction list from user choice</param>
        /// <returns></returns>
        public bool? GenerateScript(MacroTemplate macro, IList<IAEActionViewModel> aEActionList)
        {
            if (!ApplyConvertSetting(macro))
                return false;

            var macroTemplate = this.ConstructCompleteMacro(macro, aEActionList);

            var script = this.emulatorToScriptFactory.GetEmulatorScriptGenerator(setting.SelectedEmulator).MacroToScript(macroTemplate);

            return scriptApplyFactory.GetScriptApplier(setting.SelectedEmulator).ApplyScriptTo(string.IsNullOrEmpty(setting.CustomName) ? macroTemplate.MacroName : setting.CustomName, setting.SelectedPath, script);

        }

        /// <summary>
        /// Generate script for the selected AEaction only
        /// </summary>
        /// <param name="macro">The macro template</param>
        /// <param name="aEActionList">the aeaction to generate script</param>
        /// <returns></returns>
        public bool? GenerateScript(MacroTemplate macro, IAEActionViewModel aEAction)
        {
            if (!ApplyConvertSetting(macro))
                return false;

            var actionList = ScaleActionsToMacroResolution(macro, aEAction.UserChoicesToActionList());

            var script = this.emulatorToScriptFactory.GetEmulatorScriptGenerator(setting.SelectedEmulator).MacroToScript(actionList);

            return scriptApplyFactory.GetScriptApplier(setting.SelectedEmulator).ApplyScriptTo(string.IsNullOrEmpty(setting.CustomName) ? macro.MacroName + "_Test" : setting.CustomName + "_Test", setting.SelectedPath, script, false);
        }

        /// <summary>
        /// Contruct the complete macro base on user selection
        /// </summary>
        /// <param name="template"></param>
        /// <param name="aEActionList"></param>
        /// <returns></returns>
        public MacroTemplate ConstructCompleteMacro(MacroTemplate template, IList<IAEActionViewModel> aEActionList)
        {
            //Copy the macro so subsequence convert start on fresh macro
            var copiedMacro = CopyMacro(template);

            var offset = 0;
            var lastActionGroupIndex = -1;

            foreach (var action in aEActionList)
            {
                var actionList = ScaleActionsToMacroResolution(template, action.UserChoicesToActionList());
                var actionIndexToInsert = action.ActionIndex;

                //Fix index to insert if placeholder in the same actiongroup
                if (action.ActionGroupIndex == lastActionGroupIndex)
                {
                    actionIndexToInsert += offset;
                    offset += actionList.Count;
                }
                else
                {
                    offset = actionList.Count;
                }

                (copiedMacro.ActionGroupList[action.ActionGroupIndex] as ActionGroup).ActionList.InsertRange(actionIndexToInsert, actionList);

                lastActionGroupIndex = action.ActionGroupIndex;
            }

            return copiedMacro;
        }

        /// <summary>
        /// Generate a temporary complete macro, return the path to the macro if success, null otherwise
        /// </summary>
        /// <param name="macro"></param>
        /// <param name="aEActionList"></param>
        /// <returns></returns>
        public string GenerateTempScript(MacroTemplate macro, IList<IAEActionViewModel> aEActionList)
        {
            try
            {
                if (!ApplyConvertSetting(macro))
                    return null;

                var macroTemplate = this.ConstructCompleteMacro(macro, aEActionList);

                return this.dataIO.SaveToFile(macroTemplate, Path.Combine(StaticVariables.TEMP_FOLDER, macro.MacroName + ".emm"));
            }
            catch
            {
                return null;
            }

        }

        private bool ApplyConvertSetting(MacroTemplate macro)
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

        /// <summary>
        /// Scale the default action to macro resolution
        /// </summary>
        /// <param name="macro"></param>
        /// <param name="actionList"></param>
        /// <returns></returns>
        private IList<IAction> ScaleActionsToMacroResolution(MacroTemplate macro, IList<IAction> actionList)
        {
            var newList = new List<IAction>();
            //double scaleX = macro.OriginalX / 1280.0;
            //double scaleY = macro.OriginalY / 720.0;
            GlobalData.CustomX = macro.OriginalX;
            GlobalData.CustomY = macro.OriginalY;
            GlobalData.OriginalX = actionProvider.GetDefaultActions().X;
            GlobalData.OriginalY = actionProvider.GetDefaultActions().Y;
            GlobalData.Emulator = Emulator.Nox;

            foreach (var action in actionList)
            {
                switch (action.BasicAction)
                {
                    case BasicAction.Click:
                        var copied = this.autoMapper.SimpleAutoMap<Click, Click>(action as Click);
                        //copied.ClickPoint = new System.Windows.Point(Math.Round((action as Click).ClickPoint.X * scaleX), Math.Round((action as Click).ClickPoint.Y * scaleY));
                        copied.Scale();
                        newList.Add(copied);
                        break;
                    case BasicAction.Swipe:
                        var copiedSwipe = this.autoMapper.SimpleAutoMap<Swipe, Swipe>(action as Swipe);
                        //copiedSwipe.PointList = copiedSwipe.PointList.Select(sp => new SwipePoint(sp)).ToList();
                        copiedSwipe.Scale();
                        newList.Add(copiedSwipe);
                        break;
                    default:
                        newList.Add(action);
                        break;
                }
            }

            ApplyConvertSetting(macro);

            return newList;
        }

        private MacroTemplate CopyMacro(MacroTemplate macro)
        {
            if (macro == null)
                throw new ArgumentNullException();

            var temp = Guid.NewGuid().ToString();
            var tempFolder = Path.Combine(Environment.CurrentDirectory, temp);
            Directory.CreateDirectory(tempFolder);
            var tempFilePath = Path.Combine(tempFolder, temp + ".emm");

            dataIO.SaveToFile(macro, tempFilePath);
            try
            {
                return dataIO.LoadMacroFileFromPath(tempFilePath).MacroTemplate;
            }
            finally
            {
                if (Directory.Exists(tempFolder))
                    Directory.Delete(tempFolder, true);
            }
        }
    }
}
