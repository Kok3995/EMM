using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace EMM.Core
{
    public abstract class BaseScriptGenerateStragegy : IActionScriptGenerator
    {
        public BaseScriptGenerateStragegy(IScriptHelper helper)
        {
            this.helper = helper;
        }
        protected Random random = new Random();
        protected const int timeBetweenPoint = 12;
        protected readonly IScriptHelper helper;

        public abstract object ActionToScript(IAction action, ref int timer);
    }
}
