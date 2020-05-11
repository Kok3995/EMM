using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace EMM.Core
{
    public class AnkuLuaScriptHelper : NoxScriptHelper
    {
        private const string touchDown = "touchDown";
        private const string touchMove = "touchMove";
        private const string touchUp = "touchUp";
        private const string wait = "wait";
        public override void AppendAction(object scriptObj, MouseAction mouseAction, double x, double y, ref int timer)
        {
            if (!(scriptObj is StringBuilder script))
            {
                throw new ArgumentException("Passed in object is not of type StringBuilder");
            }

            switch (mouseAction)
            {
                case MouseAction.MouseDown:
                    script.Append($"{{ action = \"{touchDown}\", target = Location({x}, {y}) }},").AppendLine();
                    break;
                case MouseAction.MouseDrag:
                    script.Append($"{{ action = \"{touchMove}\", target = Location({x}, {y}) }},").AppendLine();
                    break;
                case MouseAction.MouseUp:
                    script.Append($"{{ action = \"{touchUp}\", target = Location({x}, {y}) }},").AppendLine();
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

            script.Append($"{{ action = \"{wait}\", target = {((double)holdTime / 1000):F3} }},").AppendLine();
        }

        public override void WaitNext(object scriptObj, int waitNextTime, ref int timer)
        {
            Hold(scriptObj, waitNextTime, ref timer);
        }
    }
}
