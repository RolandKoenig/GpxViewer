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
using FirLib.Core.Patterns.Mvvm;

namespace GpxViewer.ViewServices.Rename
{
    /// <summary>
    /// Interaction logic for RenameGpxFilesDialog.xaml
    /// </summary>
    public partial class RenameGpxFilesDialog : MvvmWindow
    {
        public RenameGpxFilesDialog()
        {
            this.InitializeComponent();
        }
    }
}
