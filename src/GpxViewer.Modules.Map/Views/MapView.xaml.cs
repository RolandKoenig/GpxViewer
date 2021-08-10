using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core;
using FirLib.Core.Infrastructure;
using FirLib.Core.Patterns.Mvvm;
using Mapsui.Geometries;
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

                this.HandleDataContextChanged<MapViewModel>(
                    this.OnThis_AttachToViewModel,
                    this.OnThis_DetachFromViewModel);
            }
        }

        private void OnThis_AttachToViewModel(MapViewModel viewModel)
        {
            foreach (var actLayer in viewModel.AdditionalMapLayers)
            {
                this.CtrlMap.Map.Layers.Add(actLayer);
            }
            viewModel.AdditionalMapLayers.CollectionChanged += this.OnViewModel_AdditionalMapLayers_CollectionChanged;
            viewModel.RequestNavigateToBoundingBox += this.OnViwewModel_RequestNavigateToBoundingBox;
        }

        private void OnThis_DetachFromViewModel(MapViewModel viewModel)
        {
            viewModel.AdditionalMapLayers.CollectionChanged -= this.OnViewModel_AdditionalMapLayers_CollectionChanged;
            viewModel.RequestNavigateToBoundingBox -= this.OnViwewModel_RequestNavigateToBoundingBox;
            foreach (var actLayer in viewModel.AdditionalMapLayers)
            {
                this.CtrlMap.Map.Layers.Remove(actLayer);
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

        private void OnViwewModel_RequestNavigateToBoundingBox(object? sender, RequestNavigateToBoundingBoxEventArgs e)
        {
            this.CtrlMap.Navigator.NavigateTo(e.NavTarget, ScaleMethod.Fit, 500L, Easing.Linear);
        }
    }
}
