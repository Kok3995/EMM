using EMM.Core.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM.Core
{
    public static class StaticVariables
    {
        public const int DEFAULT_STARTTIME = 200;

        public static string DEFAULT_NOX_FOLDER = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Nox", "record");

        public static string DEFAULT_MEMU_FOLDER = Path.Combine(Helpers.GetInstallationPath("MEmu"), "MEmu", "scripts");

        public static string DEFAULT_BLUESTACKS_FOLDER = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "ProgramData", "BlueStacks", "Engine", "UserData", "InputMapper", "UserScripts");

        public static string DEFAULT_LDPlayer_FOLDER = Path.Combine(Helpers.GetInstallationPath("LDPlayer"), "vms", "operationRecords");

        public static string DEFAULT_MOBILE_FOLDER = Environment.CurrentDirectory;

        public const string NO_SAVE_AGRS = "no-save";

        public static string TEMP_FOLDER = Path.Combine(Environment.CurrentDirectory, "temp");
    }
}
