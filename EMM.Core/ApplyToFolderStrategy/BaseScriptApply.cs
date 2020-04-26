using MTPExplorer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM.Core
{
    /// <summary>
    /// Base class for script apply
    /// </summary>
    public class BaseScriptApply
    {
        public BaseScriptApply(IMTPManager mTPManager)
        {
            this.mTPManager = mTPManager;
        }

        private IMTPManager mTPManager;

        protected bool IsFileExist(string path)
        {
            if (mTPManager.IsMTPPath(path))
                return mTPManager.IsFileExist(path);

            return File.Exists(path);
        }

        protected bool IsDirectoryExist(string path)
        {
            if (mTPManager.IsMTPPath(path))
                return mTPManager.IsFolderExist(path);

            return Directory.Exists(path);
        }

        protected void CreateFolder(string path)
        {
            if (mTPManager.IsMTPPath(path))
                mTPManager.CreateDirectory(path);
            else
                Directory.CreateDirectory(path);
        }

        protected void WriteAllText(string path, string contents)
        {
            if (mTPManager.IsMTPPath(path))
            {
                var fileName = Path.GetFileName(path);
                var folder = Path.GetDirectoryName(path);

                mTPManager.WriteToDevice(contents, fileName, folder, true);
            }
            else
                File.WriteAllText(path, contents);
        }

        protected string ReadAllText(string path)
        {
            if (mTPManager.IsMTPPath(path))
                return mTPManager.ReadFromDevice(path);

            return File.ReadAllText(path);
        }

        protected string[] ReadAllLines(string path)
        {
            return ReadAllText(path).Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        protected void Copy(string sourceFilePath, string destinationFilePath, bool overwrite)
        {
            if (mTPManager.IsMTPPath(destinationFilePath))
                mTPManager.TransferFileToDevice(sourceFilePath, Path.GetDirectoryName(destinationFilePath), Path.GetFileName(destinationFilePath), overwrite);
            else
                File.Copy(sourceFilePath, destinationFilePath, overwrite);
        }
    }
}
