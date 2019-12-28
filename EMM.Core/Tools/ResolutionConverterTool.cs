using EMM.Core.Converter;
using EMM.Core.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace EMM.Core.Tools
{
    /// <summary>
    /// Convert resolution of the macro
    /// </summary>
    public class ResolutionConverterTool
    {
        public ResolutionConverterTool(ViewModelFactory modelFactory)
        {
            this.modelFactory = modelFactory;
        }

        private ViewModelFactory modelFactory;

        /// <summary>
        /// Convert the resolution of the passed in macro
        /// </summary>
        /// <param name="oldMacro">The macro to convert</param>
        /// <param name="newX">new resolution in the X axis, can not be 0</param>
        /// <param name="newY">new resolution in the Y axis, can not be 0</param>
        /// <returns>return null if can not convert</returns>
        public MacroViewModel ConvertResolution(MacroViewModel oldMacro, int newX, int newY, MidpointRounding roundMode = MidpointRounding.ToEven)
        {
            //check for invalid input
            if (oldMacro == null || newX <= 0 || newY <= 0)
                return null;

            //make copy
            var newMacro = modelFactory.NewMacroViewModel();
            newMacro.PopulateProperties(oldMacro.ConvertBack());

            //convert
            double scaleX = (double)newX / oldMacro.OriginalX;
            double scaleY = (double)newY / oldMacro.OriginalY;

            newMacro.ViewModelList = new ObservableCollection<IActionViewModel>(oldMacro.ViewModelList.Select(i => i.ChangeResolution(scaleX, scaleY, roundMode)));

            return newMacro;
        }
    }
}
