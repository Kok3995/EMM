using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace EMM.Core
{
    public class RobotmonScriptHelper : NoxScriptHelper
    {
        private const string tapDown = "tapDown";
        private const string tapUp = "tapUp";
        private const string moveTo = "moveTo";
        private const string sleep = "sleep";

        public override object CreateScriptObject()
        {
            return new List<string>();
        }

        public override void AddToGroup(object scriptObj, object actionToAdd)
        {
            if (!(scriptObj is List<string> script) || !(actionToAdd is List<string> events))
            {
                throw new ArgumentException("Passed in object is not of type List<string>");
            }

            script.AddRange(events);
        }

        public override void AppendAction(object scriptObj, MouseAction mouseAction, double x, double y, ref int timer)
        {
            if (!(scriptObj is List<string> script))
            {
                throw new ArgumentException("Passed in object is not of type List<string>");
            }

            switch (mouseAction)
            {
                case MouseAction.MouseDown:
                    script.Add($"\"{tapDown}({x}, {y}, 0)\"");
                    break;
                case MouseAction.MouseDrag:
                    script.Add($"\"{moveTo}({x}, {y}, 0)\"");
                    break;
                case MouseAction.MouseUp:
                    script.Add($"\"{tapUp}({x}, {y}, 0)\"");
                    break;
            }
        }

        public override void Hold(object scriptObj, int holdTime, ref int timer)
        {
            if (!(scriptObj is List<string> script))
            {
                throw new ArgumentException("Passed in object is not of type List<string>");
            }

            if (holdTime <= 0)
                return;

            script.Add($"\"{sleep}({holdTime.ToString()})\"");
        }

        public override void WaitNext(object scriptObj, int waitNextTime, ref int timer)
        {
            Hold(scriptObj, waitNextTime, ref timer);
        }
    }
}
