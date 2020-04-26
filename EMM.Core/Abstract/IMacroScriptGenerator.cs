using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM.Core
{
    /// <summary>
    /// Generate script from macro base on the selected emulator
    /// </summary>
    public interface IMacroScriptGenerator
    {
        /// <summary>
        /// Generate script from <see cref="MacroTemplate"/>
        /// </summary>
        /// <returns></returns>
        object MacroToScript(MacroTemplate macro);

        /// <summary>
        /// Generate script from a List of <see cref="IAction"/>
        /// </summary>
        /// <returns></returns>
        object MacroToScript(IList<IAction> actions);
    }
}
