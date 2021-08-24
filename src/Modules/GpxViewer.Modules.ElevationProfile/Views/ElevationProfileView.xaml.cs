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
using LiveCharts.Wpf;

namespace GpxViewer.Modules.ElevationProfile.Views
{
    /// <summary>
    /// Interaction logic for ElevationProfileView.xaml
    /// </summary>
    internal partial class ElevationProfileView : MvvmUserControl
    {
        public ElevationProfileView()
        {
            InitializeComponent();
        }
    }
}
