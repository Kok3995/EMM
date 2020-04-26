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
    public class NoxScriptApply : BaseScriptApply, IApplyScriptToFolder
    {
        public NoxScriptApply(IMessageBoxService messageBoxService, IMTPManager mTPManager) : base(mTPManager)
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

                var fileNameInRecordFolder = CalculateMD5Hash(DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString());
                var fullPathToMacroFile = Path.Combine(path, fileNameInRecordFolder); // path/md5string
                var recordsFileFullPath = Path.Combine(path, "records"); // path/records
                Dictionary<string, NoxRecordFormat> recordsFileContent = null;

                LoadRecord(recordsFileFullPath, ref recordsFileContent);                

                var isNameExisted = recordsFileContent.FirstOrDefault(i => i.Value.Name.Equals(scriptName)).Value;
                var key = recordsFileContent.FirstOrDefault(i => i.Value.Name.Equals(scriptName)).Key;

                if (isNameExisted == null)
                {
                    //File.WriteAllText(fullPathToMacroFile, script);
                    WriteAllText(fullPathToMacroFile, script);
                    recordsFileContent.Add(fileNameInRecordFolder, new NoxRecordFormat { Name = scriptName });
                }
                else
                {
                    MessageResult overwriteChoice = MessageResult.Yes;

                    if (prompt == true)
                        overwriteChoice = messageBoxService.ShowMessageBox("Macro's name already existed. Do you want to overwrite?", "Overwrite?", MessageButton.YesNo, MessageImage.Question);

                    if (overwriteChoice == MessageResult.Yes)
                    {
                        //File.WriteAllText(Path.Combine(path, key), script);
                        WriteAllText(Path.Combine(path, key), script);

                    }
                    else
                        return null; //user press No return null
                }

                //File.WriteAllText(recordsFileFullPath, JsonConvert.SerializeObject(recordsFileContent, Formatting.Indented));
                WriteAllText(recordsFileFullPath, JsonConvert.SerializeObject(recordsFileContent, Formatting.Indented));

                return true;
            }
            catch (Exception ex)
            {
                messageBoxService.ShowMessageBox(ex.Message + "\n" + "Something wrong. Cannot convert. Maybe try to delete the record file then try again");
                return false;
            }
        }

        private void LoadRecord(string record, ref Dictionary<string, NoxRecordFormat> recordContent)
        {
            if (!/*File.Exists(record)*/IsFileExist(record))
            {
                //(new FileInfo(record)).Directory.Create();
                CreateFolder(Path.GetDirectoryName(record));
                recordContent = new Dictionary<string, NoxRecordFormat>();
            }
            else
            {
                recordContent = JsonConvert.DeserializeObject<Dictionary<string, NoxRecordFormat>>(/*File.ReadAllText(record)*/ReadAllText(record));
            }
        }

        /// <summary>
        /// Calculate md5 hash from input string
        /// </summary>
        /// <param name="input">string to hash</param>
        /// <returns></returns>
        private string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }

    public class NoxRecordFormat
    {
        [JsonProperty("combination")]
        public string Combination { get; set; } = "false";

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("needShow")]
        public string NeedShow { get; set; } = "true";

        [JsonProperty("new")]
        public string New { get; set; } = "true";

        [JsonProperty("playSet")]
        public PlaySet PlaySet { get; set; } = new PlaySet();

        [JsonProperty("priority")]
        public string Priority { get; set; } = "99";

        [JsonProperty("time")]
        public string Time { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
    }

    public class PlaySet
    {
        [JsonProperty("accelerator")]
        public string Accelerator { get; set; } = "1";

        [JsonProperty("interval")]
        public string Interval { get; set; } = "0";

        [JsonProperty("mode")]
        public string Mode { get; set; } = "0";

        [JsonProperty("playOnStart")]
        public string PlayOnStart { get; set; } = "false";

        [JsonProperty("playSeconds")]
        public string PlaySeconds { get; set; } = "0#0#0";

        [JsonProperty("repeatTimes")]
        public string RepeatTimes { get; set; } = "1";

        [JsonProperty("restartPlayer")]
        public string RestartPlayer { get; set; } = "false";

        [JsonProperty("restartTime")]
        public string RestartTime { get; set; } = "60";
    }
}