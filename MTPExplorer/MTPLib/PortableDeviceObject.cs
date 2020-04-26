using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTPExplorer
{
    public class PortableDeviceObject
    {
        public PortableDeviceObject(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public bool IsFolder => this.GetType() == typeof(PortableDeviceFolder);
    }
}
