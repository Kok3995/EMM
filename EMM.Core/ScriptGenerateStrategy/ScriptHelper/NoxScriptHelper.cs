using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace EMM.Core
{
    public class NoxScriptHelper : IScriptHelper
    {
        public virtual void AddToGroup(object scriptObj, object actionToAdd)
        {
            if (!(scriptObj is StringBuilder script))
            {
                throw new ArgumentException("Action is not an ActionGroup");
            }

            script.Append(actionToAdd);
        }

        public virtual void AppendAction(object scriptObj, MouseAction mouseAction, double x, double y, ref int timer)
        {
            if (!(scriptObj is StringBuilder script))
            {
                throw new ArgumentException("Passed in object is not of type StringBuilder");
            }

            script.Append((int)mouseAction)
            .Append(GlobalData.noxSeperator)
            .Append(x.ToString())
            .Append(GlobalData.noxSeperator)
            .Append(y.ToString())
            .Append(GlobalData.whatisthis)
            .Append(timer.ToString())
            .Append(GlobalData.noxSeperator)
            .Append(GlobalData.CustomX)
            .Append(GlobalData.noxSeperator)
            .Append(GlobalData.CustomY)
            .AppendLine();
        }

        public virtual object CreateScriptObject()
        {
            return new StringBuilder();
        }

        public virtual void Hold(object scriptObj, int holdTime, ref int timer)
        {
            timer += holdTime;
        }

        public virtual void WaitNext(object scriptObj, int waitNextTime, ref int timer)
        {
            timer += waitNextTime;
        }
    }
}
