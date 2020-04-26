using Data;
using EMM;
using EMM.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Input;

namespace AEMG_EX.Core
{
    /// <summary>
    /// Scan for macro to display in combobox. Subscribe to SelectChanged event to get the currently selected macro
    /// </summary>
    public class AEMacroManagerViewModel : BaseViewModel
    {
        #region Ctor

        public AEMacroManagerViewModel(IScanner scanner, IMessageBoxService messageBoxService, IMacroManager macroManager, IWindowInitlizer windowInitlizer)
        {
            this.scanner = scanner;
            this.messageBoxService = messageBoxService;
            this.macroManager = macroManager;
            this.windowInitlizer = windowInitlizer;

            InitializeCommand(); 
            HookEventHandler();
        }

        #endregion

        #region Private members

        private readonly IScanner scanner;
        private IMessageBoxService messageBoxService;
        private IMacroManager macroManager;
        private IWindowInitlizer windowInitlizer;
        private Dictionary<string, MacroTemplate> loadedTemplates;
        private string tempFolder = Path.Combine(Environment.CurrentDirectory, "temp");

        #endregion

        #region Public Properties

        /// <summary>
        /// Collection of macroes scan from /Macroes
        /// </summary>
        public ObservableCollection<MacroTemplate> MacroList { get; set; }

        /// <summary>
        /// Currently selected macro. Will fire changed event
        /// </summary>
        public MacroTemplate SelectedMacro 
        {
            get => macroManager.GetCurrentTemplate();
            set
            {
                var path = GetSelectedMacroPath(value);

                macroManager.SetCurrentTemplate(value, path);
            }
        }

        /// <summary>
        /// The index of the selectedmacro
        /// </summary>
        public int SelectedMacroIndex { get; set; }

        #endregion

        #region Commands

        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ReScanCommand { get; set; }
        public ICommand ReLoadCommand { get; set; }
        public ICommand OpenInEMMCommand { get; set; }
        public ICommand OpenSettingWindowCommand { get; set; }

        private void InitializeCommand()
        {
            AddCommand = new RelayCommand(p =>
            {
                try
                {
                    var error = 0;

                    var loadeds = this.scanner.ScanUserSelected((e) => { error++; }).ToList();

                    if (loadeds == null || loadeds.Count == 0)
                        return;

                    if (error > 0)
                        this.messageBoxService.ShowMessageBox(error + " error(s) has occured. Some macro might not be completed", "Error", MessageButton.OK, MessageImage.Error);

                    foreach (var loaded in loadeds)
                        CopyAndAddMacroToList(loaded);
                }
                catch
                {
                    this.messageBoxService.ShowMessageBox("Can not add the macro", "ERROR", MessageButton.OK, MessageImage.Error);
                }
            });

            DeleteCommand = new RelayCommand(p =>
            {
                //find the current selected macro path
                var path = CheckIfSelectedMacroStillExist();

                var result = this.messageBoxService.ShowMessageBox("Do you want to delete this macro?", "DELETE", MessageButton.YesNo, MessageImage.Question, MessageResult.No);

                if (result == MessageResult.Yes)
                {
                    if (path != null)
                    {
                        File.Delete(path);
                        this.MacroList.Remove(SelectedMacro);
                    }
                    else
                    {
                        this.ScanForMacroes();
                    }
                }
            }, p => this.SelectedMacro != null);

            ReScanCommand = new RelayCommand(p =>
            {
                this.ScanForMacroes();
            });

            ReLoadCommand = new RelayCommand(p =>
            {
                //find the current selected macro path
                var path = CheckIfSelectedMacroStillExist();

                var reloadedMacro = this.scanner.ScanSingle(path);

                if (reloadedMacro == null)
                    this.messageBoxService.ShowMessageBox("Cannot find the macro anymore", "ERROR", MessageButton.OK, MessageImage.Error);

                this.macroManager.InvokeBeforeMacroReloaded();

                var index = this.MacroList.IndexOf(SelectedMacro);

                if (index > -1 && index < this.MacroList.Count)
                {
                    this.MacroList.Remove(SelectedMacro);
                    this.MacroList.Insert(index, reloadedMacro.MacroTemplate);
                    this.SelectedMacro = reloadedMacro.MacroTemplate;
                    this.loadedTemplates[path] = reloadedMacro.MacroTemplate;
                }
                this.macroManager.InvokeAfterMacroReloaded();
            }, p => this.SelectedMacro != null);

            OpenInEMMCommand = new RelayCommand(p =>
            {
                if (this.SelectedMacro == null)
                    return;

                //check if EMM exist
                if (!File.Exists(Path.Combine(Environment.CurrentDirectory, AEMGStatic.EMM_NAME)))
                {
                    this.messageBoxService.ShowMessageBox("Cannot find EMM application. Put this application to the same folder as EMM.exe", "ERROR", MessageButton.OK, MessageImage.Error);
                    return;
                }

                //Get the path
                var path = CheckIfSelectedMacroStillExist();

                if (path == null)
                    return;

                AEMGHelpers.StartEMM(path);

            }, p => this.SelectedMacro != null);

            OpenSettingWindowCommand = new RelayCommand(p =>
            {
                windowInitlizer.OpenSettingWindow();
            });
        }

        #endregion

        #region Events

        private void HookEventHandler()
        {
            Messenger.Register((sender, e) =>
            {
                this.DropMacroHandler(e.FilePaths);
            });
        }

        private void DropMacroHandler(string[] filepaths)
        {
            int error = 0;
            if (filepaths == null || filepaths.Length < 1)
                return;

            foreach (var filepath in filepaths)
            {
                //check exist
                if (!File.Exists(filepath))
                    return;

                var macro = this.scanner.ScanSingle(filepath);

                if (macro == null)
                    error++;

                if (macro != null)
                    this.CopyAndAddMacroToList(macro);
            }

            if (error > 0)
                this.messageBoxService.ShowMessageBox(error + " error(s) has occured. Some macro might not be completed", "Error", MessageButton.OK, MessageImage.Error);
        }

        #endregion

        #region Implement Interface

        public void ScanForMacroes()
        {
            var loadedList = this.scanner.ScanAll(AEMGStatic.MACRO_FOLDER).Where(m => m != null).ToList();

            this.loadedTemplates = loadedList.ToDictionary(k => k.MacroFullPath, e => e.MacroTemplate);

            var macroInfoList = loadedList.Select(t => t.MacroTemplate).ToList();

            if (this.MacroList == null)
            {
                this.MacroList = new ObservableCollection<MacroTemplate>(macroInfoList);
                return;
            }

            this.MacroList.Clear();
            foreach (var m in macroInfoList)
                this.MacroList.Add(m);

            //clear cache
            this.macroManager.ClearCache();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// check if the current selected macro still exist. return the path
        /// </summary>
        /// <returns>return null if not find</returns>
        private string CheckIfSelectedMacroStillExist()
        {
            //find the current selected macro path
            var path = this.loadedTemplates.FirstOrDefault(m => this.SelectedMacro.Equals(m.Value)).Key;

            if (string.IsNullOrWhiteSpace(path))
                this.messageBoxService.ShowMessageBox("Cannot find the macro anymore", "ERROR", MessageButton.OK, MessageImage.Error);

            if (!File.Exists(path))
                this.messageBoxService.ShowMessageBox("The file does not exist anymore", "ERROR", MessageButton.OK, MessageImage.Error);

            return path;
        }

        private void CopyAndAddMacroToList(LoadedTemplate macro)
        {
            if (macro == null)
                return;

            //If the macro has not already in the folder then copy and add to the list
            if (!AEMGStatic.MACRO_FOLDER.Equals(Path.GetDirectoryName(macro.MacroFullPath), StringComparison.OrdinalIgnoreCase))
            {
                var extension = Path.GetExtension(macro.MacroFullPath);
                var copyPath = macro.MacroFullPath;

                if (extension.Equals(".zip"))
                {
                    Directory.CreateDirectory(tempFolder);
                    ZipFile.ExtractToDirectory(macro.MacroFullPath, tempFolder);

                    var emmFile = Directory.GetFiles(tempFolder).Where(f => f.EndsWith("emm")).FirstOrDefault();

                    if (emmFile != null)
                        copyPath = emmFile;
                }

                var filename = Path.GetFileNameWithoutExtension(copyPath) + "_" + DateTime.UtcNow.ToString("dd-MM-yy-HH-mm-ss") + ".emm";
                var filePath = Path.Combine(AEMGStatic.MACRO_FOLDER, filename);
                File.Copy(copyPath, filePath);
                this.loadedTemplates[filePath] = macro.MacroTemplate;
                this.MacroList.Add(macro.MacroTemplate);
                this.SelectedMacro = macro.MacroTemplate;

                if (Directory.Exists(tempFolder))
                    Directory.Delete(tempFolder, true);
            }
            else
            {
                var macroInMemory = this.loadedTemplates[macro.MacroFullPath];
                if (macroInMemory == null)
                    return;

                this.SelectedMacro = macroInMemory;
                this.ReLoadCommand.Execute(null);
            }
        }

        private string GetSelectedMacroPath(MacroTemplate macro)
        {
            if (macro == null)
                return null;

            var path = this.loadedTemplates.FirstOrDefault(m => macro.Equals(m.Value)).Key;

            if (string.IsNullOrWhiteSpace(path))
                return null;

            return path;
        }

        #endregion
    }
}
