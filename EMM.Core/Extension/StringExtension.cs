using System;

namespace EMM.Core.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// Properly null terminate a string
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        public static string NullTerminateString(this string targetString)
        {
            var indexOfNullCharacter = targetString.IndexOf("\0");

            if (indexOfNullCharacter > 0)
                targetString = targetString.Substring(0, indexOfNullCharacter);

            return targetString;
        }
    }
}
