using Data;
using EMM;
using EMM.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace AEMG_EX.Core
{
    /// <summary>
    /// Scan for macro to display in combobox. Subscribe to SelectChanged event to get the currently selected macro
    /// </summary>
    public class AEMacroManager : BaseViewModel, IMacroManager
    {
        #region Ctor

        public AEMacroManager(IScanner scanner, IMessageBoxService messageBoxService)
        {
            this.scanner = scanner;
            this.messageBoxService = messageBoxService;

            InitializeCommand(); 
            HookEventHandler();
        }

        #endregion

        #region Private members

        private readonly IScanner scanner;
        private IMessageBoxService messageBoxService;
        private MacroTemplate selectedMacro;
        private Dictionary<string, MacroTemplate> loadedTemplates;
        private Dictionary<MacroTemplate, List<IAEActionViewModel>> cache;

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
            get => selectedMacro;
            set
            {
                selectedMacro = value;

                SelectChanged?.Invoke(this, EventArgs.Empty);
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

        private void InitializeCommand()
        {
            AddCommand = new RelayCommand(p =>
            {
                try
                {
                    var loadeds = this.scanner.ScanUserSelected().ToList();

                    if (loadeds == null || loadeds.Count == 0)
                        return;

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

                if (path != null)
                {
                    File.Delete(path);
                    this.MacroList.Remove(SelectedMacro);
                }
                else
                {
                    this.ScanForMacroes();
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

                this.MacroList.Remove(SelectedMacro);
                this.MacroList.Add(reloadedMacro.MacroTemplate);
                this.SelectedMacro = reloadedMacro.MacroTemplate;
                this.loadedTemplates[path] = reloadedMacro.MacroTemplate;
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

                using (var process = new Process())
                {
                    var startInfo = new ProcessStartInfo()
                    {
                        FileName = AEMGStatic.EMM_NAME,
                        Arguments = path,
                    };

                    process.StartInfo = startInfo;
                    process.Start();
                }

            }, p => this.SelectedMacro != null);
        }

        #endregion

        #region Events

        /// <summary>
        /// Fire when selected macro changed
        /// </summary>
        public event EventHandler SelectChanged;

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
                this.messageBoxService.ShowMessageBox(error + " file(s) can not be parsed", "Error", MessageButton.OK, MessageImage.Error);
        }

        #endregion

        #region Implement Interface

        public IEnumerable<IAEActionViewModel> GetCurrentAEActionList()
        {
            if (this.SelectedMacro == null)
                return Enumerable.Empty<IAEActionViewModel>();

            //check cache
            if (this.cache == null)
                this.cache = new Dictionary<MacroTemplate, List<IAEActionViewModel>>();

            if (this.cache.ContainsKey(this.SelectedMacro))
                return this.cache[this.selectedMacro];

            var aevmList = this.scanner.ScanMacroForPlaceHolder(this.SelectedMacro).ToList();

            this.cache[this.SelectedMacro] = aevmList;

            return aevmList;
        }

        public void ScanForMacroes()
        {
            var loadedList = this.scanner.ScanAll(AEMGStatic.MACRO_FOLDER).ToList();

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
            this.cache?.Clear();
        }

        public MacroTemplate GetCurrentTemplate()
        {
            return SelectedMacro;
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

            if (path == null)
                this.messageBoxService.ShowMessageBox("Cannot find the macro anymore", "ERROR", MessageButton.OK, MessageImage.Error);

            if (!File.Exists(path))
                this.messageBoxService.ShowMessageBox("The file does not exist anymore", "ERROR", MessageButton.OK, MessageImage.Error);

            return path;
        }

        private void CopyAndAddMacroToList(LoadedTemplate macro)
        {
            //If the macro has not already in the folder then copy and add to the list
            if (!AEMGStatic.MACRO_FOLDER.Equals(Path.GetDirectoryName(macro.MacroFullPath), StringComparison.OrdinalIgnoreCase))
            {
                var filename = Path.GetFileNameWithoutExtension(macro.MacroFullPath) + "_" + DateTime.UtcNow.ToString("dd-MM-yy-HH-mm-ss") + ".emm";
                var filePath = Path.Combine(AEMGStatic.MACRO_FOLDER, filename);
                File.Copy(macro.MacroFullPath, filePath);
                this.loadedTemplates[filePath] = macro.MacroTemplate;
                this.MacroList.Add(macro.MacroTemplate);
                this.SelectedMacro = macro.MacroTemplate;
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

        #endregion
    }
}
