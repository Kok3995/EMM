using System;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace EMM
{ 
    public class ScrollIntoViewListBox : ListBox
    {
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (this.Items == null || this.Items.Count < 1)
                return;

            this.ScrollIntoView(this.Items.GetItemAt(this.Items.Count - 1));
        }
    }
}
