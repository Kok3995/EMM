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
    public class LDPlayerScriptApply : BaseScriptApply, IApplyScriptToFolder
    {
        public LDPlayerScriptApply(IMessageBoxService messageBoxService, IMTPManager mTPManager) : base(mTPManager)
        {
            this.messageBoxService = messageBoxService;
        }

        private IMessageBoxService messageBoxService;

        public bool? ApplyScriptTo(string scriptName, string path, object scriptObj, bool prompt = true)
        {
            try
            {
                if (!(scriptObj is List<LDPlayerOperation> script))
                {
                    throw new InvalidOperationException("Passed in script object is not a list of LDPlayerOperation");
                }

                var fileNameInRecordFolder = scriptName + ".record";
                var fullPathToMacroFile = Path.Combine(path, fileNameInRecordFolder); // path/scriptname.record
                var ldplayerFileJson = new LDPlayerMacroFormat((script.LastOrDefault()?.Timing ?? 0), script);

                //check existed
                bool isExisted = /*File.Exists(fullPathToMacroFile)*/IsFileExist(fullPathToMacroFile);

                if (!isExisted)
                {
                    //File.WriteAllText(fullPathToMacroFile, JsonConvert.SerializeObject(ldplayerFileJson, Formatting.Indented));
                    WriteAllText(fullPathToMacroFile, JsonConvert.SerializeObject(ldplayerFileJson, Formatting.Indented));
                }
                else
                {
                    MessageResult overwriteChoice = MessageResult.Yes;

                    if (prompt == true)
                        overwriteChoice = messageBoxService.ShowMessageBox("Macro's name already existed. Do you want to overwrite?", "Overwrite?", MessageButton.YesNo, MessageImage.Question);

                    if (overwriteChoice == MessageResult.Yes)
                    {
                        //File.WriteAllText(fullPathToMacroFile, JsonConvert.SerializeObject(ldplayerFileJson, Formatting.Indented));
                        WriteAllText(fullPathToMacroFile, JsonConvert.SerializeObject(ldplayerFileJson, Formatting.Indented));
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

        public class LDPlayerMacroFormat
        {
            public LDPlayerMacroFormat(int maxTime, List<LDPlayerOperation> operations)
            {
                this.RecordInfo = new LDPLayerRecordInfo { CircleDuration = maxTime };
                this.Operations = operations;
            }

            [JsonProperty("operations")]
            public List<LDPlayerOperation> Operations { get; set; }

            [JsonProperty("recordInfo")]
            public LDPLayerRecordInfo RecordInfo { get; set; }
        }

        public class LDPLayerRecordInfo
        {
            [JsonProperty("loopType")]
            public int LoopType { get; set; }

            [JsonProperty("loopTimes")]
            public int LoopTimes { get; set; } = 1;

            [JsonProperty("circleDuration")]
            public int CircleDuration { get; set; }

            [JsonProperty("loopInterval")]
            public int LoopInterval { get; set; }

            [JsonProperty("loopDuration")]
            public int LoopDuration { get; set; }

            [JsonProperty("accelerateTimes")]
            public int AccelerateTimes { get; set; } = 1;

            [JsonProperty("recordName")]
            public string RecordName { get; set; } = "";

            [JsonProperty("createTime")]
            public string CreateTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            [JsonProperty("playOnBoot")]
            public bool PlayOnBoot { get; set; }

            [JsonProperty("rebootTiming")]
            public int RebootTiming { get; set; }
        }
    }
}