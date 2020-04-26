using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EMM.Core.ViewModels
{
    public class RecentFileManager : BaseViewModel, IRecentFile
    {
        public RecentFileManager()
        {
            LoadRecentItems();

            BindingOperations.EnableCollectionSynchronization(recentItems, _lock);

            recentItems.CollectionChanged += (sender, e) => SaveRecentItems();
        }

        private const int MAX = 20;
        private string recentPath = Path.Combine(Environment.CurrentDirectory, "Setting", "Recent");
        private ObservableCollection<RecentItem> recentItems;
        private object _lock = new object();

        public ObservableCollection<RecentItem> GetRecentItems()
        {
            return recentItems;
        }

        public void AddRecentItem(RecentItem item)
        {
            if (recentItems == null)
                throw new InvalidOperationException();

            var existed = recentItems.Where(r => r.Path.Equals(item.Path)).FirstOrDefault();

            if (existed != null)
            {
                lock (_lock)
                    recentItems.Move(recentItems.IndexOf(existed), 0);

                return;
            }

            if (recentItems.Count >= MAX)
            {
                lock (_lock)
                    recentItems.RemoveAt(recentItems.Count - 1);
            }

            lock(_lock)
                recentItems.Insert(0, item);
        }

        public void AddRecentItem(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
                AddRecentItem(new RecentItem(path));
        }

        public void RemoveRecentItem(RecentItem item)
        {
            lock (_lock)
                recentItems.Remove(item);
        }

        public void RemoveRecentItem(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
                recentItems.Remove(recentItems.Where(r => r.Path.Equals(path)).FirstOrDefault());
        }

        public void ClearRecent()
        {
            if (recentItems != null && recentItems.Count > 0)
            {
                lock (_lock)
                    recentItems.Clear();
            }
        }

        private void LoadRecentItems()
        {
            try
            {
                if (File.Exists(recentPath))
                {
                    var temp = JsonConvert.DeserializeObject<List<RecentItem>>(File.ReadAllText(recentPath));
                    recentItems = new ObservableCollection<RecentItem>(temp.Where(r => File.Exists(r.Path)));
                }
                else
                    recentItems = new ObservableCollection<RecentItem>();
            }
            catch
            {
                recentItems = new ObservableCollection<RecentItem>();
            }
        }

        private void SaveRecentItems()
        {
            try
            {
                File.WriteAllText(recentPath, JsonConvert.SerializeObject(recentItems, Formatting.Indented));
            }
            catch
            {
                //TO DO: ADD HANDLE HERE
            }
        }
    }

    public class RecentItem
    {
        public RecentItem(string path)
        {
            this.Path = path;
        }

        public string Path { get; set; }

        [JsonIgnore]
        public string FileName => new DirectoryInfo(System.IO.Path.GetDirectoryName(Path)).Name + "\\" + Path.Split('\\').LastOrDefault();
    }
}
