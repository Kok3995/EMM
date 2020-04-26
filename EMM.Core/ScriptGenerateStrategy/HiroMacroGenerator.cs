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
    /// Macro to Hiro macro script
    /// </summary>
    public class HiroMacroGenerator : NoxMacroGenerator
    {
        public HiroMacroGenerator(IActionToScriptFactory actionToScriptFactory) : base(actionToScriptFactory)
        {
            this.actionToScriptFactory = actionToScriptFactory;
        }

        private IActionToScriptFactory actionToScriptFactory;

        public override object MacroToScript(IList<IAction> actions)
        {
            var timer = StaticVariables.DEFAULT_STARTTIME;

            var script = new StringBuilder();

            script.Append(":start").AppendLine();

            foreach (var actionGroup in actions)
            {
                script.Append(actionToScriptFactory.GetActionScriptGenerator(GlobalData.Emulator, actionGroup.BasicAction).ActionToScript(actionGroup, ref timer));
            }

            script.Append(":end");

            return script.ToString();
        }
    }
}
