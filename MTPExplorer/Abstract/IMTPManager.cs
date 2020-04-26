using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTPExplorer
{
    /// <summary>
    /// Manage MTP devices
    /// </summary>
    public interface IMTPManager
    {
        /// <summary>
        /// Get instance of IMTPExplorer
        /// </summary>
        /// <returns></returns>
        IMTPExplorer Open(string initialDirectory = null);

        /// <summary>
        /// Connect to all devices
        /// </summary>
        void ConnectAll();

        /// <summary>
        /// Disconnect all devices
        /// </summary>
        void DisconnectAll();

        /// <summary>
        /// Create a directory along the path
        /// </summary>
        /// <param name="path"></param>
        void CreateDirectory(string path);

        /// <summary>
        /// Get the list of all connected devices
        /// </summary>
        /// <returns></returns>
        IEnumerable<PortableDeviceFolder> GetDevices();

        /// <summary>
        /// Transfer a file to the devices
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationFolder"></param>
        /// <param name="newFileName"></param>
        /// <param name="overwrite"></param>
        void TransferFileToDevice(string sourcePath, string destinationFolder, string newFileName = null, bool overwrite = false);

        /// <summary>
        /// Transfer a folder to device
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="destinationFolder"></param>
        void TransferFolderToDevice(string sourceFolder, string destinationFolder, bool recursive = false, bool overwrite = false);

        /// <summary>
        /// Write text content to device
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fileName"></param>
        /// <param name="destinationFolder"></param>
        /// <param name="overwrite"></param>
        void WriteToDevice(string text, string fileName, string destinationFolder, bool overwrite = false);

        /// <summary>
        /// Read from device
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string ReadFromDevice(string path);

        /// <summary>
        /// Check if file/folder exist in given path and return it, otherwise null
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        PortableDeviceObject Exist(string path);

        /// <summary>
        /// Check if file/folder exist in given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool IsExist(string path);

        /// <summary>
        /// Check if exist and it must be a file
        /// </summary>
        /// <param name="path"></param>
        bool IsFileExist(string path);

        /// <summary>
        /// Check if exist and it must be a folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool IsFolderExist(string path);

        /// <summary>
        /// Check if path is valid MTP path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool IsMTPPath(string path);

        /// <summary>
        /// If file or empty folder then delete, if folder then delete every files in that folder, if recursive then delete everything include the parent folder
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recursive"></param>
        void Delete(string path, bool recursive = false);

        /// <summary>
        /// Get the content from specific path
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        List<PortableDeviceObject> GetContentsFromPath(string fullPath);
    }
}
