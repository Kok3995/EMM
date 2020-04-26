using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEMG_EX.Core
{
    public class AEMGHelpers
    {
        public static void StartEMM(string path, string agrs = "")
        {
            using (var process = new Process())
            {
                var startInfo = new ProcessStartInfo()
                {
                    FileName = AEMGStatic.EMM_NAME,
                    Arguments = "\"" + path + "\"" + (agrs.Equals("") ? "" : " " + agrs),
                    UseShellExecute = false
                };

                process.StartInfo = startInfo;
                process.Start();
            }
        }

        public static bool IsValidFilename(string testName)
        {
            var arr = Path.GetInvalidFileNameChars();
            return !testName.Any(l => arr.Contains(l));
        }

    }
}
