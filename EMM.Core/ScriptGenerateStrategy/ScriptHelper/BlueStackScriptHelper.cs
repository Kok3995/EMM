using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace EMM.Core
{
    public class BlueStackScriptHelper : NoxScriptHelper
    {
        public override object CreateScriptObject()
        {
            return new List<BlueStackEvent>();
        }

        public override void AppendAction(object scriptObj, MouseAction mouseAction, double x, double y, ref int timer)
        {
            if (!(scriptObj is List<BlueStackEvent> script))
            {
                throw new ArgumentException("Passed in object is not of type List<BlueStackEvent>");
            }

            switch (mouseAction)
            {
                case MouseAction.MouseDown:
                    script.Add(new BlueStackEvent(timer, Helpers.GetPercentage(x, GlobalData.CustomX), Helpers.GetPercentage(y, GlobalData.CustomY), BSEventType.MouseDown));
                    break;
                case MouseAction.MouseDrag:
                    script.Add(new BlueStackEvent(timer, Helpers.GetPercentage(x, GlobalData.CustomX), Helpers.GetPercentage(y, GlobalData.CustomY), BSEventType.MouseMove));
                    break;
                case MouseAction.MouseUp:
                    script.Add(new BlueStackEvent(timer, Helpers.GetPercentage(x, GlobalData.CustomX), Helpers.GetPercentage(y, GlobalData.CustomY), BSEventType.MouseUp));
                    break;
            }
        }

        public override void AddToGroup(object scriptObj, object actionToAdd)
        {
            if (!(scriptObj is List<BlueStackEvent> script) || !(actionToAdd is List<BlueStackEvent> events))
            {
                throw new ArgumentException("Passed in object is not of type List<BlueStackEvent>");
            }

            if (events.Count > 0)
                script.AddRange(events);
        }
    }
}
