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
    public class BlueStackMacroGenerator : NoxMacroGenerator
    {
        public BlueStackMacroGenerator(IActionToScriptFactory actionToScriptFactory) : base(actionToScriptFactory)
        {
            this.actionToScriptFactory = actionToScriptFactory;
        }

        private IActionToScriptFactory actionToScriptFactory;

        public override object MacroToScript(IList<IAction> actions)
        {
            var timer = StaticVariables.DEFAULT_STARTTIME;

            var script = new List<BlueStackEvent>();

            foreach (var actionGroup in actions)
            {
                if (actionToScriptFactory.GetActionScriptGenerator(GlobalData.Emulator, actionGroup.BasicAction).ActionToScript(actionGroup, ref timer) is List<BlueStackEvent> eventItem)
                {
                    script.AddRange(eventItem);
                }
            }

            return script;
        }
    }
}
