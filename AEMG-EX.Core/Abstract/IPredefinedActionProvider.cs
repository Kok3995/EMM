using Data;
using System.Collections.Generic;

namespace AEMG_EX.Core
{
    public interface IPredefinedActionProvider
    {
        /// <summary>
        /// Get the predefined action
        /// </summary>
        /// <returns></returns>
        DefaultAction GetDefaultActions();

        /// <summary>
        /// Save a new PredefinedAction
        /// </summary>
        /// <param name="predefineAction"></param>
        bool SaveDefaultActions(DefaultAction defaultAction);

        /// <summary>
        /// Return list of IAction
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IList<IAction> GetCharacterAction(Action action);

        /// <summary>
        /// Generate series of click base on aftime
        /// </summary>
        /// <param name="skill">The skill to spam</param>
        /// <param name="aftime">Time to spam</param>
        /// <returns></returns>
        IList<IAction> GetAFSpamClick(CharacterAction skill, int aftime);

        /// <summary>
        /// Return the click base on character skill
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        IList<IAction> GetSkillClick(CharacterAction skill);

        /// <summary>
        /// Get the wait action
        /// </summary>
        /// <param name="waittime"></param>
        /// <returns></returns>
        IAction GetWait(int waittime);
    }
}
