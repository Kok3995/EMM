using Data;
using EMM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEMG_EX.Core
{
    public interface IAEActionViewModel : IViewModel, IAEAction, ICopy<IAEActionViewModel>
    {
        int ActionGroupIndex { get; set; }

        int ActionIndex { get; set; }

        /// <summary>
        /// Convert the viewmodel back to model for saving, Generate scripts
        /// </summary>
        /// <returns></returns>
        IAEAction ConvertBackToAction();

        /// <summary>
        /// Convert to <see cref="IAEActionViewModel"/> from <see cref="IAEAction"/>
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IAEActionViewModel ConvertFromAction(IAEAction action);

        /// <summary>
        /// Return list of action base on user choice
        /// </summary>
        /// <returns></returns>
        IList<IAction> UserChoicesToActionList();
    }
}
