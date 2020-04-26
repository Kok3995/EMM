using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace EMM.Core
{
    public class LDPlayerScriptHelper : BlueStackScriptHelper
    {
        public override object CreateScriptObject()
        {
            return new List<LDPlayerOperation>();
        }

        public override void AppendAction(object scriptObj, MouseAction mouseAction, double x, double y, ref int timer)
        {
            if (!(scriptObj is List<LDPlayerOperation> script))
            {
                throw new ArgumentException("Passed in object is not of type List<LDPlayerOperation>");
            }

            switch (mouseAction)
            {
                case MouseAction.MouseDown:
                case MouseAction.MouseDrag:
                    script.Add(new LDPlayerOperation(timer, (int)x, (int)y, LDPLayerState.MouseDown));
                    break;
                case MouseAction.MouseUp:
                    script.Add(new LDPlayerOperation(timer, (int)x, (int)y, LDPLayerState.MouseUp));
                    break;
            }
        }

        public override void AddToGroup(object scriptObj, object actionToAdd)
        {
            if (!(scriptObj is List<LDPlayerOperation> script) || !(actionToAdd is List<LDPlayerOperation> events))
            {
                throw new ArgumentException("Passed in object is not of type List<LDPlayerOperation>");
            }

            if (events.Count > 0)
                script.AddRange(events);
        }
    }
}
