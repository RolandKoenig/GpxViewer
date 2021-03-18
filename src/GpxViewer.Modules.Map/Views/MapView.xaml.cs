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
using FirLib.Core.Infrastructure;
using FirLib.Core.Patterns.Mvvm;
using Mapsui.Layers;
using Mapsui.Utilities;

namespace GpxViewer.Modules.Map.Views
{
    public partial class MapView : MvvmUserControl
    {
        private ILayer? _mainLayer;

        public MapView()
        {
            this.InitializeComponent();

            if (FirLibApplication.IsLoaded)
            {
                // Add main map layer
                _mainLayer = OpenStreetMap.CreateTileLayer();
                this.CtrlMap.Map.Layers.Add(_mainLayer);
            }
        }
    }
}
