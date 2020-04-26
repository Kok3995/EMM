using Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEMG_EX.Core
{
    public class AERepository : AEBaseRepository<IAEAction> ,IAERepository
    {
        public AERepository()
        {
            if (!File.Exists(AEMGStatic.SAVED_AEACTION_FILEPATH))
            {
                internalStorage = new List<IAEAction>();
                SaveChange();
            }
        }
    }

}
