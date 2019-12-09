using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace AEMG_EX.Core
{
    public static class AEMGStatic
    {
        public static string MACRO_FOLDER = Path.Combine(Environment.CurrentDirectory, "Macroes");
        public static string AEMG_FOLDER = Path.Combine(Environment.CurrentDirectory, "AEMG");
        public const string AEMG_DEFAULT_PREDEFINED_ACTIONS_FILENAME = "DefaultAction";
        public const string EMM_NAME = "EMM.exe";

        public static BitmapImage C1 = new BitmapImage(new Uri(Path.Combine(Environment.CurrentDirectory,"Images", "Battle", "C-1.png")));
        public static BitmapImage C2 = new BitmapImage(new Uri(Path.Combine(Environment.CurrentDirectory,"Images", "Battle", "C-2.png")));
        public static BitmapImage C3 = new BitmapImage(new Uri(Path.Combine(Environment.CurrentDirectory,"Images", "Battle", "C-3.png")));
        public static BitmapImage C4 = new BitmapImage(new Uri(Path.Combine(Environment.CurrentDirectory,"Images", "Battle", "C-4.png")));
        public static BitmapImage C5 = new BitmapImage(new Uri(Path.Combine(Environment.CurrentDirectory,"Images", "Battle", "C-5.png")));
        public static BitmapImage C6 = new BitmapImage(new Uri(Path.Combine(Environment.CurrentDirectory,"Images", "Battle", "C-6.png")));

        public static BitmapImage BattleScreenNormal = new BitmapImage(new Uri(Path.Combine(Environment.CurrentDirectory, "Images", "UI", "BattleScreen.jpg")));
        public static BitmapImage BattleScreenBoss = new BitmapImage(new Uri(Path.Combine(Environment.CurrentDirectory, "Images", "UI", "BattleScreenBoss.jpg")));
        public static BitmapImage AFScreen = new BitmapImage(new Uri(Path.Combine(Environment.CurrentDirectory, "Images", "UI", "AFScreen.png")));
    }
}
