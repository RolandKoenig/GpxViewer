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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirLib.Core.Patterns.Mvvm;

namespace GpxViewer.Modules.GpxFiles.Views
{
    public partial class FileTreeView : MvvmUserControl
    {
        public FileTreeView()
        {
            this.InitializeComponent();
        }
    }
}
