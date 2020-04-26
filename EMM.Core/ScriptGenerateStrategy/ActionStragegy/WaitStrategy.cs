using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace EMM.Core
{
    public class WaitStrategy : BaseScriptGenerateStragegy
    {
        public WaitStrategy(IScriptHelper helper) : base(helper)
        {
        }

        public override object ActionToScript(IAction action, ref int timer)
        {
            if (!(action is Wait wait))
            {
                throw new InvalidOperationException("Action is not a Wait");
            }

            var script = helper.CreateScriptObject();

            if (!action.IsDisable)
            {
                helper.WaitNext(script, wait.WaitTime, ref timer);
            }

            return script;
        }
    }
}
