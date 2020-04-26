using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTPExplorer
{
    /// <summary>
    /// Manage MTP devices
    /// </summary>
    public class MTPManager : IMTPManager
    {
        public MTPManager()
        {
            portableDevices = new PortableDeviceCollection();
        }

        private PortableDeviceCollection portableDevices;

        private Dictionary<string, string> deviceNameAndID = new Dictionary<string, string>();

        public IMTPExplorer Open(string initialDirectory = null)
        {
            var vm = new StructureViewModel(this, initialDirectory);
            var dialog = new MTPExplorer()
            {
                DataContext = vm,
            };

            vm.FolderSelected -= dialog.OnSubmit;
            vm.FolderSelected += dialog.OnSubmit;

            dialog.ShowDialog();

            return dialog;
        }

        public List<PortableDeviceObject> GetContentsFromPath(string path)
        {
            var content = Exist(path);

            if (content == null)
                throw new MTPException(MTPException.NOT_FOUND_PATH);

            var device = GetDeviceFromPath(path);

            if (device == null)
                throw new MTPException(MTPException.NOT_FOUND_DEVICE);

            try
            {
                device.Connect();
                return device.GetContentsIn(content.Id).ToList();
            }
            catch
            {
                throw new MTPException(MTPException.NOT_FOUND_PATH);
            }
            finally
            {
                device.DisConnect();
            }
        }

        public IEnumerable<PortableDeviceFolder> GetDevices()
        {
            try
            {
                portableDevices.Refresh();

                ConnectAll();

                foreach (var device in portableDevices)
                    yield return new PortableDeviceFolder(device.DeviceId, device.FriendlyName);
            }
            finally
            {
                DisconnectAll();
            }
        }

        public virtual void ConnectAll()
        {
            foreach (var device in portableDevices)
                device.Connect();
        }

        public virtual void DisconnectAll()
        {
            foreach (var device in portableDevices)
                device.DisConnect();
        }

        public void CreateDirectory(string path)
        {
            if (!IsMTPPath(path))
                throw new MTPException(MTPException.INVALID_PATH);

            var device = GetDeviceFromPath(path);

            if (device == null)
                throw new MTPException(MTPException.NOT_FOUND_DEVICE);

            var pathArr = path.Split(Path.DirectorySeparatorChar);

            if (pathArr.Length == 3) //to storage only
                return;

            bool shouldCreate = false;

            try
            {
                device.Connect();

                PortableDeviceObject parent = device.GetStorageByName(pathArr[2]);
                PortableDeviceObject folder;
                for (int i = 3; i < pathArr.Length; i++)
                {
                    folder = device.GetContentByNameIn(pathArr[i], parent.Id);
                    if (folder == null)
                        shouldCreate = true;

                    if (shouldCreate)
                    {
                        folder = device.NewFolder(pathArr[i], parent.Id);
                    }

                    parent = folder;
                }
            }
            catch
            {
                throw new MTPException(MTPException.ERROR_WRITE);
            }
            finally
            {
                device.DisConnect();
            }
        }

        public PortableDeviceObject Exist(string path)
        {
            if (!IsMTPPath(path))
                return null;

            var device = GetDeviceFromPath(path);

            if (device == null)
                return null;

            PortableDeviceObject content;
            try
            {
                device.Connect();
                content = device.GetContentByPath(path);
            }
            catch
            {
                return null;
            }
            finally
            {
                device.DisConnect();
            }

            return content;
        }

        public bool IsExist(string path)
        {
            return Exist(path) == null ? false : true;
        }

        public bool IsFileExist(string path)
        {
            var content = Exist(path);
            return content == null ? false : !content.IsFolder;
        }

        public bool IsFolderExist(string path)
        {
            var content = Exist(path);
            return content == null ? false : content.IsFolder;
        }

        public bool IsMTPPath(string path)
        {
            return Helpers.IsMTPPathValid(path);
        }

        public void TransferFileToDevice(string sourcePath, string destinationFolder, string newFileName = null, bool overwrite = false)
        {
            if (!File.Exists(sourcePath) || !IsMTPPath(destinationFolder))
                throw new MTPException(MTPException.INVALID_PATH);

            var device = GetDeviceFromPath(destinationFolder);

            if (device == null)
                throw new MTPException(MTPException.NOT_FOUND_DEVICE);

            string tempFolder = null;
            var fileName = Path.GetFileName(sourcePath);
            if (newFileName != null && Helpers.IsValidFilename(newFileName))
            {
                fileName = newFileName;
                var tempSourcePath = CreateTempFilePath(fileName, out tempFolder);
                File.Copy(sourcePath, tempSourcePath, true);
                sourcePath = tempSourcePath;
            }

            try
            {
                device.Connect();

                var folder = device.GetContentByPath(destinationFolder);

                if (folder.IsFolder)
                {
                    var file = device.GetContentByNameIn(fileName, folder.Id);
                    var exist = file == null ? false : true;

                    if (!exist)
                    {
                        device.TransferToDevice(sourcePath, folder.Id);
                    }
                    else if (overwrite)
                    {
                        device.DeleteFile(file.Id);
                        device.TransferToDevice(sourcePath, folder.Id);
                    }
                }
                else
                    throw new MTPException(MTPException.INVALID_PATH);
            }
            catch
            {
                throw new MTPException(MTPException.ERROR_WRITE);
            }
            finally
            {
                if (tempFolder != null && Directory.Exists(tempFolder))
                    Directory.Delete(tempFolder, true);
                device.DisConnect();
            }
        }

        public void TransferFolderToDevice(string sourceFolder, string destinationFolder, bool recursive = false, bool overwrite = false)
        {
            throw new NotImplementedException();
        }

        public void WriteToDevice(string text, string fileName, string destinationFolder, bool overwrite = false)
        {
            if (text == null)
                throw new ArgumentNullException();

            if (!IsMTPPath(destinationFolder))
                throw new MTPException(MTPException.INVALID_PATH);

            var device = GetDeviceFromPath(destinationFolder);

            if (device == null)
                throw new MTPException(MTPException.NOT_FOUND_DEVICE);

            string tempFolder = null;

            try
            {
                device.Connect();

                var folder = device.GetContentByPath(destinationFolder);

                if (folder.IsFolder)
                {
                    var file = device.GetContentByNameIn(fileName, folder.Id);
                    var exist = file == null ? false : true;

                    var filePath = CreateTempFilePath(fileName, out tempFolder);
                    File.WriteAllText(filePath, text);

                    if (!exist)
                    {                      
                        device.TransferToDevice(filePath, folder.Id);
                    }
                    else if (overwrite)
                    {
                        device.DeleteFile(file.Id);
                        device.TransferToDevice(filePath, folder.Id);
                    }
                }
                else
                    throw new MTPException(MTPException.INVALID_PATH);
            }
            catch
            {
                throw new MTPException(MTPException.ERROR_WRITE);
            }
            finally
            {
                device.DisConnect();
                if (tempFolder != null && Directory.Exists(tempFolder))
                    Directory.Delete(tempFolder, true);
            }
        }

        public string ReadFromDevice(string path)
        {
            if (!IsMTPPath(path))
                throw new MTPException(MTPException.INVALID_PATH);

            var device = GetDeviceFromPath(path);

            if (device == null)
                throw new MTPException(MTPException.NOT_FOUND_DEVICE);

            string tempFolder = null;
            try
            {
                device.Connect();

                var file = device.GetContentByPath(path);

                if (!file.IsFolder)
                {
                    var filePath = CreateTempFilePath(file.Name, out tempFolder);

                    device.DownloadFile(file.Id, filePath);

                    return File.ReadAllText(filePath);
                }
                else
                    throw new MTPException(MTPException.INVALID_PATH);
            }
            catch
            {
                throw new MTPException(MTPException.ERROR_WRITE);
            }
            finally
            {
                device.DisConnect();
                if (tempFolder != null && Directory.Exists(tempFolder))
                    Directory.Delete(tempFolder, true);
            }
        }

        public void Delete(string path, bool recursive = false)
        {
            if (!IsMTPPath(path))
                throw new FileNotFoundException("Path is invalid");

            var device = GetDeviceFromPath(path);

            if (device == null)
                throw new FileNotFoundException("Can not find the device");

            if (!IsExist(path))
                throw new FileNotFoundException("The file/folder does not exist on the device");

            try
            {
                device.Connect();

                var content = device.GetContentByPath(path);

                if (!content.IsFolder) //If file
                {
                    device.DeleteFile(content.Id);
                    return;
                }

                var contents = device.GetContentsIn(content.Id).ToList();

                if (contents.Count == 0) //empty folder
                {
                    device.DeleteFile(content.Id);
                    return;
                }

                //Folder has items at this point
                if (!recursive)
                    throw new IOException();

                //Recursive is true
                var isRecursiveSupport = device.IsRecursiveDeleteSupported();

                if (isRecursiveSupport)
                    device.DeleteFile(content.Id, true);
                else //Fallback if recursive not supported
                {                  
                    var stack = new Stack<PortableDeviceObject>();
                    stack.Push(content); //The main folder

                    foreach (var item in contents)
                        stack.Push(item);

                    while (stack.Count > 0)
                    {
                        var currentObject = stack.Peek();

                        if (!currentObject.IsFolder)
                        {
                            device.DeleteFile(currentObject.Id);
                            stack.Pop();
                            continue;
                        }

                        var currentFolderContents = device.GetContentsIn(currentObject.Id).ToList();
                        if (currentFolderContents.Count == 0)
                        {
                            device.DeleteFile(currentObject.Id);
                            stack.Pop();
                            continue;
                        }

                        foreach (var itemInFolder in currentFolderContents)
                            stack.Push(itemInFolder);
                    }
                }                                      
            }
            finally
            {
                device.DisConnect();
            }
        }

        /// <summary>
        /// Get the device from path, return null if not found
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IPortableDevice GetDeviceFromPath(string path)
        {
            try
            {
                var deviceName = path.Split(Path.DirectorySeparatorChar)[1];
                portableDevices.Refresh();

                if (deviceNameAndID.ContainsKey(deviceName))
                {
                    return portableDevices.GetDeviceById(deviceNameAndID[deviceName]);
                }

                ConnectAll();

                var device = portableDevices.GetDeviceByName(deviceName);

                if (device != null)
                    deviceNameAndID.Add(device.FriendlyName, device.DeviceId);

                return device;
            }
            finally
            {
                DisconnectAll();
            }
        }

        private string CreateTempFilePath(string fileName, out string tempFolder)
        {
            var guid = Guid.NewGuid();
            tempFolder = Path.Combine(Environment.CurrentDirectory, guid.ToString());
            Directory.CreateDirectory(tempFolder);
            var filePath = Path.Combine(tempFolder, fileName);

            return filePath;
        }
    }
}
