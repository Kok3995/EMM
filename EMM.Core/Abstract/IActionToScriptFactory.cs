using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM.Core
{
    /// <summary>
    /// Factory for <see cref="IActionScriptGenerator"/>
    /// </summary>
    public interface IActionToScriptFactory
    {
        /// <summary>
        /// Return a script generator base on the selected Emulator and the action pass in
        /// </summary>
        /// <param name="emulator"></param>
        /// <param name="basicAction"></param>
        /// <returns></returns>

        IActionScriptGenerator GetActionScriptGenerator(Emulator emulator, BasicAction basicAction);

        /// <summary>
        /// Set the dependency diction for the factory
        /// </summary>
        /// <param name=""></param>
        void SetDependency(Dictionary<Emulator, Dictionary<BasicAction, IActionScriptGenerator>> dict);
    }
}
