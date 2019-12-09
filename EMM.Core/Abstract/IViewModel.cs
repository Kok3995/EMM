using Data;
using System.ComponentModel;

namespace EMM.Core
{
    public interface IViewModel : INotifyPropertyChanged, IChangeTracking
    {
        /// <summary>
        /// True if the item is selected
        /// </summary>
        bool IsSelected { get; set; }
    }
}
