using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace AEMG_EX.Core
{
    public class Food : IAEAction
    {
        public string Name { get; set; }

        public AEAction AEAction { get; set; }

        public bool EatOrNotToEat { get; set; }
    }
}
