using EMM;
using EMM.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AEMG_EX.Core
{
    public class AEActionListViewModel : BaseViewModel
    {
        public AEActionListViewModel(IMacroManager macroManager)
        {
            this.macroManager = macroManager;
           
            InitializeCommandAndEvents();
        }

        private readonly IMacroManager macroManager;

        private IAEActionViewModel copyAEAction;

        public ObservableCollection<IAEActionViewModel> AEActionList { get; set; }

        public IAEActionViewModel SelectedAEAction { get; set; }

        #region Commands

        public ICommand CopyCommand { get; set; }
        public ICommand ApplyCommand { get; set; }

        private void InitializeCommandAndEvents()
        {
            CopyCommand = new RelayCommand(p =>
            {
                this.copyAEAction = this.SelectedAEAction;
            }, p => SelectedAEAction != null);

            ApplyCommand = new RelayCommand(p =>
            {
                foreach (var action in AEActionList.GetSelectedElement())
                {
                    if (action.AEAction == copyAEAction.AEAction)
                        action.CopyOntoSelf(copyAEAction);
                };
            }, p => this.copyAEAction != null);

            this.macroManager.SelectChanged += (sender, e) =>
            {
                if (AEActionList == null)
                    AEActionList = new ObservableCollection<IAEActionViewModel>();

                AEActionList.Clear();

                foreach (var a in macroManager.GetCurrentAEActionList())
                {
                    AEActionList.Add(a);
                }

                this.copyAEAction = null;
            };
        }

        #endregion
    }
}
