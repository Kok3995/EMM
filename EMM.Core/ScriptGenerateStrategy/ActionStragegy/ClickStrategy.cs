using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace EMM.Core
{
    /// <summary>
    /// Click to Nox script
    /// </summary>
    public class ClickStrategy : BaseScriptGenerateStragegy
    {
        public ClickStrategy(IScriptHelper helper) : base(helper)
        {
        }

        public override object ActionToScript(IAction action, ref int timer)
        {
            if (!(action is Click click))
            {
                throw new InvalidOperationException("Action is not a click");
            }

            var script = helper.CreateScriptObject();

            if (action.IsDisable)
            {
                return script;
            }

            click.Scale();

            var random = new Random();

            for (int i = 1; i <= click.Repeat; i++)
            {
                var point = click.Randomize(random);

                //Mouse down
                helper.AppendAction(script, MouseAction.MouseDown, point.X, point.Y, ref timer);

                //hold time
                helper.Hold(script, click.HoldTime, ref timer);

                //mouse up
                helper.AppendAction(script, MouseAction.MouseUp, point.X, point.Y, ref timer);

                //wait for next action
                helper.WaitNext(script, click.WaitBetweenAction, ref timer);
            }

            return script;
        }
    }
}
