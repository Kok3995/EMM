using System;
using System.Reflection;

namespace EMM.Core.Update
{
    /// <summary>
    /// Store update infomation
    /// </summary>
    public class UpdateInfo
    {
        public Version Version { get; set; }

        public string Url { get; set; } = string.Empty;

        public string Changelog { get; set; } = string.Empty;
    }
}
