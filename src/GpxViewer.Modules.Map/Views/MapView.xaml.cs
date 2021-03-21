using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

                this.DataContextChanged += this.OnThis_DataContextChanged;
            }
        }

        private void OnThis_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is MapViewModel oldVM)
            {
                oldVM.AddditionalMapLayers.CollectionChanged -= this.OnViewModel_AdditionalMapLayers_CollectionChanged;
                foreach(var actLayer in oldVM.AddditionalMapLayers)
                {
                    this.CtrlMap.Map.Layers.Remove(actLayer);
                }
            }

            if (e.NewValue is MapViewModel newVM)
            {
                foreach (var actLayer in newVM.AddditionalMapLayers)
                {
                    this.CtrlMap.Map.Layers.Add(actLayer);
                }
                newVM.AddditionalMapLayers.CollectionChanged += this.OnViewModel_AdditionalMapLayers_CollectionChanged;
            }
        }

        private void OnViewModel_AdditionalMapLayers_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (ILayer actNewLayer in e.NewItems!)
                    {
                        this.CtrlMap.Map.Layers.Add(actNewLayer);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (ILayer actRemovedLayer in e.OldItems!)
                    {
                        this.CtrlMap.Map.Layers.Remove(actRemovedLayer);
                    }
                    break;

                default:
                    throw new NotSupportedException($"Action {e.Action} is not supported yet!");
            }
        }
    }
}
