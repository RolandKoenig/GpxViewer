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
using PropertyTools.Wpf;

namespace GpxViewer.Core.ViewServices.AboutDialog
{
    /// <summary>
    /// Interaction logic for AboutDialogWindow.xaml
    /// </summary>
    internal partial class AboutDialogWindow : MvvmWindow
    {
        public AboutDialogWindow()
        {
            InitializeComponent();
        }
    }
}
