using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using BruTile.Predefined;
using FirLib.Core.Infrastructure;
using FirLib.Core.Patterns.Mvvm;
using GpxViewer.ViewServices.Rename;
using Mapsui.Fetcher;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.Utilities;
using Microsoft.Win32;

namespace GpxViewer
{
    public partial class MainWindow : MvvmWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();

            if(FirLibApplication.IsLoaded)
            {
                // Register view services
                this.ViewServices.Add(new WpfRenameGpxFilesViewService(this));
            }
        }
    }
}
