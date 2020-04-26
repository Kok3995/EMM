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
        /// <param name="x">point x</param>
        /// <param name="y">point y</param>
        /// <param name="timer">ref to current timer</param>
        public static void AppendNoxAction(this StringBuilder script, MouseAction mouseAction, string x, string y, ref int timer)
        {
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
        }

        /// <summary>
        /// Generate NOX script for each Point
        /// </summary>
        /// <param name="script">The script to append</param>
        /// <param name="x">point x</param>
        /// <param name="y">point y</param>
        /// <param name="timer">ref to current timer</param>
        public static void AppendMEmuAction(this StringBuilder script, MouseAction mouseAction, string x, string y, ref int timer)
        {           
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
                .Append(x)
                .Append(GlobalData.memuSeperator)
                .Append(y)
                .Append(GlobalData.memuSeperator)
                .Append(action)
                .AppendLine();
            }          
        }

        /// <summary>
        /// Generate Hiro Macro script for each Point
        /// </summary>
        /// <param name="script"></param>
        /// <param name="mouseAction"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="timer"></param>
        public static void AppendHiroAction(this StringBuilder script, HiroAnkuLuaMouseAction mouseAction, string x = null, string y = null, int time = 0)
        {
            switch(mouseAction)
            {
                case HiroAnkuLuaMouseAction.touchDown:
                case HiroAnkuLuaMouseAction.touchMove:
                    script.Append($"{mouseAction.ToString()} 0 {x} {y}").AppendLine();
                    break;
                case HiroAnkuLuaMouseAction.touchUp:
                    script.Append($"{mouseAction.ToString()} 0").AppendLine();
                    break;
                case HiroAnkuLuaMouseAction.sleep:
                    script.Append($"{mouseAction.ToString()} {time}").AppendLine();
                    break;
            }
        }

        /// <summary>
        /// Generate AnkuLua script for each Point
        /// </summary>
        /// <param name="script"></param>
        /// <param name="mouseAction"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="timer"></param>
        public static void AppendAnkuAction(this StringBuilder script, HiroAnkuLuaMouseAction mouseAction, string x = null, string y = null, int time = 0)
        {
            switch (mouseAction)
            {
                case HiroAnkuLuaMouseAction.touchDown:
                case HiroAnkuLuaMouseAction.touchMove:
                case HiroAnkuLuaMouseAction.touchUp:
                    script.Append($"{mouseAction.ToString()}(Location({x}, {y}))").AppendLine();
                    break;
                case HiroAnkuLuaMouseAction.wait:
                    script.Append($"{mouseAction.ToString()}({(double)time/1000:F3})").AppendLine();
                    break;
            }
        }
    }
}
