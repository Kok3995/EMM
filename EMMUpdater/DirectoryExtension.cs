using System.IO;

namespace EMMUpdater
{
    public static class DirectoryExtension
    {
        /// <summary>
        /// Copy the content of a directory
        /// </summary>
        /// <param name="source">The source DirectoryInfo</param>
        /// <param name="target">The target DirectoryInfo</param>
        public static void CopyAllTo(this DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            //Copy all the files the the main directory
            foreach(var fileInfo in source.GetFiles())
            {
                fileInfo.CopyTo(Path.Combine(target.FullName, fileInfo.Name), true);
            }

            //Copy all files in sub directory using recursive
            foreach(var subDirectoryInfo in source.GetDirectories())
            {
                var subTargetDirectory = target.CreateSubdirectory(subDirectoryInfo.Name);
                subDirectoryInfo.CopyAllTo(subTargetDirectory);
            }
        }
    }
}
