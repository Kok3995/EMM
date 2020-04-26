using Data;

namespace EMM.Core
{
    public class AEStrategy : BaseScriptGenerateStragegy
    {
        public AEStrategy(IScriptHelper helper) : base(helper)
        {
        }

        public override object ActionToScript(IAction action, ref int timer)
        {
            return helper.CreateScriptObject();
        }
    }
}
