using Data;
using EMM.Core.Converter;
using System;

namespace EMM.Core
{
    public interface IActionViewModel : IViewModel, IResolutionChange
    {
        BasicAction BasicAction { get; set; }

        /// <summary>
        /// Convert the viewmodel back to model for saving, Generate scripts
        /// </summary>
        /// <returns></returns>
        IAction ConvertBackToAction();

        /// <summary>
        /// Return copy of itself
        /// </summary>
        /// <returns></returns>
        IActionViewModel MakeCopy();

        /// <summary>
        /// Convert to <see cref="IActionViewModel"/> from <see cref="IAction"/>
        /// </summary>
        /// <param name="autoMapper"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        IActionViewModel ConvertFromAction(IAction action);
    }
}
