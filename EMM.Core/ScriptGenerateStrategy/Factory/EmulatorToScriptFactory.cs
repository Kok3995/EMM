using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace EMM.Core
{
    public class EmulatorToScriptFactory : IEmulatorToScriptFactory
    {
        public EmulatorToScriptFactory(Dictionary<Emulator, IMacroScriptGenerator> dict)
        {
            this.dict = dict;
        }

        private Dictionary<Emulator, IMacroScriptGenerator> dict { get; set; }

        public IMacroScriptGenerator GetEmulatorScriptGenerator(Emulator emulator)
        {
            if (!dict.ContainsKey(emulator))
            {
                throw new NotImplementedException(emulator.ToString() + " is not implemented");
            }

            return dict[emulator];
        }
    }
}
