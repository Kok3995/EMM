using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace MTPExplorer
{
    public static class DirectoryMethods
    {
        /// <summary>
        /// Get a <see cref="DirectoryItems"></see> list of logical drives of the machine
        /// </summary>
        /// <returns></returns>
        public static List<DirectoryItems> GetLogicalDrives()
        {
            return Directory.GetLogicalDrives().Select(drive => new DirectoryItems { FullPath = drive, Type = DirectoryItemsType.Drive }).ToList();
        }

        /// <summary>
        /// Get a <see cref="DirectoryItems"></see> list of files and folders from full path of directory
        /// </summary>
        /// <param name="fullPath">full path of the directory</param>
        /// <returns></returns>
        public static List<DirectoryItems> GetDirectoryContents(string fullPath)
        {
            //Create new empty list
            var items = new List<DirectoryItems>();

            #region Get Contents

            try
            {
                if (fullPath != null)
                    //Loop through each item in the directory to get the folder and files
                    items.AddRange(Directory.GetDirectories(fullPath).Select(f => new DirectoryItems { FullPath = f, Type = DirectoryItemsType.Folder }));
                    items.AddRange(Directory.GetFiles(fullPath).Select(files => new DirectoryItems { FullPath = files, Type = DirectoryItemsType.File }));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            #endregion

            return items;
        }

        #region Helper methods

        /// <summary>
        /// Get file and folders name
        /// </summary>
        /// <param name="directories">full path of file or folder you want the name of</param>
        /// <returns></returns>
        public static string GetFileFolderName(string directories)
        {
            string uniPath = directories.Replace('/', '\\');

            int lastIndex = uniPath.LastIndexOf('\\');

            if (lastIndex <= 0)
                return directories;

            return uniPath.Substring(lastIndex + 1);
        }

        #endregion
    }
}
