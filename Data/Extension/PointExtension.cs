using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Data
{
    /// <summary>
    /// Extend point struct
    /// </summary>
    public static class PointExtension
    {
        public static Point ScalePointBaseOnMode(this Point point)
        {
            var x = point.X;
            var y = point.Y;

            switch (GlobalData.ScaleMode)
            {
                case ScaleMode.Stretch:
                    x = (GlobalData.ScaleX.Equals(1.0)) ? x : Math.Round(x * GlobalData.ScaleX);
                    y = (GlobalData.ScaleY.Equals(1.0)) ? y : Math.Round(y * GlobalData.ScaleY);
                    break;
                case ScaleMode.Fit:
                    x = (GlobalData.ScaleFit.Equals(1.0)) ? x : Math.Round(x * GlobalData.ScaleFit);
                    y = (GlobalData.ScaleFit.Equals(1.0)) ? y : Math.Round(y * GlobalData.ScaleFit);
                    if (GlobalData.DeltaX != 0)
                    {
                        x += GlobalData.DeltaX;
                    }
                    if (GlobalData.DeltaY != 0)
                    {
                        y += GlobalData.DeltaY;
                    }
                    break;
                case ScaleMode.Zoom:
                    x = (GlobalData.ScaleZoom.Equals(1.0)) ? x : Math.Round(x * GlobalData.ScaleZoom);
                    y = (GlobalData.ScaleZoom.Equals(1.0)) ? y : Math.Round(y * GlobalData.ScaleZoom);
                    if (GlobalData.DeltaX != 0)
                    {
                        x += GlobalData.DeltaX;
                        //if (x <= 0)
                        //    x = 0;
                    }
                    if (GlobalData.DeltaY != 0)
                    {
                        y += GlobalData.DeltaY;
                        //if (y <= 0)
                        //    y = 0;
                    }
                    break;
            }

            //adjust for hiromacro
            if (GlobalData.CustomX > GlobalData.CustomY && GlobalData.Emulator == Emulator.HiroMacro)
            {
                var temp = y;
                y = x;
                x = GlobalData.CustomY - temp;
            }

            //adjust for LDPLayer
            if (GlobalData.Emulator == Emulator.LDPlayer)
            {     
                x = x * 19200 / GlobalData.CustomX;
                y = y * 10800 / GlobalData.CustomY;
            }

            return new Point(x, y);
        }
    }
}
