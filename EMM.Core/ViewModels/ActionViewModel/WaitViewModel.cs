using Data;
using EMM.Core.Converter;
using System;
using System.Windows;

namespace EMM.Core.ViewModels
{
    public class WaitViewModel : BaseViewModel, IActionViewModel
    {
        public WaitViewModel(SimpleAutoMapper autoMapper, ViewModelFactory viewModelFactory)
        {
            this.autoMapper = autoMapper;
            this.viewModelFactory = viewModelFactory;
        }

        private SimpleAutoMapper autoMapper;
        private ViewModelFactory viewModelFactory;


        #region Public Properties

        /// <summary>
        /// This is wait action
        /// </summary>
        public BasicAction BasicAction { get => BasicAction.Wait; set { } }

        /// <summary>
        /// Action's Description
        /// </summary>
        public string ActionDescription { get; set; } = "New Wait";

        /// <summary>
        /// Time to wait for
        /// </summary>
        public int WaitTime { get; set; }


        #endregion

        #region Interface Implement

        /// <summary>
        /// Convert the viewmodel back to model for saving, Generate scripts
        /// </summary>
        /// <returns></returns>
        public IAction ConvertBackToAction()
        {
            return this.autoMapper.SimpleAutoMap<WaitViewModel, Wait>(this);
        }

        public IActionViewModel ConvertFromAction(IAction action)
        {
            this.autoMapper.SimpleAutoMap<Wait, WaitViewModel>(action as Wait, this);
            return this;
        }

        public IActionViewModel MakeCopy()
        {
            var newWaitVM = (WaitViewModel)viewModelFactory.NewActionViewModel(this.BasicAction);
            this.autoMapper.SimpleAutoMap<WaitViewModel, WaitViewModel>(this, newWaitVM);
            return newWaitVM;
        }

        public IActionViewModel ChangeResolution(double scaleX, double scaleY, MidpointRounding roundMode = MidpointRounding.ToEven)
        {
            return this.MakeCopy();
        } 

        #endregion
    }
}
