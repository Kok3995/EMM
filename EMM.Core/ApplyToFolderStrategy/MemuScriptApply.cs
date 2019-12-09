using System;
using System.IO;
using System.Text;

namespace EMM.Core
{
    /// <summary>
    /// This class is used to apply script to emulator folder
    /// </summary>
    public class MemuScriptApply : IApplyScriptToFolder
    {
        public MemuScriptApply(IMessageBoxService messageBoxService)
        {
            this.messageBoxService = messageBoxService;
        }

        private IMessageBoxService messageBoxService;

        public bool ApplyScriptTo(string scriptName, string path, StringBuilder script, bool prompt = true)
        {
            try
            {
                var currentTime = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
                var section = Uri.EscapeDataString(currentTime);

                var fullPath = Path.Combine(path, currentTime.Replace("-",string.Empty).Replace(":",string.Empty).Replace(" ", string.Empty) + ".mir");
                var record = Path.Combine(path, "info.ini");

                if (!File.Exists(record))
                {
                    (new FileInfo(record)).Directory.Create();
                    File.Create(record).Close();
                }

                File.WriteAllText(fullPath, script.ToString());

                StringBuilder iniFile = new StringBuilder(File.ReadAllText(record));
                iniFile.AppendLine().Append("[" + section + "]")
                    .AppendLine()
                    .Append("name=" + scriptName)
                    .AppendLine()
                    .Append("replayTime=0\nreplayCycles=1\nreplayAccelRates=1\nreplayInterval=0\ncycleInfinite=false\nbNew=false")
                    .AppendLine();

                File.WriteAllText(record, iniFile.ToString());

                return true;
            }
            catch (Exception ex)
            {
                messageBoxService.ShowMessageBox(ex.Message + "\n" + "Something wrong with the records file. Maybe delete it then try again");
                return false;
            }
        }
    }

}