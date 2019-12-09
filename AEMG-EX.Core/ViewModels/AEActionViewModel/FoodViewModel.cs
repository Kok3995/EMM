using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Data;
using EMM;
using EMM.Core;

namespace AEMG_EX.Core
{
    public class FoodViewModel : BaseViewModel, IAEActionViewModel
    {
        #region Ctor

        public FoodViewModel(IPredefinedActionProvider actionProvider)
        {
            this.actionProvider = actionProvider;

            InitializeCommands();
        }

        private IPredefinedActionProvider actionProvider;

        #endregion

        #region View Properties

        public string ActionDescription { get; set; }

        public bool EatOrNotToEat { get; set; }

        #endregion

        public ICommand BackCommand { get; set; }

        public ICommand UseCommand { get; set; }

        private void InitializeCommands()
        {
            BackCommand = new RelayCommand(p =>
            {
                this.EatOrNotToEat = false;
            });

            UseCommand = new RelayCommand(p =>
            {
                this.EatOrNotToEat = true;
            });
        }


        #region Interface implement

        public int ActionGroupIndex { get; set; }

        public int ActionIndex { get; set; }

        public virtual AEAction AEAction => AEAction.FoodAD;

        public IAEAction ConvertBackToAction()
        {
            throw new System.NotImplementedException();
        }

        public IAEActionViewModel ConvertFromAction(IAEAction action)
        {
            throw new System.NotImplementedException();
        }

        public IAEActionViewModel Copy()
        {
            throw new System.NotImplementedException();
        }

        public virtual IList<IAction> UserChoicesToActionList()
        {
            if (!EatOrNotToEat)
                return new List<IAction>();

            return this.actionProvider.GetCharacterAction((AEAction == AEAction.FoodAD) ? Action.FoodAD : Action.ReFoodAD);
        }

        public void CopyOntoSelf(IAEActionViewModel source)
        {
            if (source.AEAction != AEAction)
                return;

            this.EatOrNotToEat = (source as FoodViewModel).EatOrNotToEat;
        }

        #endregion

    }
}
