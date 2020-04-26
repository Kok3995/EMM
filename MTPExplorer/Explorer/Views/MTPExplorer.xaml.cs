using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MTPExplorer
{
    /// <summary>
    /// Interaction logic for MTPExplorer.xaml
    /// </summary>
    public partial class MTPExplorer : Window, IMTPExplorer
    {
        public MTPExplorer()
        {
            InitializeComponent();
        }

        public bool? Result => DialogResult;

        public string SelectedPath { get; set; }

        private void Close(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        internal void OnSubmit(object sender, MTPFolderSelectEventArgs e)
        {
            SelectedPath = e.SelectedPath;
            DialogResult = true;
            Close();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            var vm = (DataContext as StructureViewModel);
            vm.OpenInitialDirectory();
        }
    }
}
