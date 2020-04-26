using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM.Core
{
    /// <summary>
    /// Interface to Iaction to convert to script
    /// </summary>
    public interface IActionScriptGenerator
    {
        /// <summary>
        /// Convert an IAction to script
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        object ActionToScript(IAction action, ref int timer);
    }
}
