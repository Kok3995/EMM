using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace EMM.Core
{
    public class HiroMacroScriptHelper : NoxScriptHelper
    {
        private const string touchDown = "touchDown";
        private const string touchMove = "touchMove";
        private const string touchUp = "touchUp";
        private const string sleep = "sleep";
        public override void AppendAction(object scriptObj, MouseAction mouseAction, double x, double y, ref int timer)
        {
            if (!(scriptObj is StringBuilder script))
            {
                throw new ArgumentException("Passed in object is not of type StringBuilder");
            }

            switch (mouseAction)
            {
                case MouseAction.MouseDown:
                    script.Append($"{touchDown} 0 {x} {y}").AppendLine();
                    break;
                case MouseAction.MouseDrag:
                    script.Append($"{touchMove} 0 {x} {y}").AppendLine();
                    break;
                case MouseAction.MouseUp:
                    script.Append($"{touchUp.ToString()} 0").AppendLine();
                    break;
            }
        }

        public override void Hold(object scriptObj, int holdTime, ref int timer)
        {
            if (!(scriptObj is StringBuilder script))
            {
                throw new ArgumentException("Passed in object is not of type StringBuilder");
            }

            if (holdTime <= 0)
                return;

            script.Append($"{sleep} {holdTime}").AppendLine();
        }

        public override void WaitNext(object scriptObj, int waitNextTime, ref int timer)
        {
            Hold(scriptObj, waitNextTime, ref timer);
        }
    }
}
