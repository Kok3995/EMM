using System.Collections.Generic;
using Data;
using EMM.Core;

namespace AEMG_EX.Core
{
    public class WaitViewModel : BaseViewModel, IAEActionViewModel
    {
        #region Ctor

        public WaitViewModel(IPredefinedActionProvider actionProvider)
        {
            this.actionProvider = actionProvider;
        }

        private IPredefinedActionProvider actionProvider;
        #endregion

        #region View Properties

        /// <summary>
        /// Time to wait in milisecond
        /// </summary>
        public int WaitTime { get; set; }

        #endregion

        #region Interface implement

        public AEAction AEAction => AEAction.Wait;

        public int ActionGroupIndex { get; set; }
        public int ActionIndex { get; set; }
        public string ActionDescription { get; set; }

        public IAEAction ConvertBackToAction()
        {
            throw new System.NotImplementedException();
        }

        public IAEActionViewModel ConvertFromAction(IAEAction action)
        {
            throw new System.NotImplementedException();
        }

        public IList<IAction> UserChoicesToActionList()
        {
            return new List<IAction> { this.actionProvider.GetWait(this.WaitTime) };
        }

        public void CopyOntoSelf(IAEActionViewModel source)
        {
            if (source.AEAction != AEAction || source.Equals(this))
                return;

            var sourceObject = source as WaitViewModel;

            this.WaitTime = sourceObject.WaitTime;
        }

        #endregion

    }
}
