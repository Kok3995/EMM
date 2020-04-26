using EMM.Core;
using EMM.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace EMM
{
    public class AutoListBox : ScrollIntoViewListBox
    {
        public AutoListBox()
        {
        }

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

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            foreach (var item in e.AddedItems)
            {
                (item as BaseViewModel).IsSelected = true;
            }

            foreach (var item in e.RemovedItems)
            {
                (item as BaseViewModel).IsSelected = false;
            }
        }

        internal void DeSelectAllChild()
        {
            if (Items == null || Items.Count <= 0)
                return;

            var stack = new Stack<CommonViewModel>();
            var commonViewModel = DataContext as CommonViewModel;

            if (commonViewModel == null)
            {
                if (DataContext is BaseViewModel viewModel)
                {
                    viewModel.IsSelected = false;
                    return;
                }
            }

            foreach (var item in commonViewModel.ViewModelList)
            {
                item.IsSelected = false;

                if (item is CommonViewModel commonVM)
                {
                    stack.Push(commonVM);
                }
            }

            while(stack.Count > 0)
            {
                var currentItem = stack.Pop();

                foreach (var item in currentItem.ViewModelList)
                {
                    item.IsSelected = false;

                    if (item is CommonViewModel commonVM)
                    {
                        stack.Push(commonVM);
                    }
                }
            }
        }

        //protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        //{
        //    base.OnPreviewMouseWheel(e);

        //    if (!e.Handled)
        //    {
        //        e.Handled = true;

        //        if (this.Template.FindName("Scroller", this) is ScrollViewer scrollViewer)
        //        {
        //            if (e.Delta < 0)
        //                scrollViewer.PageDown();
        //            else
        //                scrollViewer.PageUp();
        //        }
        //    }
        //}
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

            var parentListBox = this.FindTopLevelParentOfType<AutoListBox>();

            if (parentListBox != null)
            {
                parentListBox.DeSelectAllChild();
            }

            if (DataContext is BaseViewModel dataContext)
                dataContext.IsSelected = true;

            base.OnMouseDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
                this.IsMouseDoubleClicked = false;
            base.OnKeyDown(e);
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
