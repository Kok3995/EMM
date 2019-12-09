using System;
using System.ComponentModel;

namespace EMM.Core
{
    public class BaseViewModel : IViewModel
    {
        public BaseViewModel()
        {
            PropertyChanged = (sender, e) =>
            {
                if (e != null && !string.Equals(e.PropertyName, "IsChanged", StringComparison.Ordinal))
                IsChanged = true;
            };
        }
        private bool _isChanged = false;

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
        }

        /// <summary>
        /// Notify a Property has been changed
        /// </summary>
        /// <param name="propertyName">The property name</param>
        public void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Determine if this viewmodel is selected in the listbox
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
