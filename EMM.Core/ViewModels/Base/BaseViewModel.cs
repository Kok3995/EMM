using EMM.Core.ViewModels;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EMM.Core
{
    public class BaseViewModel : IViewModel
    {
        public BaseViewModel()
        {
            PropertyChanged = (sender, e) =>
            {
                //if (e != null
                //&& !StringCompare(e.PropertyName, "IsChanged")
                //&& !StringCompare(e.PropertyName, "IsSelected")
                //&& !StringCompare(e.PropertyName, "SelectedItem")
                //&& !StringCompare(e.PropertyName, "SelectedItemIndex")
                //&& !StringCompare(e.PropertyName, "IsActionEnable")
                //)
                //    IsChanged = true;
            };
        }

        private bool _isChanged = false;

        [JsonIgnore]
        /// <summary>
        /// true if the viewmodel was modified
        /// </summary>
        public bool IsChanged
        {
            get
            {
                return _isChanged;
            }
            set
            {
                if (Boolean.Equals(_isChanged, value))
                    return;

                _isChanged = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Reset changed state
        /// </summary>
        public void AcceptChanges()
        {
            IsChanged = false;

            if (this is MacroViewModel mvm)
            {
                var stack = new Stack<IViewModel>();

                foreach (var ag in mvm.ViewModelList)
                {
                    stack.Push(ag);
                }

                while (stack.Count > 0)
                {
                    var current = stack.Pop();

                    current.AcceptChanges();

                    if (current is ActionGroupViewModel agvm)
                    {
                        foreach (var ag in agvm.ViewModelList)
                        {
                            stack.Push(ag);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Notify a Property has been changed
        /// </summary>
        /// <param name="propertyName">The property name</param>
        public void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonIgnore]
        [DoNotSetChanged]
        /// <summary>
        /// Determine if this viewmodel is selected in the listbox
        /// </summary>
        public bool IsSelected { get; set; }

        //private bool StringCompare(string left, string right)
        //{
        //    return string.Equals(left, right, StringComparison.Ordinal);
        //}
    }
}
