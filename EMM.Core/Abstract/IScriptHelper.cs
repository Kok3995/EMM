using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM.Core
{
    public interface IScriptHelper
    {
        void AppendAction(object scriptObj, MouseAction mouseAction, double x, double y, ref int timer);
        void AddToGroup(object scriptObj, object actionToAdd);
        void Hold(object scriptObj, int holdTime, ref int timer);
        void WaitNext(object scriptObj, int waitNextTime, ref int timer);
        object CreateScriptObject();
    }
}
