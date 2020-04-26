using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM.Core
{
    /// <summary>
    /// Factory to get the correct script generator for the macro
    /// </summary>
    public interface IEmulatorToScriptFactory
    {
        /// <summary>
        /// Return the generator the the pass in emulator
        /// </summary>
        /// <param name="emulator"></param>
        /// <returns></returns>
        IMacroScriptGenerator GetEmulatorScriptGenerator(Emulator emulator);
    }
}
