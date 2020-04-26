using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEMG_EX.Core
{
    public class WindowInit : IWindowInitlizer
    {
        public event EventHandler SettingWindowOpen;
        public event EventHandler SettingWindowClose;

        public void CloseSettingWindow()
        {
            SettingWindowClose?.Invoke(this, EventArgs.Empty);
        }

        public void OpenSettingWindow(object datacontext = null)
        {
            SettingWindowOpen?.Invoke(this, new WindowInitEventArgs(datacontext));
        }
    }

    public class WindowInitEventArgs: EventArgs
    {
        public WindowInitEventArgs(object datacontext)
        {
            DataContext = datacontext;
        }

        public object DataContext { get; set; }
    }   
}
