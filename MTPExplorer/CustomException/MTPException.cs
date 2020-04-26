using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTPExplorer
{
    public class MTPException : Exception
    {
        public MTPException(string message) : base(message)
        {
        }

        internal const string NOT_FOUND_ITEM = "The item does not exist";
        internal const string NOT_FOUND_PATH = "The path does not exist on the device";
        internal const string NOT_FOUND_DEVICE = "Device is not connected";
        internal const string INVALID_PATH = "The Path is invalid";
        internal const string ERROR_WRITE = "Errors occur while trying to write to the device";
        internal const string ERROR_READ = "Errors occur while trying to read from the device";
    }
}
