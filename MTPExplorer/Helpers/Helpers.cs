using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTPExplorer
{
    /// <summary>
    /// Common methods
    /// </summary>
    internal static class Helpers
    {
        internal static bool IsValidFilename(string testName)
        {
            var arr = Path.GetInvalidFileNameChars();
            return !testName.Any(l => arr.Contains(l));
        }

        internal static bool IsMTPPathValid(string path)
        {
            var nameArr = path.Split(Path.DirectorySeparatorChar);

            return !(string.IsNullOrWhiteSpace(path) || !path.ToLower().StartsWith("mtp:") || nameArr.Length < 1);
        }
    }
}
