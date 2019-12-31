using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    /// <summary>
    /// This class is used do some data access: Load, Save from files....
    /// </summary>
    public class DataIO
    {
        private readonly string savedTemplateDirectory = Path.Combine(Environment.CurrentDirectory);
        private string lastPath = string.Empty;

        /// <summary>
        /// OpenFileDialog to Load macro .emm file
        /// </summary>
        /// <returns>Return null if the user press cancel or close the dialog</returns>
        public LoadedTemplate LoadFromFile(string initialDirectory = null, Action<Newtonsoft.Json.Serialization.ErrorEventArgs> errorCallback = null)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = @"Easy Macro Maker (.emm)|*.emm|All files (*.*)|*.*",
                InitialDirectory = (string.IsNullOrEmpty(this.lastPath)) ? (initialDirectory ?? Environment.CurrentDirectory) : lastPath,
            };
            openFileDialog.ShowDialog();

            //Check if the user press cancel
            if (string.IsNullOrEmpty(openFileDialog.FileName))
                return null;

            //set last path
            this.lastPath = Path.GetDirectoryName(openFileDialog.FileName);

            return this.LoadMacroFileFromPath(openFileDialog.FileName, errorCallback);            
        }

        /// <summary>
        /// OpenFileDialog to Load multiple macro .emm file
        /// </summary>
        /// <returns>Return null if the user press cancel or close the dialog</returns>
        public IEnumerable<LoadedTemplate> LoadFromFileMultiple(string initialDirectory = null, Action<Newtonsoft.Json.Serialization.ErrorEventArgs> errorCallback = null)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = @"Easy Macro Maker (.emm)|*.emm|All files (*.*)|*.*",
                Multiselect = true,
                InitialDirectory = (string.IsNullOrEmpty(this.lastPath)) ? (initialDirectory ?? Environment.CurrentDirectory) : lastPath, 
            };

            openFileDialog.ShowDialog();

            //Check if the user press cancel
            if (openFileDialog.FileName == null || openFileDialog.FileName.Equals(string.Empty))
                yield break;

            //set last path
            this.lastPath = Path.GetDirectoryName(openFileDialog.FileName);

            foreach (var filename in openFileDialog.FileNames)
            { 
                yield return LoadMacroFileFromPath(filename, errorCallback);
            }
        }

        /// <summary>
        /// Load .emm file given the path to the file
        /// </summary>
        /// <param name="path"></param>
        /// <returns>return null if can not parse the file</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public LoadedTemplate LoadMacroFileFromPath(string path, Action<Newtonsoft.Json.Serialization.ErrorEventArgs> errorCallback = null)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("The path can not be null");

            string macroString = File.ReadAllText(path);

            var setting = new JsonSerializerSettings();
            setting.Converters.Add(new CustomJsonConverter());
            setting.Error = (sender, e) =>
            {
                errorCallback?.Invoke(e);

                e.ErrorContext.Handled = true;
            };

            var macro = JsonConvert.DeserializeObject<MacroTemplate>(macroString, setting);

            return (macro != null) ? new LoadedTemplate { MacroTemplate = macro, MacroFullPath = path } : null;
        }

        /// <summary>
        /// Save to .emm files
        /// </summary>
        /// <param name="actionGroups"></param>
        /// <param name="folderPath">save to this folder</param>
        public string SaveAsToFile(MacroTemplate macroTemplate, string fullPath)
        {
            //Open the file save dialog
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
                CheckPathExists = true,
                DefaultExt = ".emm",
                InitialDirectory = (fullPath.Equals(string.Empty)) ? savedTemplateDirectory : Path.GetDirectoryName(fullPath),
                Filter = @"Easy Macro Maker (.emm)|*.emm",
                FileName = (fullPath.Equals(string.Empty)) ? string.Empty : Path.GetFileNameWithoutExtension(fullPath)
            };

            var result = saveFileDialog.ShowDialog();

            if (result == false)
                return fullPath;

            //Serialize the macro then save it
            File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(macroTemplate, Formatting.Indented));

            return saveFileDialog.FileName;
        }

        /// <summary>
        /// Save the <see cref="MacroTemplate"/> if the path is already existed, otherwise call the save as method
        /// </summary>
        /// <param name="macroTemplate">The <see cref="MacroTemplate"/> to save</param>
        /// <param name="fullPath">The path the <see cref="MacroTemplate"/></param>
        public string SaveToFile(MacroTemplate macroTemplate, string fullPath)
        {
            if (fullPath.Equals(string.Empty))
                return this.SaveAsToFile(macroTemplate, string.Empty);
            else
            //Serialize the macro then save it
            File.WriteAllText(fullPath, JsonConvert.SerializeObject(macroTemplate, Formatting.Indented));

            return fullPath;
        }

        /// <summary>
        /// Load the saved actions from file
        /// <paramref name="savedCustomAction"/>The path to the saved action list
        /// </summary>
        public IList<ActionGroup> LoadCustomActions(string savedCustomAction)
        {
            if (!File.Exists(savedCustomAction))
            {
                (new FileInfo(savedCustomAction)).Directory.Create();
                File.Create(savedCustomAction).Close();
                return new List<ActionGroup>();
            }

            return JsonConvert.DeserializeObject<List<ActionGroup>>(File.ReadAllText(savedCustomAction), new CustomJsonConverter());
        }

        /// <summary>
        /// Save the saved actions...No better name sorry
        /// </summary>
        /// <param name="saveds">The lists of actions to be saved</param>
        /// <param name="savedCustomActionPath">The path to the file to save</param>
        public void SaveCustomActions(IList<IAction> customActionList, string savedCustomActionPath)
        {
            if (!File.Exists(savedCustomActionPath))
            {
                (new FileInfo(savedCustomActionPath)).Directory.Create();
                File.Create(savedCustomActionPath).Close();
            }

            File.WriteAllText(savedCustomActionPath, JsonConvert.SerializeObject(customActionList, Formatting.Indented));
        }
    }

    public class LoadedTemplate
    {
        /// <summary>
        /// The loaded macro template
        /// </summary>
        public MacroTemplate MacroTemplate { get; set; }

        /// <summary>
        /// Full Path to the macro
        /// </summary>
        public string MacroFullPath { get; set; }
    }
}
