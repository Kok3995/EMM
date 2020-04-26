using Data;
using EMM.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM.Core
{
    /// <summary>
    /// Macro to nox script
    /// </summary>
    public class NoxMacroGenerator : IMacroScriptGenerator
    {
        public NoxMacroGenerator(IActionToScriptFactory actionToScriptFactory)
        {
            this.actionToScriptFactory = actionToScriptFactory;
        }

        private IActionToScriptFactory actionToScriptFactory;

        public object MacroToScript(MacroTemplate macro)
        {
            return MacroToScript(macro.ActionGroupList);
        }

        public virtual object MacroToScript(IList<IAction> actions)
        {
            var timer = StaticVariables.DEFAULT_STARTTIME;

            var script = new StringBuilder();

            foreach (var actionGroup in actions)
            {
                script.Append(actionToScriptFactory.GetActionScriptGenerator(GlobalData.Emulator, actionGroup.BasicAction).ActionToScript(actionGroup, ref timer));
            }

            return script.ToString();
        }
    }
}
