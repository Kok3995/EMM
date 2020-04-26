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
    /// Macro to MEmu script
    /// </summary>
    public class AnkuMacroGenerator : NoxMacroGenerator
    {
        public AnkuMacroGenerator(IActionToScriptFactory actionToScriptFactory) : base(actionToScriptFactory)
        {
        }
    }
}
