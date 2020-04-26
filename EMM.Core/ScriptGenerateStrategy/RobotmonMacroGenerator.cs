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
    /// Macro to Robotmon script
    /// </summary>
    public class RobotmonMacroGenerator : NoxMacroGenerator
    {
        public RobotmonMacroGenerator(IActionToScriptFactory actionToScriptFactory) : base(actionToScriptFactory)
        {
            this.actionToScriptFactory = actionToScriptFactory;
        }

        private IActionToScriptFactory actionToScriptFactory;

        public override object MacroToScript(IList<IAction> actions)
        {
            var timer = StaticVariables.DEFAULT_STARTTIME;

            var script = new List<string>();

            foreach (var actionGroup in actions)
            {
                var actionToScript = actionToScriptFactory.GetActionScriptGenerator(GlobalData.Emulator, actionGroup.BasicAction).ActionToScript(actionGroup, ref timer);

                if (!(actionToScript is List<string> groupScript))
                    throw new InvalidOperationException("Return type is not List<string>");

                script.AddRange(groupScript);
            }

            return script;
        }
    }
}
