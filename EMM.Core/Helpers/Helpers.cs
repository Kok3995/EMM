using Data;
using EMM.Core.Extension;
using EMM.Core.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EMM.Core
{
    /// <summary>
    /// Helper methods
    /// </summary>
    public class Helpers
    {
        /// <summary>
        /// Return a list of in-between points to make up a swipe motion
        /// </summary>
        /// <param name="startPoint">Starting Point</param>
        /// <param name="endPoint">End Point</param>
        /// <returns></returns>
        public static IEnumerable<Point> GetInBetweenPoints(Point startPoint, Point endPoint, int step)
        {
            double dx = endPoint.X - startPoint.X;
            double dy = endPoint.Y - startPoint.Y;
            double slope = dy / dx;

            int numberOfPoint = (int)Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)) / step;

            if (numberOfPoint == 0)
                yield break;

            double xstep = dx / numberOfPoint;
            double ystep = dy / numberOfPoint;
            --numberOfPoint;


            for (int i = 1; i <= numberOfPoint; i++)
            {
                double x;
                double y;

                if (Math.Abs(dx) >= Math.Abs(dy))
                {
                    x = xstep * i;
                    y = (dy == 0) ? 0 : x * slope;
                }
                else
                {
                    y = ystep * i;
                    x = (dx == 0) ? 0 : y / slope;
                }

                yield return new Point(Math.Round(startPoint.X + x), Math.Round(startPoint.Y + y));
            }
        }

        /// <summary>
        /// Scale and randomized the point
        /// </summary>
        /// <param name="point">The <see cref="SwipePoint"/> to randomize</param>
        /// <param name="random">The <see cref="Random"/> instance</param>
        /// <param name="xString">output x as string</param>
        /// <param name="yString">output y as string</param>
        /// <returns>The point that has been scaled and randomized</returns>
        public static Point ScaleAndRandomize(Point point, Random random, out string xString, out string yString)
        {
            //scale the point
            var x = (GlobalData.ScaleX.Equals(1.0)) ? point.X : Math.Round(point.X * GlobalData.ScaleX);
            var y = (GlobalData.ScaleX.Equals(1.0)) ? point.Y : Math.Round(point.Y * GlobalData.ScaleY);

            //randomized it
            x = (GlobalData.Randomize > 0) ? random.RandomCoordinate(x, GlobalData.Randomize) : x;
            y = (GlobalData.Randomize > 0) ? random.RandomCoordinate(y, GlobalData.Randomize) : y;

            //convert to string
            xString = x.ToString();
            yString = y.ToString();

            return new Point(x, y);
        }

        /// <summary>
        /// Return the percentage with 2 decimal places
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static double GetPercentage(double target, double source)
        {
            return Math.Round((target / source) * 100, 2);
        }

        /// <summary>
        /// Get the installation path of a program
        /// </summary>
        /// <param name="appName">name of the program</param>
        /// <returns></returns>
        public static string GetInstallationPath(string appName)
        {
            var path64 = $"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{appName}\\";
            var path32 = $"SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{appName}\\";

            List<string> pathFounded = new List<string>();
            string installPath64;
            string installPath32;

            using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (RegistryKey key = hklm.OpenSubKey(path64))
            {
                installPath64 = key?.GetValue("InstallLocation")?.ToString()?.NullTerminateString();

                pathFounded.Add(key?.GetValue("DisplayIcon")?.ToString()?.NullTerminateString() ?? string.Empty);
            }

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path32))
            {
                installPath32 = key?.GetValue("InstallLocation")?.ToString()?.NullTerminateString();

                pathFounded.Add(key?.GetValue("DisplayIcon")?.ToString()?.NullTerminateString() ?? string.Empty);
            }

            if (!string.IsNullOrEmpty(installPath64))
                return installPath64;

            if (!string.IsNullOrEmpty(installPath32))
                return installPath32;

            return GetFileDirectory(pathFounded.FirstOrDefault(i => !string.IsNullOrEmpty(i)));
        }

        /// <summary>
        /// Get file and folders directory
        /// </summary>
        /// <param name="directories">full path of file or folder you want the name of directory</param>
        /// <returns></returns>
        public static string GetFileDirectory(string directories)
        {
            if (string.IsNullOrEmpty(directories))
                return string.Empty;

            string uniPath = directories.Replace('/', '\\').Replace("\"", string.Empty);

            int lastIndex = uniPath.LastIndexOf('\\');

            if (lastIndex <= 0)
                return directories;

            return uniPath.Substring(0, lastIndex);
        }

        /// <summary>
        /// Scale and randomized the point for hiro macro script, adjust point in landscape
        /// </summary>
        /// <param name="point">The <see cref="SwipePoint"/> to randomize</param>
        /// <param name="random">The <see cref="Random"/> instance</param>
        /// <param name="xString">output x as string</param>
        /// <param name="yString">output y as string</param>
        /// <returns>The point that has been scaled and randomized</returns>
        public static Point HiroScaleAndRandomize(Point point, Random random, out string xString, out string yString)
        {
            //scale the point
            var x = (GlobalData.ScaleX.Equals(1.0)) ? point.X : Math.Round(point.X * GlobalData.ScaleX);
            var y = (GlobalData.ScaleX.Equals(1.0)) ? point.Y : Math.Round(point.Y * GlobalData.ScaleY);

            //randomized it
            x = (GlobalData.Randomize > 0) ? random.RandomCoordinate(x, GlobalData.Randomize) : x;
            y = (GlobalData.Randomize > 0) ? random.RandomCoordinate(y, GlobalData.Randomize) : y;

            //if landscape mode
            if (GlobalData.CustomX > GlobalData.CustomY)
            {
                var temp = y;
                y = x;
                x = GlobalData.CustomY - temp;
            }

            //convert to string
            xString = x.ToString();
            yString = y.ToString();

            return new Point(x, y);
        }
    }
}
