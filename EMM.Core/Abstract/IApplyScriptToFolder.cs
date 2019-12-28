using Data;
using System.Text;

namespace EMM.Core
{
    /// <summary>
    /// Apply the converted script to emulator folder
    /// </summary>
    public interface IApplyScriptToFolder
    {
        bool? ApplyScriptTo(string scriptName, string path, StringBuilder script, bool prompt = true);
    }
}
