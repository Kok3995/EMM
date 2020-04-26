using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace AEMG_EX.Core
{
    public class AEWait : IAEAction
    {
        public string Name { get; set; }

        public AEAction AEAction { get; set; }

        /// <summary>
        /// Time to wait in milisecond
        /// </summary>
        public int WaitTime { get; set; }
    }
}
