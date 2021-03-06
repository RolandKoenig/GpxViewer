using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using Mapsui.Layers;

namespace GpxViewer.View.Map
{
    public partial class MapView : MvvmUserControl
    {
        private ILayer _mainLayer;

        public MapView()
        {
            this.InitializeComponent();

            if (FirLibApplication.IsInitialized)
            {
                // Add main map layer
                _mainLayer = new TileLayer(KnownTileSources.Create());
                this.CtrlMap.Map.Layers.Add(_mainLayer);

                // Register on events
                this.DataContextChanged += this.OnDataContextChanged;
            }
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Discard old ViewModel
            if (e.OldValue is MapViewModel viewModelOld)
            {
                viewModelOld.VisibleGpxFiles.CollectionChanged -= this.OnViewModel_VisibleGpxFiles_CollectionChanged;
            }

            // Clear all additional layers
            for (var loop = this.CtrlMap.Map.Layers.Count - 1; loop >= 0; loop--)
            {
                var actLayer = this.CtrlMap.Map.Layers[loop];
                if (actLayer != _mainLayer)
                {
                    this.CtrlMap.Map.Layers.Remove(actLayer);
                }
            }

            // Apply new ViewModel
            if (e.NewValue is MapViewModel viewModelNew)
            {
                foreach (var actGpxFile in viewModelNew.VisibleGpxFiles)
                {
                    this.CtrlMap.Map.Layers.Add(actGpxFile.GpxMapLayer);
                }

                viewModelNew.VisibleGpxFiles.CollectionChanged += this.OnViewModel_VisibleGpxFiles_CollectionChanged;
            }
        }

        private void OnViewModel_VisibleGpxFiles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (GpxFileViewModel actOldFile in e.OldItems)
                {
                    this.CtrlMap.Map.Layers.Remove(actOldFile.GpxMapLayer);
                }
            }

            if(e.NewItems != null)
            {
                foreach (GpxFileViewModel actNewFile in e.NewItems)
                {
                    this.CtrlMap.Map.Layers.Add(actNewFile.GpxMapLayer);
                }
            }
        }
    }
}
