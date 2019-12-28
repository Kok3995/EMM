using Data;
using EMM.Core.Converter;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace EMM.Core.ViewModels
{
    public class AEViewModel : BaseViewModel, IActionViewModel
    {
        #region Public Properties
        public AEViewModel(SimpleAutoMapper autoMapper, ViewModelFactory viewModelFactory)
        {
            this.autoMapper = autoMapper;
            this.viewModelFactory = viewModelFactory;
        }

        private SimpleAutoMapper autoMapper;
        private ViewModelFactory viewModelFactory;

        /// <summary>
        /// This is Another Eden specific Action
        /// </summary>
        public BasicAction BasicAction { get => BasicAction.AE; set { } }

        /// <summary>
        /// Action's Description
        /// </summary>
        public string ActionDescription { get; set; } = "Another Eden";

        /// <summary>
        /// Another Eden specific action
        /// </summary>
        public AEAction AnotherEdenAction { get; set; }

        /// <summary>
        /// AEAction list
        /// </summary>
        public ObservableCollection<AEAction> AEOptionList { get; } = new ObservableCollection<AEAction>(Enum.GetValues(typeof(AEAction)).Cast<AEAction>().ToList());

        /// <summary>
        /// Repeat the action. Use for Battle
        /// </summary>
        public int Repeat { get; set; } = 1;
        
        /// <summary>
        /// Convert the viewmodel back to model for saving, Generate scripts
        /// </summary>
        /// <returns></returns>
        public IAction ConvertBackToAction()
        {
            return this.autoMapper.SimpleAutoMap<AEViewModel, AE>(this);
        }

        public IActionViewModel ConvertFromAction(IAction action)
        {
            this.autoMapper.SimpleAutoMap<AE, AEViewModel>(action as AE, this);
            return this;
        }

        public IActionViewModel MakeCopy()
        {
            var newAE = (AEViewModel)viewModelFactory.NewActionViewModel(this.BasicAction);
            this.autoMapper.SimpleAutoMap<AEViewModel, AEViewModel>(this, newAE);
            return newAE;
        }

        public IActionViewModel ChangeResolution(double scaleX, double scaleY, MidpointRounding roundMode = MidpointRounding.ToEven)
        {
            return this.MakeCopy();
        }

        #endregion
    }
}
