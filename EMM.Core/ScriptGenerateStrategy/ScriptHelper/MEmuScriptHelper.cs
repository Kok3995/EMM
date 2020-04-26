using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace EMM.Core
{
    public class MEmuScriptHelper : NoxScriptHelper
    {
        public override void AppendAction(object scriptObj, MouseAction mouseAction, double x, double y, ref int timer)
        {
            if (!(scriptObj is StringBuilder script))
            {
                throw new ArgumentException("Passed in object is not of type StringBuilder");
            }

            if (mouseAction == MouseAction.MouseUp)
            {
                script.Append($"{timer.ToString()}")
                    .Append(GlobalData.memuwhatisthis)
                    .Append(GlobalData.menumouseup)
                    .AppendLine();
            }
            else
            {
                var action = (mouseAction == MouseAction.MouseDown) ? "0" : "1";

                script.Append($"{timer.ToString()}")
                .Append(GlobalData.memuwhatisthis)
                .Append("0")
                .Append(GlobalData.memuSeperator)
                .Append(x.ToString())
                .Append(GlobalData.memuSeperator)
                .Append(y.ToString())
                .Append(GlobalData.memuSeperator)
                .Append(action)
                .AppendLine();
            }
        }
    }
}
