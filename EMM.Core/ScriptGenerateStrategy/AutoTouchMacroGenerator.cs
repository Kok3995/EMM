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
    /// Macro to AutoTouch script
    /// </summary>
    public class AutoTouchMacroGenerator : NoxMacroGenerator
    {
        public AutoTouchMacroGenerator(IActionToScriptFactory actionToScriptFactory) : base(actionToScriptFactory)
        {
        }
    }
}
