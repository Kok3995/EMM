using Data;
using EMM.Core.Converter;
using System.Windows;
using System.Windows.Input;

namespace EMM.Core.ViewModels
{
    /// <summary>
    /// This class is used to manage macro: new, load, save....
    /// </summary>
    public class MacroManagerViewModel : BaseViewModel
    {
        #region Ctor

        public MacroManagerViewModel(DataIO dataIO, IMessageBoxService messageBoxService, ViewModelFactory viewModelFactory)
        {
            this.dataIO = dataIO;
            this.messageBoxService = messageBoxService;
            this.viewModelFactory = viewModelFactory;

            //var test = new TestClass();

            //CurrentMacro = this.LoadMacroViewModel(test.ReturnTestTemplate());

            InitializeCommands();
            HookEventHandler();
        }

        #endregion

        #region Private members

        private DataIO dataIO;
        private IMessageBoxService messageBoxService;
        private ViewModelFactory viewModelFactory;

        #endregion

        #region Public Properties

        /// <summary>
        /// Check if macro is loaded
        /// </summary>
        public bool IsMacroLoaded => (CurrentMacro == null) ? false : true;

        /// <summary>
        /// Current Macro ViewModel ready to be edited....
        /// </summary>
        public MacroViewModel CurrentMacro { get; set; }

        #endregion

        #region Commands

        public ICommand NewCommand { get; set; }
        public ICommand OpenCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand SaveAsCommand { get; set; }

        private void InitializeCommands()
        {
            NewCommand = new RelayCommand(p =>
            {
                if (ShouldSaveMacro() == null)
                    return;

                CurrentMacro = viewModelFactory.NewMacroViewModel();
            });

            OpenCommand = new RelayCommand(p =>
            {
                if (ShouldSaveMacro() == null)
                    return;

                var loadedMacro = this.LoadMacroViewModel();
                if (loadedMacro == null)
                {
                    this.messageBoxService.ShowMessageBox("Cannot parse the file", "ERROR", MessageButton.OK, MessageImage.Error);
                    return;
                }
                
                this.CurrentMacro = loadedMacro;
            });

            ExitCommand = new RelayCommand(p => Application.Current.Shutdown());

            SaveCommand = new RelayCommand(p =>
            {
                this.CurrentMacro.MacroPath = this.SaveMacro(CurrentMacro, this.CurrentMacro.MacroPath);
                CurrentMacro.AcceptChanges();
            }, p => CurrentMacro != null);

            SaveAsCommand = new RelayCommand(p =>
            {
                this.CurrentMacro.MacroPath = this.SaveAsMacro(CurrentMacro, this.CurrentMacro.MacroPath);
                CurrentMacro.AcceptChanges();
            }, p => CurrentMacro != null);
        }

        #endregion

        #region Events

        private void HookEventHandler()
        {
            //Handler file drop
            Messenger.Register((sender, e) =>
            {
                if (ShouldSaveMacro() == null)
                    return;

                if (e.FilePaths == null || e.FilePaths.Length < 1)
                    return;

                var filePath = e.FilePaths[0];

                var loadedMacro = this.dataIO.LoadMacroFileFromPath(filePath);
                if (loadedMacro == null)
                {
                    this.messageBoxService.ShowMessageBox("Cannot parse the file", "ERROR", MessageButton.OK, MessageImage.Error);
                    return;
                }

                this.CurrentMacro = this.LoadMacroViewModel(loadedMacro.MacroTemplate);
                this.CurrentMacro.MacroPath = loadedMacro.MacroFullPath;
                this.CurrentMacro.AcceptChanges();
            });

            //SaveMacroBeforeExit
            Messenger.Register((sender, e) =>
            {
                if (e.EventMessage != EventMessage.SaveMacroBeforeExit)
                    return;

                this.SaveCommand.Execute(null);
            });
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Set the current macro to the pass in macro
        /// </summary>
        /// <param name="macro"></param>
        public void SetCurrentMacro(MacroViewModel macro)
        {
            this.CurrentMacro = macro;
            this.CurrentMacro.AcceptChanges();
        }

        /// <summary>
        /// set the current macro base on the path
        /// </summary>
        /// <param name="macro"></param>
        public void SetCurrentMacro(string path)
        {
            this.CurrentMacro = this.LoadMacroViewModel(path);
        }

        /// <summary>
        /// Load <see cref="MacroViewModel"/> from <see cref="MacroTemplate"/>
        /// </summary>
        /// <param name="macroTemplate">The source <see cref="MacroTemplate"/></param>
        /// <returns></returns>
        public MacroViewModel LoadMacroViewModel(MacroTemplate macroTemplate)
        {
            return viewModelFactory.NewMacroViewModel().PopulateProperties(macroTemplate);
        }

        /// <summary>
        /// Load <see cref="MacroViewModel"/> from path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public MacroViewModel LoadMacroViewModel(string path)
        {
            var macroTemplate = this.dataIO.LoadMacroFileFromPath(path).MacroTemplate;
            var vm = viewModelFactory.NewMacroViewModel().PopulateProperties(macroTemplate);
            vm.MacroPath = path;
            return vm;
        }

        /// <summary>
        /// Load <see cref="MacroViewModel"/> from file using OpenFileDialog
        /// </summary>
        /// <returns></returns>
        public MacroViewModel LoadMacroViewModel()
        {
            var loadedMacro = this.dataIO.LoadFromFile();
            if (loadedMacro == null)
                return null;
            var macroViewModel = this.LoadMacroViewModel(loadedMacro.MacroTemplate);
            macroViewModel.MacroPath = loadedMacro.MacroFullPath;
            macroViewModel.AcceptChanges();
            return macroViewModel;
        }

        /// <summary>
        /// Save the Macro to file
        /// </summary>
        /// <param name="macroViewModel">The Macro to be saved</param>
        private string SaveMacro(MacroViewModel macroViewModel, string fullPath)
        {
            var macroTemplate = macroViewModel.ConvertBack();

            return this.dataIO.SaveToFile(macroTemplate, fullPath);
        }

        /// <summary>
        /// Save AS the Macro to file
        /// </summary>
        /// <param name="macroViewModel">The Macro to be saved</param>
        private string SaveAsMacro(MacroViewModel macroViewModel, string fullPath)
        {
            var macroTemplate = macroViewModel.ConvertBack();

            return this.dataIO.SaveAsToFile(macroTemplate, fullPath);
        }

        /// <summary>
        /// Ask for saving macro if the macro's IsChanged is true
        /// </summary>
        private bool? ShouldSaveMacro()
        {
            if (CurrentMacro == null)
                return false;

            if (CurrentMacro.IsChanged == true)
            {
                MessageResult result = MessageResult.Cancel;
                if (CurrentMacro != null)
                {
                    result = this.messageBoxService.ShowMessageBox("Do you want to save the current Macro?", "EMM", MessageButton.YesNoCancel, MessageImage.Question);
                }

                switch (result)
                {
                    case MessageResult.Cancel:
                    case MessageResult.None:
                        return null;
                    case MessageResult.No:
                        return false;
                    case MessageResult.Yes:
                        SaveCommand.Execute(null);
                        return true;
                }
            }

            return false;
        }

        #endregion
    }
}
