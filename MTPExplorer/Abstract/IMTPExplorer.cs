using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTPExplorer
{
    public interface IMTPExplorer
    {
        /// <summary>
        /// Dialog result
        /// </summary>
        bool? Result { get; }

        /// <summary>
        /// User selected path, set this to open the browser at this location
        /// </summary>
        string SelectedPath { get; set; }
    }
}
