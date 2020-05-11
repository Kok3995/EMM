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
        public BaseScriptGenerateStragegy()
        {

        }

        public BaseScriptGenerateStragegy(IScriptHelper helper)
        {
            this.helper = helper;
        }
        protected Random random = new Random();
        protected const int timeBetweenPoint = 12;
        protected readonly IScriptHelper helper;

        public abstract object ActionToScript(IAction action, ref int timer);

        protected int GetInBetweenSwipeStep(sbyte step)
        {
            int result;
            if (GlobalData.Emulator == Emulator.LDPlayer)
            {
                result = (int)(step * (Math.Sqrt(Math.Pow(19200, 2) + Math.Pow(10800, 2)) / Math.Sqrt(Math.Pow(1280, 2) + Math.Pow(720, 2))));
            }
            else
                result = (int)(step * (Math.Sqrt(Math.Pow(GlobalData.CustomX, 2) + Math.Pow(GlobalData.CustomY, 2)) / Math.Sqrt(Math.Pow(1280, 2) + Math.Pow(720, 2))));

            return result <= 0 ? 20 : result;
        }
    }
}
