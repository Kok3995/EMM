using System;
using System.Windows.Input;
using Data;
using EMM.Core.Converter;

namespace EMM.Core.ViewModels
{
    public class RecordedViewModel : BaseViewModel, IActionViewModel
    {
        public RecordedViewModel(SimpleAutoMapper autoMapper, ViewModelFactory viewModelFactory)
        {
            this.autoMapper = autoMapper;
            this.viewModelFactory = viewModelFactory;
        }

        private SimpleAutoMapper autoMapper;

        private ViewModelFactory viewModelFactory;

        public BasicAction BasicAction { get => BasicAction.Recorded; set { } }

        public string ActionDescription { get; set; } = "New Recorded";

        public string RecordedString { get; set; } = "Copy Recorded script here";

        public Emulator RecordedStringEmulator { get; set; }

        #region Commands

        #endregion

        #region Interface Implement

        public IAction ConvertBackToAction()
        {
            return this.autoMapper.SimpleAutoMap<RecordedViewModel, Recorded>(this);
        }

        public IActionViewModel ConvertFromAction(IAction action)
        {
            this.autoMapper.SimpleAutoMap<Recorded, RecordedViewModel>(action as Recorded, this);

            return this;
        }

        public IActionViewModel MakeCopy()
        {
            var newVM = this.viewModelFactory.NewActionViewModel(this.BasicAction);

            return newVM;
        }

        public IActionViewModel ChangeResolution(double scaleX, double scaleY, MidpointRounding roundMode = MidpointRounding.ToEven)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
