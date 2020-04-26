using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEMG_EX.Core
{
    /// <summary>
    /// Saved setup of a macro
    /// </summary>
    public class SavedAESetup
    {
        public SavedAESetup()
        {
        }

        [JsonIgnore]
        public string MacroFileName { get; set; }

        public List<SavedAEProfile> Profiles { get; set; }

        [JsonIgnore]
        public SavedAEProfile DefaultProfile => Profiles?.Where(p => p.IsDefault).FirstOrDefault();
    }
}
