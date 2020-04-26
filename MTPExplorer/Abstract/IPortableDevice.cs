using System;
using System.Collections.Generic;
using System.IO;

namespace MTPExplorer
{
    public interface IPortableDevice
    {
        /// <summary>
        /// ID of the device
        /// </summary>
        string DeviceId { get; set; }

        /// <summary>
        /// Device's friendly name
        /// </summary>
        string FriendlyName { get; }

        /// <summary>
        /// Connect to the device
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnect the device
        /// </summary>
        void DisConnect();

        /// <summary>
        /// Get the contents of the device
        /// </summary>
        /// <returns></returns>
        PortableDeviceFolder GetContents();

        /// <summary>
        /// Get the contents of the device, specify the depth
        /// </summary>
        /// <param name="depth">The depth to look for contents</param>
        /// <returns></returns>
        PortableDeviceFolder GetContents(int? depth);

        /// <summary>
        /// Get the contents of the device, specify the depth
        /// </summary>
        /// <param name="depth">The depth to look for contents</param>
        /// <param name="FolderOnly">Search for folder only</param>
        /// <returns></returns>
        PortableDeviceFolder GetContents(int? depth, bool FolderOnly);

        /// <summary>
        /// Get all the files/folder in the folder with the parentObjectID
        /// </summary>
        /// <param name="parentObjectID"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        IEnumerable<PortableDeviceObject> GetContentsIn(string parentObjectID);

        /// <summary>
        /// Get a single content by its ObjectID
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        PortableDeviceObject GetContentByID(string id);

        /// <summary>
        /// Get the Content by its name in a specific parent folder, return null if not found
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentObjectID"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        PortableDeviceObject GetContentByNameIn(string name, string parentObjectID);

        /// <summary>
        /// Get the content by its path: mtp:\{FriendlyName}\{storage}\....
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <returns></returns>
        PortableDeviceObject GetContentByPath(string path);

        /// <summary>
        /// Get the device storage by its name
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <returns></returns>
        PortableDeviceObject GetStorageByName(string name);

        /// <summary>
        /// Transfer file to device
        /// </summary>
        /// <param name="sourcePath">Full Path to the file</param>
        /// <param name="parentFolderId">ID of the parent folder on the device</param>
        /// <returns></returns>
        void TransferToDevice(string sourcePath, string parentFolderId);

        /// <summary>
        /// Get a file from the device
        /// </summary>
        /// <param name="sourceID"></param>
        /// <param name="destinationPath"></param>
        void DownloadFile(string sourceID, string destinationPath);

        /// <summary>
        /// Create new folder in parent folder
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentFolderId"></param>
        PortableDeviceFolder NewFolder(string name, string parentFolderId);

        /// <summary>
        /// Delete a file from the device
        /// </summary>
        /// <param name="objectID"></param>
        void DeleteFile(string objectID, bool recursive = false);

        /// <summary>
        /// True if device support recursive delete
        /// </summary>
        /// <returns></returns>
        bool IsRecursiveDeleteSupported();
    }
}
