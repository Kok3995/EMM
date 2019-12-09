using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public static class StringBuilderExtension
    {
        /// <summary>
        /// Generate NOX script for each Point
        /// </summary>
        /// <param name="script">The script to append</param>
        /// <param name="action">0 for mouse down, 1 for mouse up, 2 for move</param>
        /// <param name="x">point x</param>
        /// <param name="y">point y</param>
        /// <param name="timer">ref to current timer</param>
        /// <param name="resolution">resolution of NOX</param>
        public static void AppendAction(this StringBuilder script, ScriptMouseAction mouseAction, string x, string y, ref int timer)
        {
            switch (GlobalData.Emulator)
            {
                case Emulator.Nox:
                    script.Append((int)mouseAction)
                    .Append(GlobalData.noxSeperator)
                    .Append(x)
                    .Append(GlobalData.noxSeperator)
                    .Append(y)
                    .Append(GlobalData.whatisthis)
                    .Append(timer.ToString())
                    .Append(GlobalData.noxSeperator)
                    .Append(GlobalData.CustomX)
                    .Append(GlobalData.noxSeperator)
                    .Append(GlobalData.CustomY)
                    .AppendLine();
                    break;
                case Emulator.Memu:
                    if (mouseAction == ScriptMouseAction.MouseUp)
                    {
                        script.Append($"{timer.ToString()}")
                            .Append(GlobalData.memuwhatisthis)
                            .Append(GlobalData.menumouseup)
                            .AppendLine();
                        break;
                    }
                    else
                    {
                        var action = (mouseAction == ScriptMouseAction.MouseDown) ? "0" : "1";

                        script.Append($"{timer.ToString()}")
                        .Append(GlobalData.memuwhatisthis)
                        .Append("0")
                        .Append(GlobalData.memuSeperator)
                        .Append(x)
                        .Append(GlobalData.memuSeperator)
                        .Append(y)
                        .Append(GlobalData.memuSeperator)
                        .Append(action)
                        .AppendLine();
                        break;
                    }
            }
            
        }
    }
}
