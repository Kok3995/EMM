using PortableDeviceApiLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace MTPExplorer
{
    /// <summary>
    /// Represent an MTP Device
    /// </summary>
    public class PortableDevice : IPortableDevice
    {
        public PortableDevice(string deviceId)
        {
            this.DeviceId = deviceId;
            this.device = new PortableDeviceClass();
            clientInfos = GetClientInfo();
        }

        private PortableDeviceClass device;

        private bool isConnected;
        private IPortableDeviceValues clientInfos;

        private string friendlyName = null;

        private char pathSeparator => Path.DirectorySeparatorChar;

        public string DeviceId { get; set; }

        public virtual string FriendlyName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(friendlyName))
                    return friendlyName;

                if (!isConnected)
                    throw new InvalidOperationException("Device is not connected");

                device.Content(out IPortableDeviceContent content);

                content.Properties(out IPortableDeviceProperties properties);

                properties.GetValues("DEVICE", null, out IPortableDeviceValues propValues);

                propValues.GetStringValue(ref GlobalVar.WPD_DEVICE_FRIENDLY_NAME, out friendlyName);

                return friendlyName;
            }
        }

        public virtual void Connect()
        {
            if (isConnected || string.IsNullOrWhiteSpace(DeviceId))
                return;

            device.Open(DeviceId, clientInfos);

            isConnected = true;
        }

        public virtual void DisConnect()
        {
            if (!isConnected)
                return;

            device.Close();

            isConnected = false;
        }

        public PortableDeviceFolder GetContents()
        {
            return GetContents(null, false);
        }

        public PortableDeviceFolder GetContents(int? depth)
        {
            return GetContents(depth, false);
        }

        public virtual PortableDeviceFolder GetContents(int? depth, bool folderOnly)
        {
            var root = new PortableDeviceFolder("DEVICE", FriendlyName);

            var queue = new Queue<PortableDeviceObject>();

            device.Content(out IPortableDeviceContent content);
            content.Properties(out IPortableDeviceProperties properties);

            try
            {
                EnumerateContents(ref content, ref properties, root, queue, depth, folderOnly);

                while (queue.Count > 0)
                {
                    var currentItem = queue.Dequeue();

                    if (currentItem is PortableDeviceFolder nextFolder)
                        EnumerateContents(ref content, ref properties, nextFolder, queue, --depth, folderOnly);
                }

                return root;
            }
            finally
            {
                Marshal.ReleaseComObject(content);
                Marshal.ReleaseComObject(properties);
            }
        }

        public virtual IEnumerable<PortableDeviceObject> GetContentsIn(string parentObjectID)
        {
            if (string.IsNullOrEmpty(parentObjectID))
                throw new ArgumentException();

            device.Content(out IPortableDeviceContent content);
            content.Properties(out IPortableDeviceProperties properties);

            content.EnumObjects(0, parentObjectID, null, out IEnumPortableDeviceObjectIDs objectIDs);
            try
            {
                uint fetched = 0;
                do
                {
                    objectIDs.Next(1, out string nextObjectIDs, ref fetched);

                    if (fetched > 0)
                    {
                        yield return WrapObject(properties, nextObjectIDs);
                    }
                } while (fetched > 0);
            }
            finally
            {
                Marshal.ReleaseComObject(content);
                Marshal.ReleaseComObject(properties);
                Marshal.ReleaseComObject(objectIDs);
            }
        }

        public virtual PortableDeviceObject GetContentByID(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException();

            device.Content(out IPortableDeviceContent content);
            content.Properties(out IPortableDeviceProperties properties);

            try
            {
                return WrapObject(properties, id);
            }
            finally
            {
                Marshal.ReleaseComObject(content);
                Marshal.ReleaseComObject(properties);
            }
        }

        public virtual PortableDeviceObject GetContentByNameIn(string name, string parentObjectID)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(parentObjectID) || !Helpers.IsValidFilename(name))
                throw new ArgumentException();

            return GetContentsIn(parentObjectID).Where(c => c.Name.Equals(name)).FirstOrDefault();
        }

        public virtual PortableDeviceObject GetContentByPath(string path)
        {
            //path = mtp:\{FriendlyName}\{storage}\...
            //         0       1             2       3
            if (!Helpers.IsMTPPathValid(path))
                throw new ArgumentException();

            var nameArr = path.Split(pathSeparator);

            if (nameArr.Length == 2 && nameArr[1].Equals(FriendlyName))
                return new PortableDeviceFolder("DEVICE", FriendlyName);

            var storageName = nameArr[2];
            var storage = GetStorageByName(storageName);

            if (nameArr.Length == 3)
                return storage;

            PortableDeviceObject parentObject = storage;
            PortableDeviceObject deviceObject = null;
            for (int i = 3; i < nameArr.Length; i++)
            {
                deviceObject = GetContentByNameIn(nameArr[i], parentObject.Id);
                parentObject = deviceObject ?? throw new FileNotFoundException(path + " Can not find the path");
            }

            return deviceObject;
        }

        public virtual PortableDeviceObject GetStorageByName(string name)
        {
            var root = GetContents(1, true); //Get all the storage

            if (root.Files.Count <= 0)
                throw new DirectoryNotFoundException("Can not find the device's storage");

            var storage = root.Files.Where(s => s.Name.Equals(name)).FirstOrDefault();

            if (storage == null)
                throw new DirectoryNotFoundException("Can not find the device's storage");

            return storage;
        }

        public virtual void TransferToDevice(string sourcePath, string parentFolderId)
        {
            if (!File.Exists(sourcePath))
                throw new FileNotFoundException(sourcePath);

            device.Content(out IPortableDeviceContent content);
            GetRequiredPropertiesForAllType(sourcePath, parentFolderId, out IPortableDeviceValues values);
            
            uint optimalBufferSize = 0;
            content.CreateObjectWithPropertiesAndData(values, out IStream stream, ref optimalBufferSize, null);
            
            var targetStream = (System.Runtime.InteropServices.ComTypes.IStream)stream;

            try
            {
                using (var sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
                {
                    var bytes = new byte[optimalBufferSize];
                    int bytesRead;
                    do
                    {
                        bytesRead = sourceStream.Read(bytes, 0, (int)optimalBufferSize);

                        targetStream.Write(bytes, bytesRead, IntPtr.Zero);
                    } while (bytesRead > 0);
                }
            }
            catch
            {
                throw new IOException("Can not write to the device");
            }
            finally
            {
                stream.Commit(0);
                Marshal.ReleaseComObject(stream);
                Marshal.ReleaseComObject(values);
                Marshal.ReleaseComObject(content);
            }
        }

        public virtual void DownloadFile(string sourceID, string destinationPath)
        {
            device.Content(out IPortableDeviceContent content);
            content.Transfer(out IPortableDeviceResources resources);

            uint optimalBufferSize = 0;
            resources.GetStream(sourceID, ref GlobalVar.WPD_RESOURCE_DEFAULT, 0, ref optimalBufferSize, out IStream stream);

            var sourceStream = (System.Runtime.InteropServices.ComTypes.IStream)stream;
            IntPtr bytesRead = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)));

            try
            {
                using (var targetStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
                {
                    var bytes = new byte[optimalBufferSize];
                    do
                    {
                        sourceStream.Read(bytes, (int)optimalBufferSize, bytesRead);

                        targetStream.Write(bytes, 0, Marshal.ReadInt32(bytesRead));
                    } while (Marshal.ReadInt32(bytesRead) > 0);
                }
            }
            catch
            {
                throw new IOException("Can not get the file from device");
            }
            finally
            {
                Marshal.FreeCoTaskMem(bytesRead);
                Marshal.ReleaseComObject(stream);
                Marshal.ReleaseComObject(resources);
                Marshal.ReleaseComObject(content);
            }
        }

        public virtual PortableDeviceFolder NewFolder(string name, string parentFolderId)
        {
            if (string.IsNullOrWhiteSpace(name) || !Helpers.IsValidFilename(name))
                throw new ArgumentException(name + " is not a valid folder name");

            device.Content(out IPortableDeviceContent content);
            GetRequiredPropertiesForFolderType(name, parentFolderId, out IPortableDeviceValues values);
            try
            {
                string folderID = null;
                content.CreateObjectWithPropertiesOnly(values, ref folderID);

                return new PortableDeviceFolder(folderID, name);
            }
            finally
            {
                Marshal.ReleaseComObject(values);
                Marshal.ReleaseComObject(content);
            }
        }

        public virtual void DeleteFile(string objectID, bool recursive = false)
        {
            if (string.IsNullOrWhiteSpace(objectID))
                throw new ArgumentException();

            device.Content(out IPortableDeviceContent content);

            ObjectIDToPropVariant(objectID, out tag_inner_PROPVARIANT tag);

            var collection = (IPortableDevicePropVariantCollection)new PortableDeviceTypesLib.PortableDevicePropVariantCollection();
            collection.Add(ref tag);

            try
            {
                content.Delete(recursive ? (uint)1 : 0, collection, null);
            }
            finally
            {
                Marshal.ReleaseComObject(content);
            }
        }

        public bool IsRecursiveDeleteSupported()
        {
            device.Capabilities(out IPortableDeviceCapabilities capabilities);

            capabilities.GetCommandOptions(ref GlobalVar.WPD_COMMAND_OBJECT_MANAGEMENT_DELETE_OBJECTS, out IPortableDeviceValues values);

            try
            {
                values.GetBoolValue(ref GlobalVar.WPD_OPTION_OBJECT_MANAGEMENT_RECURSIVE_DELETE_SUPPORTED, out int result);

                return result == 0 ? false : true;
            }
            finally
            {
                Marshal.ReleaseComObject(capabilities);
                Marshal.ReleaseComObject(values);
            }
        }


        #region Helpers

        private void EnumerateContents(ref IPortableDeviceContent content, ref IPortableDeviceProperties properties, PortableDeviceFolder parent, Queue<PortableDeviceObject> queue, int? depth, bool folderOnly = false)
        {
            content.EnumObjects(0, parent.Id, null, out IEnumPortableDeviceObjectIDs objectIDs);

            try
            {
                uint fetched = 0;
                do
                {
                    objectIDs.Next(1, out string nextObjectIDs, ref fetched);

                    if (fetched > 0)
                    {
                        var item = WrapObject(properties, nextObjectIDs);

                        if (!folderOnly || item.IsFolder)
                            parent.Files.Add(item);

                        if (depth == null || depth > 1)
                            queue.Enqueue(item);
                    }
                } while (fetched > 0);
            }
            finally
            {
                Marshal.ReleaseComObject(objectIDs);
            }
        }

        private PortableDeviceObject WrapObject(IPortableDeviceProperties properties, string objectID)
        {
            properties.GetSupportedProperties(objectID, out IPortableDeviceKeyCollection keys);

            properties.GetValues(objectID, keys, out IPortableDeviceValues values);

            try
            {
                // Get the type of the object
                Guid contentType = Guid.Empty;
                try { values.GetGuidValue(GlobalVar.WPD_OBJECT_CONTENT_TYPE, out contentType); }
                catch (Exception ex) { Debug.WriteLine(ex.Message); }

                string name = "";

                if (contentType == GlobalVar.WPD_CONTENT_TYPE_FOLDER_GUID || contentType == GlobalVar.WPD_CONTENT_TYPE_FUNCTIONAL_OBJECT_GUID)
                {
                    // Get the name of the folder/storage
                    try { values.GetStringValue(GlobalVar.WPD_OBJECT_NAME, out name); }
                    catch (Exception ex) { Debug.WriteLine(ex.Message); }

                    return new PortableDeviceFolder(objectID, name);
                }

                // Get the name of the file
                try { values.GetStringValue(GlobalVar.WPD_OBJECT_ORIGINAL_FILE_NAME, out name); }
                catch (Exception ex) { Debug.WriteLine(ex.Message); }

                return new PortableDeviceFile(objectID, name);
            }
            finally
            {
                Marshal.ReleaseComObject(keys);
                Marshal.ReleaseComObject(values);
            }
        }

        private void GetRequiredPropertiesForAllType(string sourcePath, string parentFolderId, out IPortableDeviceValues values)
        {
            values = (IPortableDeviceValues)new PortableDeviceTypesLib.PortableDeviceValues();

            values.SetStringValue(ref GlobalVar.WPD_OBJECT_NAME, Path.GetFileNameWithoutExtension(sourcePath)); //File name without extension
            values.SetStringValue(ref GlobalVar.WPD_OBJECT_ORIGINAL_FILE_NAME, Path.GetFileName(sourcePath)); //Full file name
            values.SetStringValue(ref GlobalVar.WPD_OBJECT_PARENT_ID, parentFolderId); //ParentID
            values.SetUnsignedLargeIntegerValue(ref GlobalVar.WPD_OBJECT_SIZE, (ulong)(new FileInfo(sourcePath).Length)); //File size
        }

        private void GetRequiredPropertiesForFolderType(string folderName, string parentFolderId, out IPortableDeviceValues values)
        {
            values = (IPortableDeviceValues)new PortableDeviceTypesLib.PortableDeviceValues();

            values.SetStringValue(ref GlobalVar.WPD_OBJECT_NAME, Path.GetFileNameWithoutExtension(folderName)); //Folder name
            values.SetStringValue(ref GlobalVar.WPD_OBJECT_PARENT_ID, parentFolderId); //ParentID
            values.SetGuidValue(ref GlobalVar.WPD_OBJECT_CONTENT_TYPE, ref GlobalVar.WPD_CONTENT_TYPE_FOLDER_GUID); //Folder type
        }

        private void ObjectIDToPropVariant(string objectID, out tag_inner_PROPVARIANT tag)
        {
            var values = (IPortableDeviceValues)new PortableDeviceTypesLib.PortableDeviceValues();

            values.SetStringValue(ref GlobalVar.WPD_OBJECT_ID, objectID);

            try
            {
                values.GetValue(ref GlobalVar.WPD_OBJECT_ID, out tag);
            }
            finally
            {
                Marshal.ReleaseComObject(values);
            }
        }

        private IPortableDeviceValues GetClientInfo()
        {
            var values = (IPortableDeviceValues)new PortableDeviceTypesLib.PortableDeviceValues();

            values.SetStringValue(ref GlobalVar.WPD_CLIENT_NAME, "MTPExplorer");
            values.SetUnsignedLargeIntegerValue(ref GlobalVar.WPD_CLIENT_MAJOR_VERSION, (uint)1);
            values.SetUnsignedLargeIntegerValue(ref GlobalVar.WPD_CLIENT_MINOR_VERSION, (uint)0);
            values.SetUnsignedLargeIntegerValue(ref GlobalVar.WPD_CLIENT_REVISION, (uint)0);

            return values;
        }

        #endregion
    }
}