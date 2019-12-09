namespace Data
{
    /// <summary>
    /// This class hold some static data needed to generate the scripts
    /// </summary>
    public static class GlobalData
    {
        #region Script settings

        public static int Randomize;

        public static double ScaleX = 1.0;
        public static double ScaleY = 1.0;
        public static int CustomX = 1280;
        public static int CustomY = 720;

        public static Emulator Emulator = Emulator.Nox;

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
