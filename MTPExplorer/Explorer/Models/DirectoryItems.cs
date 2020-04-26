using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTPExplorer
{
    public class DirectoryItems
    {
        /// <summary>
        /// Type of directory: file, folder, drive
        /// </summary>
        public DirectoryItemsType Type { get; set; }

        /// <summary>
        /// Name of the directory
        /// </summary>
        public string Name { get { return this.Type == DirectoryItemsType.Drive ? this.FullPath : DirectoryMethods.GetFileFolderName(FullPath); } set { } }

        /// <summary>
        /// The full path of the directory
        /// </summary>
        public string FullPath { get; set; }   
    }
}
