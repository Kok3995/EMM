using EMM.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM.Core
{
    public static class MacroExtension
    {
        public static bool IsDirty(this MacroViewModel currentMacro)
        {
            if (currentMacro == null)
                return false;

            if (currentMacro.IsChanged)
                return currentMacro.IsChanged;

            var stack = new Stack<IChangeTracking>();

            foreach (var ag in currentMacro.ViewModelList)
            {
                stack.Push(ag);
            }

            bool isDirty = false;

            while (!isDirty && stack.Count > 0)
            {
                var current = stack.Pop();

                if (current.IsChanged)
                {
                    isDirty = true;
                    continue;
                }

                if (current is ActionGroupViewModel agvm)
                {
                    foreach (var a in agvm.ViewModelList)
                    {
                        stack.Push(a);
                    }
                }
            }

            return isDirty;
        }
    }
}
