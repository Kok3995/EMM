using System;
using System.IO;
using System.Linq;
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

        public bool? ApplyScriptTo(string scriptName, string path, StringBuilder script, bool prompt = true)
        {
            try
            {
                var currentTime = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
                var section = Uri.EscapeDataString(currentTime);

                var fullPath = Path.Combine(path, currentTime.Replace("-",string.Empty).Replace(":",string.Empty).Replace(" ", string.Empty) + ".mir");
                var record = Path.Combine(path, "info.ini");

                if (!File.Exists(record))
                {
                    Directory.CreateDirectory(path);
                    File.Create(record).Close();
                }

                //Read the file into a string array
                var iniFileStringList = File.ReadAllLines(record).ToList();

                if (iniFileStringList.Count > 0)
                {
                    //check name exist
                    var name = iniFileStringList.Where(m => m.Equals("name=" + scriptName)).FirstOrDefault();

                    if (name != null) //name exist
                    {
                        //ask overwrite
                        MessageResult overwriteChoice = MessageResult.Yes;

                        if (prompt == true)
                            overwriteChoice = messageBoxService.ShowMessageBox("Macro's name already existed. Do you want to overwrite?", "Overwrite?", MessageButton.YesNo, MessageImage.Question);

                        if (overwriteChoice == MessageResult.Yes)
                        {
                            //get the index of file name part in ini
                            var indexOfFileName = iniFileStringList.IndexOf(name) - 1;

                            //convert it to filename
                            var filenameOnDisk = iniFileStringList[indexOfFileName]
                                .Replace("-", string.Empty)
                                .Replace("%20", string.Empty).Replace("%3A", string.Empty)
                                .Replace("[", string.Empty).Replace("]", string.Empty) + ".mir";

                            //overwrite it
                            File.WriteAllText(Path.Combine(path, filenameOnDisk), script.ToString());
                            return true;
                        }
                        else
                            return null;
                    } 
                };

                //Name not exist then just write to disk
                File.WriteAllText(fullPath, script.ToString());

                //Reconstruct the ini file
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