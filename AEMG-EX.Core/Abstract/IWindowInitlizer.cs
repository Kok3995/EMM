using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEMG_EX.Core
{
    /// <summary>
    /// Open various window within the program
    /// </summary>
    public interface IWindowInitlizer
    {
        /// <summary>
        /// Fire when wanting to open the setting window
        /// </summary>
        event EventHandler SettingWindowOpen;

        /// <summary>
        /// Open the setting window
        /// <paramref name="datacontext"/>
        /// </summary>
        void OpenSettingWindow(object datacontext = null);

        /// <summary>
        /// Fire when wanting to close the setting window
        /// </summary>
        event EventHandler SettingWindowClose;

        /// <summary>
        /// Close the setting window
        /// <paramref name="datacontext"/>
        /// </summary>
        void CloseSettingWindow();
    }
}
