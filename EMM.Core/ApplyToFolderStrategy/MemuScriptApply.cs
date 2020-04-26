using MTPExplorer;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace EMM.Core
{
    /// <summary>
    /// This class is used to apply script to emulator folder
    /// </summary>
    public class MemuScriptApply : BaseScriptApply, IApplyScriptToFolder
    {
        public MemuScriptApply(IMessageBoxService messageBoxService, IMTPManager mTPManager) : base(mTPManager)
        {
            this.messageBoxService = messageBoxService;
        }

        private IMessageBoxService messageBoxService;

        public bool? ApplyScriptTo(string scriptName, string path, object scriptObj, bool prompt = true)
        {
            try
            {
                var script = scriptObj as string;

                if (script == null)
                {
                    throw new InvalidOperationException("Passed in script object is not a string");
                }

                var currentTime = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
                var section = Uri.EscapeDataString(currentTime);

                var fullPath = Path.Combine(path, currentTime.Replace("-",string.Empty).Replace(":",string.Empty).Replace(" ", string.Empty) + ".mir");
                var record = Path.Combine(path, "info.ini");

                //Directory.CreateDirectory(path);
                CreateFolder(path);
                StringBuilder iniFile;
                if (IsFileExist(record))
                {
                    //Read the file into a string array
                    var iniFileStringList = /*File.ReadAllLines(record).ToList()*/ReadAllLines(record).ToList();

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
                            //File.WriteAllText(Path.Combine(path, filenameOnDisk), script);
                            WriteAllText(Path.Combine(path, filenameOnDisk), script);
                            return true;
                        }
                        else
                            return null;
                    }

                    iniFile = new StringBuilder(/*File.ReadAllText(record)*/ReadAllText(record));
                }
                else 
                {
                    iniFile = new StringBuilder();
                };

                //Name not exist then just write to disk
                //File.WriteAllText(fullPath, script);
                WriteAllText(fullPath, script);

                //Reconstruct the ini file
                iniFile.AppendLine().Append("[" + section + "]")
                    .AppendLine()
                    .Append("name=" + scriptName)
                    .AppendLine()
                    .Append("replayTime=0\nreplayCycles=1\nreplayAccelRates=1\nreplayInterval=0\ncycleInfinite=false\nbNew=false")
                    .AppendLine();

                //File.WriteAllText(record, iniFile.ToString());
                WriteAllText(record, iniFile.ToString());

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