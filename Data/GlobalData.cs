using System;

namespace Data
{
    /// <summary>
    /// This class hold some static data needed to generate the scripts
    /// </summary>
    public static class GlobalData
    {
        #region Script settings

        public static int Randomize;

        public static double ScaleX => (double)CustomX / OriginalX;
        public static double ScaleY => (double)CustomY / OriginalY;
        public static double ScaleFit => (CustomX > CustomY) ? ScaleY : ScaleX;
        public static double ScaleZoom => (CustomX > CustomY) ? ScaleX : ScaleY;
        public static int CustomX = 1280;
        public static int CustomY = 720;
        public static int OriginalX = 1280;
        public static int OriginalY = 720;
        public static double DeltaX => ScaleMode == ScaleMode.Fit ? Math.Round(CustomX - ScaleFit * OriginalX) / 2 : ScaleMode == ScaleMode.Zoom ? Math.Round(CustomX - ScaleZoom * OriginalX) /2 : 0;
        public static double DeltaY => ScaleMode == ScaleMode.Fit ? Math.Round(CustomY - ScaleFit * OriginalY) / 2 : ScaleMode == ScaleMode.Zoom ? Math.Round(CustomY - ScaleZoom * OriginalY) /2 : 0;

        public static Emulator Emulator = Emulator.Nox;

        public static ScaleMode ScaleMode = ScaleMode.Stretch;

        #endregion

        #region NOX

        public static string noxSeperator = @"|";
        public static string whatisthis = @"|0|0|0|";

        #endregion

        #region MEMU

        public static string memuSeperator = @":";
        public static string memuwhatisthis = @"000--VINPUT--MULTI2:1:0:";
        public static string menumouseup = @"-1:-1:-1:2";

        #endregion
    }
}
