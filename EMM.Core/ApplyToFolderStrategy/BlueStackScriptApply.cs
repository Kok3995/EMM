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
    public class BlueStackScriptApply : BaseScriptApply, IApplyScriptToFolder
    {
        public BlueStackScriptApply(IMessageBoxService messageBoxService, IMTPManager mTPManager) : base(mTPManager)
        {
            this.messageBoxService = messageBoxService;
        }

        private IMessageBoxService messageBoxService;

        public bool? ApplyScriptTo(string scriptName, string path, object scriptObj, bool prompt = true)
        {
            try
            {
                var script = scriptObj as List<BlueStackEvent>;

                if (script == null)
                {
                    throw new InvalidOperationException("Passed in script object is not a list of BlueStackEvent");
                }

                var fileNameInRecordFolder = scriptName + ".json";
                var fullPathToMacroFile = Path.Combine(path, fileNameInRecordFolder); // path/scriptname.json
                var bluestackFileJson = new BlueStackMacroFormat(scriptName, script);

                //check existed
                bool isExisted = /*File.Exists(fullPathToMacroFile)*/IsFileExist(fullPathToMacroFile);

                if (!isExisted)
                {
                    //File.WriteAllText(fullPathToMacroFile, JsonConvert.SerializeObject(bluestackFileJson, Formatting.Indented, new CustomBlueStackJsonConverter()));
                    WriteAllText(fullPathToMacroFile, JsonConvert.SerializeObject(bluestackFileJson, Formatting.Indented, new CustomBlueStackJsonConverter()));
                }
                else
                {
                    MessageResult overwriteChoice = MessageResult.Yes;

                    if (prompt == true)
                        overwriteChoice = messageBoxService.ShowMessageBox("Macro's name already existed. Do you want to overwrite?", "Overwrite?", MessageButton.YesNo, MessageImage.Question);

                    if (overwriteChoice == MessageResult.Yes)
                    {
                        //File.WriteAllText(fullPathToMacroFile, JsonConvert.SerializeObject(bluestackFileJson, Formatting.Indented, new CustomBlueStackJsonConverter()));
                        WriteAllText(fullPathToMacroFile, JsonConvert.SerializeObject(bluestackFileJson, Formatting.Indented, new CustomBlueStackJsonConverter()));
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

        public class BlueStackMacroFormat
        {
            public BlueStackMacroFormat(string name, List<BlueStackEvent> eventList)
            {
                this.Name = name;
                this.Events = eventList;
            }
            public string TimeCreated { get; set; } = DateTime.Now.ToString("yyyyMMddTHHmmss");

            public string Name { get; set; }

            public List<BlueStackEvent> Events { get; set; } = new List<BlueStackEvent>();

            public string LoopType { get; set; } = "TillLoopNumber";

            public int LoopNumber { get; set; } = 1;
            public int LoopTime { get; set; }
            public int LoopInterval { get; set; }
            public float Acceleration { get; set; } = 1.0f;
            public bool PlayOnStart { get; set; }
            public bool DonotShowWindowOnFinish { get; set; }
            public bool RestartPlayer { get; set; }
            public int RestartPlayerAfterMinutes { get; set; } = 60;
            public string ShortCut { get; set; } = "";
            public string UserName { get; set; } = "";
            public string MacroId { get; set; } = "";

        }

        public class CustomBlueStackJsonConverter : JsonConverter
        {
            public override bool CanRead => false;

            public override bool CanWrite => base.CanWrite;

            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(BSEventType));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new InvalidOperationException("Use the default reader");
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }
        }
    }
}