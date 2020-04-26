using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace EMM.Core
{
    class ActionToScriptFactory : IActionToScriptFactory
    {
        public ActionToScriptFactory()
        {

        }

        public ActionToScriptFactory(Dictionary<Emulator, Dictionary<BasicAction, IActionScriptGenerator>> dict)
        {
            this.dict = dict;
        }

        private Dictionary<Emulator, Dictionary<BasicAction, IActionScriptGenerator>> dict;

        public IActionScriptGenerator GetActionScriptGenerator(Emulator emulator, BasicAction basicAction)
        {
            if (!dict.ContainsKey(emulator))
            {
                throw new NotImplementedException(emulator.ToString() + " is not implemented");
            }

            var actionDict = dict[emulator];

            if (!actionDict.ContainsKey(basicAction))
            {
                throw new NotImplementedException(basicAction.ToString() + " is not implemented");
            }

            return actionDict[basicAction];
        }

        public void SetDependency(Dictionary<Emulator, Dictionary<BasicAction, IActionScriptGenerator>> dict)
        {
            this.dict = dict;
        }
    }
}
