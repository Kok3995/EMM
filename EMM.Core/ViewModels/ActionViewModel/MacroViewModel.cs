using Data;
using EMM.Core.Converter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EMM.Core.ViewModels
{
    /// <summary>
    /// ViewModel for <see cref="MacroTemplate"/>
    /// </summary>
    public class MacroViewModel : CommonViewModel
    {
        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MacroViewModel(SimpleAutoMapper autoMapper, ViewModelFactory viewModelFactory)
        {
            this.autoMapper = autoMapper;
            this.viewModelFactory = viewModelFactory;
            InitializeCommands();
        }

        #endregion

        #region Private Members

        /// <summary>
        /// back up last edited x resolution
        /// </summary>
        private string lastEditedX;

        /// <summary>
        /// back up last edited y resolution
        /// </summary>
        private string lastEditedY;

        /// <summary>
        /// Instance of automapper to load viewmodel
        /// </summary>
        private SimpleAutoMapper autoMapper;

        private ViewModelFactory viewModelFactory;

        #endregion

        #region Public Properties

        /// <summary>
        /// Path to the currently loaded macro
        /// </summary>
        public string MacroPath { get; set; } = string.Empty;

        /// <summary>
        /// Name of the macro
        /// </summary>
        public string MacroName { get; set; } = "New Macro";

        /// <summary>
        /// Get or Set the version of the macro 
        /// </summary>
        public int MacroVersion { get; set; } = 1;

        /// <summary>
        /// The template x resolution
        /// </summary>
        public int OriginalX { get; set; } = 1280;

        /// <summary>
        /// The template y resolution
        /// </summary>
        public int OriginalY { get; set; } = 720;

        /// <summary>
        /// Hide or unhide the apply and cancel button
        /// </summary>
        public bool IsResolutionModify { get; set; } = false;

        #endregion

        #region Commands

        public ICommand ModifyResolutionCommand { get; set; }
        public ICommand ApplyResolutionChangedCommand { get; set; }
        public ICommand CancelResolutionChangedCommand { get; set; }

        /// <summary>
        /// Initilize all commands in this viewmodel
        /// </summary>
        private void InitializeCommands()
        {
            ModifyResolutionCommand = new RelayCommand(p =>
            {
                lastEditedX = this.OriginalX.ToString();
                lastEditedY = this.OriginalY.ToString();

                IsResolutionModify = true;
            });

            CancelResolutionChangedCommand = new RelayCommand(p =>
            {
                try
                {
                    this.OriginalX = int.Parse(lastEditedX);
                    this.OriginalY = int.Parse(lastEditedY);
                }
                catch { }
                OnPropertyChanged("OriginalX");
                OnPropertyChanged("OriginalY");
                IsResolutionModify = false;
            });

            ApplyResolutionChangedCommand = new RelayCommand(p => IsResolutionModify = false);            
        }

        #endregion

        #region Interface Implement

        /// <summary>
        /// Convert back to <see cref="MacroTemplate"/>
        /// </summary>
        /// <returns></returns>
        public MacroTemplate ConvertBack()
        {
            var macroTemplate = this.autoMapper.SimpleAutoMap<MacroViewModel, MacroTemplate>(this);
            macroTemplate.ActionGroupList = new List<IAction>();

            foreach (var actionGroupViewModel in base.ViewModelList)
            {
                macroTemplate.ActionGroupList.Add(actionGroupViewModel.ConvertBackToAction());
            }

            return macroTemplate;
        }

        /// <summary>
        /// Populate the properties of its self with an <see cref="MacroTemplate"/>
        /// </summary>
        /// <param name="actionGroup">The <see cref="MacroTemplate"/></param>
        public MacroViewModel PopulateProperties(MacroTemplate macroTemplate)
        {
            if (macroTemplate == null)
                return null;

            this.autoMapper.SimpleAutoMap<MacroTemplate, MacroViewModel>(macroTemplate, this);

            foreach (var actionGroup in macroTemplate.ActionGroupList)
            {
                base.ViewModelList.Add(viewModelFactory.NewActionViewModel(BasicAction.ActionGroup).ConvertFromAction(actionGroup));
            }

            return this;
        }
        protected override void AddItem(object parameter)
        {
            base.ViewModelList.Add(base.SelectedItem = viewModelFactory.NewActionViewModel(BasicAction.ActionGroup));
        }
        protected override void InsertItem(object parameter)
        {
            base.ViewModelList.Insert(SelectedItemIndex + 1, SelectedItem = viewModelFactory.NewActionViewModel(BasicAction.ActionGroup));
        }

        #endregion

        #region Helpers

        #endregion
    }
}
