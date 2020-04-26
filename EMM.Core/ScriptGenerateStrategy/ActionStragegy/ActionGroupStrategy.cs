using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace EMM.Core
{
    public class ActionGroupStrategy : BaseScriptGenerateStragegy
    {
        public ActionGroupStrategy(IActionToScriptFactory actionToScriptFactory, IScriptHelper helper) : base(helper)
        {
            this.actionToScriptFactory = actionToScriptFactory;
        }

        protected IActionToScriptFactory actionToScriptFactory;

        public override object ActionToScript(IAction action, ref int timer)
        {
            var scriptObj = helper.CreateScriptObject();

            if (!(action is ActionGroup actionGroup))
            {
                throw new InvalidOperationException("Action is not an ActionGroup");
            }

            if (action.IsDisable)
            {
                return scriptObj;
            }

            for (int i = 1; i <= actionGroup.Repeat; i++)
            {
                foreach (var act in actionGroup.ActionList)
                {
                    helper.AddToGroup(scriptObj, actionToScriptFactory.GetActionScriptGenerator(GlobalData.Emulator, act.BasicAction).ActionToScript(act, ref timer));
                }
            }

            return scriptObj;
        }
    }
}
