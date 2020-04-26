using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEMG_EX.Core
{
    /// <summary>
    /// Saved setup
    /// </summary>
    public class SavedAEProfile : INotifyPropertyChanged
    {
        public SavedAEProfile()
        {
            Setups = new List<IAEAction>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public List<IAEAction> Setups { get; set; }

        public bool IsDefault { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }
}
