using Data;
using EMM.Core.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM.Core.ViewModels
{
    /// <summary>
    /// A ViewModel Factory
    /// </summary>
    public class ViewModelFactory
    {
        public ViewModelFactory(Dictionary<Type, object> dependencyDict)
        {
            this.dependencyDict = dependencyDict;
        }

        private SimpleAutoMapper autoMapper = new SimpleAutoMapper();

        private Dictionary<Type, object> dependencyDict;

        public MacroViewModel NewMacroViewModel()
        {
            return new MacroViewModel(autoMapper, this);
        }

        public IActionViewModel NewActionViewModel(BasicAction actionType)
        {
            try
            {
                return (IActionViewModel)Activator.CreateInstance(Type.GetType("EMM.Core.ViewModels." + Enum.GetName(typeof(BasicAction), actionType) + "ViewModel"), this.GetDependency<SimpleAutoMapper>(), this);
            }
            catch
            {
                throw new NotImplementedException("Cannot create an instance of this viewmodel. Maybe it's not yet implemented");
            }
        }

        private T GetDependency<T>() where T : class
        {
            if (!dependencyDict.ContainsKey(typeof(T)))
            {
                throw new NotImplementedException("The dependency has not been registed");
            }

            return (T)dependencyDict[typeof(T)];
        }
    }
}
