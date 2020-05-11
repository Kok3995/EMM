using Data;
using MTPExplorer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EMM.Core
{
    /// <summary>
    /// This class is used to apply script to emulator folder
    /// </summary>
    public class HiroMacroScriptApply : BaseScriptApply, IApplyScriptToFolder
    {
        public HiroMacroScriptApply(IMessageBoxService messageBoxService, IMTPManager mTPManager) : base(mTPManager)
        {
            this.messageBoxService = messageBoxService;
        }

        private IMessageBoxService messageBoxService;

        protected virtual string FileExtension => ".txt";

        public virtual bool? ApplyScriptTo(string scriptName, string path, object scriptObj, bool prompt = true)
        {
            try
            {
                if (!(scriptObj is string script))
                {
                    throw new InvalidOperationException("Passed in script object is not a string");
                }

                var fileNameInRecordFolder = scriptName + FileExtension;
                var fullPathToMacroFile = Path.Combine(path, fileNameInRecordFolder); // path/scriptname.txt

                //check existed
                bool isExisted = /*File.Exists(fullPathToMacroFile)*/IsFileExist(fullPathToMacroFile);

                if (!isExisted)
                {
                    //File.WriteAllText(fullPathToMacroFile, script);
                    WriteAllText(fullPathToMacroFile, script);
                }
                else
                {
                    MessageResult overwriteChoice = MessageResult.Yes;

                    if (prompt == true)
                        overwriteChoice = messageBoxService.ShowMessageBox("Macro's name already existed. Do you want to overwrite?", "Overwrite?", MessageButton.YesNo, MessageImage.Question);

                    if (overwriteChoice == MessageResult.Yes)
                    {
                        //File.WriteAllText(fullPathToMacroFile, script);
                        WriteAllText(fullPathToMacroFile, script);
                    }
                    else
                        return null; //user press No return null
                }

                return true;
            }
            catch (Exception ex)
            {
                messageBoxService.ShowMessageBox(ex.Message + "\n" + "Something wrong. Cannot convert. Maybe try to delete the record file then try again");
                return false;
            }
        }
    }
}