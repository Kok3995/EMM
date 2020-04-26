using Data;
using EMM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM.Test
{
    internal class Helpers
    {
        internal static Click GetClick(double x, double y)
        {
            return new Click { ClickPoint = new System.Windows.Point(x, y), HoldTime = 500, Repeat = 1, WaitBetweenAction = 1000 };
        }

        internal static void ApplySetting(int ox, int oy, int cx, int cy, Emulator emulator = Emulator.Nox, ScaleMode scale = ScaleMode.Stretch)
        {
            GlobalData.CustomX = cx;
            GlobalData.CustomY = cy;
            GlobalData.OriginalX = ox;
            GlobalData.OriginalY = oy;
            GlobalData.Emulator = emulator;
            GlobalData.ScaleMode = scale;
        }

        internal static void GetScriptGenerator(out IActionToScriptFactory actionToScriptFactory, out IEmulatorToScriptFactory emulatorToScriptFactory)
        {
            new ScriptGenerateBootstrap().SetUp(out actionToScriptFactory, out emulatorToScriptFactory);
        }
    }
}
