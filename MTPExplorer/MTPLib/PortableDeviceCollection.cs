using PortableDeviceApiLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTPExplorer
{
    /// <summary>
    /// Collection of all MTP connected devices
    /// </summary>
    public class PortableDeviceCollection : Collection<IPortableDevice>
    {
        public PortableDeviceCollection()
        {
            deviceManager = new PortableDeviceManagerClass(); 
        }

        private PortableDeviceManager deviceManager;

        /// <summary>
        /// Refresh MTP device list
        /// </summary>
        public virtual void Refresh()
        {
            deviceManager.RefreshDeviceList();
            Clear();

            uint count = 0;
            try { deviceManager.GetDevices(null, ref count); }
            catch (Exception ex) { Console.WriteLine(ex.Message); count = 0; }

            if (count == 0)
                return;

            var deviceIds = new string[count];
            deviceManager.GetDevices(deviceIds, ref count);

            foreach (var deviceId in deviceIds)
            {
                Add(new PortableDevice(deviceId));
            }
        }

        /// <summary>
        /// Get the device by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual IPortableDevice GetDeviceById(string id)
        {
            return Items.Where(p => p.DeviceId.Equals(id)).FirstOrDefault();
        }

        /// <summary>
        /// Get the device by its friendly name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual IPortableDevice GetDeviceByName(string name)
        {
            return Items.Where(p => p.FriendlyName.Equals(name)).FirstOrDefault();
        }
    }
}
