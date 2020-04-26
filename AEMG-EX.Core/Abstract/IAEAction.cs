using Data;
using System.Collections.Generic;
using System.ComponentModel;

namespace AEMG_EX.Core
{
    public interface IAEAction
    {
        string Name { get; set; }

        /// <summary>
        /// Another Eden specific action
        /// </summary>
        AEAction AEAction { get; set; }
    }
}
