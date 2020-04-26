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
    public class RobotmonScriptApply : BaseScriptApply, IApplyScriptToFolder
    {
        public RobotmonScriptApply(IMessageBoxService messageBoxService, IMTPManager mTPManager) : base(mTPManager)
        {
            this.messageBoxService = messageBoxService;
        }

        private IMessageBoxService messageBoxService;
        private const string HTML_FILENAME = "index.html";
        private const string JS_FILENAME = "index.js";
        private string indexHtmlFilePath = Path.Combine(Environment.CurrentDirectory, "Setting", "RobotmonIndexHTML");
        private string indexJsFilePath = Path.Combine(Environment.CurrentDirectory, "Setting", "RobotmonIndexJS");
        private const string scriptArr = @"var script = [";

        public bool? ApplyScriptTo(string scriptName, string path, object scriptObj, bool prompt = true)
        {
            try
            {
                if (!(scriptObj is List<string> script))
                {
                    throw new InvalidOperationException("Passed in script object is not List<string>");
                }

                var fileNameInRecordFolder = scriptName;
                var fullPathToMacroFile = Path.Combine(path, fileNameInRecordFolder); // path/scriptname/

                //check existed
                bool isExisted = /*Directory.Exists(fullPathToMacroFile)*/IsDirectoryExist(fullPathToMacroFile);

                if (!isExisted)
                {
                    //Directory.CreateDirectory(fullPathToMacroFile);
                    CreateFolder(fullPathToMacroFile);
                    WriteToDisk(script, fullPathToMacroFile);
                }
                else
                {
                    MessageResult overwriteChoice = MessageResult.Yes;

                    if (prompt == true)
                        overwriteChoice = messageBoxService.ShowMessageBox("Macro's name already existed. Do you want to overwrite?", "Overwrite?", MessageButton.YesNo, MessageImage.Question);

                    if (overwriteChoice == MessageResult.Yes)
                    {
                        WriteToDisk(script, fullPathToMacroFile);
                    }
                    else
                        return null; //user press No return null
                }

                return true;
            }
            catch (Exception ex)
            {
                messageBoxService.ShowMessageBox(ex.Message + "\n" + "Something wrong. Cannot convert. Maybe try to delete the " + scriptName + " folder then try again");
                return false;
            }
        }

        private void WriteToDisk(List<string> script, string folderPath)
        {
            //File.Copy(indexHtmlFilePath, Path.Combine(folderPath, HTML_FILENAME), true);
            Copy(indexHtmlFilePath, Path.Combine(folderPath, HTML_FILENAME), true);
            var jsScript = scriptArr + string.Join(",", script) + /*File.ReadAllText(indexJsFilePath)*/ReadAllText(indexJsFilePath);
            //File.WriteAllText(Path.Combine(folderPath, JS_FILENAME), jsScript);
            WriteAllText(Path.Combine(folderPath, JS_FILENAME), jsScript);
        }
    }
}