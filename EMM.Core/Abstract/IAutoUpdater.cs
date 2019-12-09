using System.Threading.Tasks;

namespace EMM.Core
{
    /// <summary>
    /// An auto updater for the program
    /// </summary>
    public interface IAutoUpdater
    {
        /// <summary>
        /// Set to false if execute <see cref="CheckForUpdate"/> from command
        /// </summary>
        bool IsStartUp { get; set; }

        /// <summary>
        /// Check for update from internet
        /// </summary>
        /// <returns></returns>
        void CheckForUpdate();

        /// <summary>
        /// Update the app
        /// </summary>
        /// <returns></returns>
        void Update();
    }
}
