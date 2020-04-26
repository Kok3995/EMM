using EMM.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM.Core
{
    /// <summary>
    /// Manage Recent File
    /// </summary>
    public interface IRecentFile
    {
        ObservableCollection<RecentItem> GetRecentItems();

        void AddRecentItem(RecentItem item);

        void AddRecentItem(string path);

        void RemoveRecentItem(RecentItem item);

        void RemoveRecentItem(string path);

        void ClearRecent();
    }
}
