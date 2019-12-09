using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EMM
{
    public class AutoListBox : ListBox
    {
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (this.Items.Count > 0)
            {
                if (this.SelectedIndex == -1)
                    this.SelectedIndex = 0;
                this.Focus();
            }
            base.OnMouseDown(e);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new EditableListBoxItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is EditableListBoxItem;
        }
    }

    public class EditableListBoxItem: ListBoxItem
    {
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            this.IsMouseDoubleClicked = true;
            base.OnMouseDoubleClick(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            this.IsMouseDoubleClicked = false;
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Get or Set the value indicate if this <see cref="EditableListBoxItem"/> is double clicked
        /// </summary>
        public bool IsMouseDoubleClicked
        {
            get { return (bool)GetValue(IsMouseDoubleClickedProperty); }
            set { SetValue(IsMouseDoubleClickedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMouseDoubleClicked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMouseDoubleClickedProperty =
            DependencyProperty.Register("IsMouseDoubleClicked", typeof(bool), typeof(EditableListBoxItem), new PropertyMetadata(default(bool)));
    }
}
