﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using FirLib.Core.Patterns;
using GpxViewer.Core;
using GpxViewer.Core.GpxExtensions;
using GpxViewer.Core.Messages;
using GpxViewer.Core.Patterns;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using GpxViewer.Modules.GpxFiles.Interface.Model;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Providers;
using Mapsui.Styles;

namespace GpxViewer.Modules.Map.Views
{
    internal class MapViewModel : GpxViewerViewModelBase
    {
        private IGpxFileRepository _gpxFileRepo;

        private MapModuleConfiguration _config;

        private MemoryLayer _layerLoadedGpxFiles;
        private MemoryProvider _layerLoadedGpxFilesProvider;
        private List<ILoadedGpxFileTourInfo> _loadedTours;

        private MemoryLayer _layerSelectedGpxFiles;
        private MemoryProvider _layerSelectedGpxFilesProvider;
        private List<ILoadedGpxFileTourInfo> _selectedTours;

        private VectorStyle _lineStyleSelected;
        private VectorStyle _lineStyleInitial;
        private VectorStyle _lineStyleSucceeded;

        public ObservableCollection<ILayer> AdditionalMapLayers { get; }

        public MapViewSettings ViewSettings { get; }

        public DelegateCommand Command_ResetCamera { get; }

        public DelegateCommand Command_FocusSelectedTour { get; }

        public event EventHandler<RequestNavigateToBoundingBoxEventArgs>? RequestNavigateToBoundingBox;

        public event EventHandler<RequestCurrentViewportEventArgs>? RequestCurrentViewport;

        public MapViewModel(MapModuleConfiguration config, IGpxFileRepository gpxFileRepo)
        {
            _config = config;
            _gpxFileRepo = gpxFileRepo;

            this.ViewSettings = new MapViewSettings(_config);

            _layerLoadedGpxFiles = new MemoryLayer();
            _layerLoadedGpxFilesProvider = new MemoryProvider();
            _layerLoadedGpxFiles.DataSource = _layerLoadedGpxFilesProvider;
            _loadedTours = new List<ILoadedGpxFileTourInfo>();

            _layerSelectedGpxFiles = new MemoryLayer();
            _layerSelectedGpxFilesProvider = new MemoryProvider();
            _layerSelectedGpxFiles.DataSource = _layerSelectedGpxFilesProvider;
            _selectedTours = new List<ILoadedGpxFileTourInfo>();

            _lineStyleInitial = new VectorStyle
            {
                Fill = null,
                Outline = null,
                Line = { Color = Color.Gray, Width = 4 }
            };
            _lineStyleSucceeded = new VectorStyle
            {
                Fill = null,
                Outline = null,
                Line = { Color = Color.Green, Width = 4 }
            };
            _lineStyleSelected = new VectorStyle
            {
                Fill = null,
                Outline = null,
                 Line = {Color = Color.Blue, Width = 6}
            };

            this.Command_ResetCamera = new DelegateCommand(this.OnCommand_ResetCamera);
            this.Command_FocusSelectedTour = new DelegateCommand(
                this.OnCommand_FocusSelectedTour,
                () => _selectedTours.Count > 0);

            this.AdditionalMapLayers = new ObservableCollection<ILayer>();
            this.AdditionalMapLayers.Add(_layerSelectedGpxFiles);
            this.AdditionalMapLayers.Add(_layerLoadedGpxFiles);
        }

        /// <inheritdoc />
        protected override void OnMvvmViewAttached()
        {
            base.OnMvvmViewAttached();

            // Get initial viewport position
            var initialBoundingBox = GetDefaultBoundingBox();
            if ((_config.LastViewportMaxX != 0) &&
                (_config.LastViewportMaxY != 0) &&
                (_config.LastViewportMinX != 0) &&
                (_config.LastViewportMinY != 0))
            {
                initialBoundingBox = new BoundingBox(
                    _config.LastViewportMinX, _config.LastViewportMinY,
                    _config.LastViewportMaxX, _config.LastViewportMaxY);
            }

            // Set initial viewport position
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += (_, _) =>
            {
                timer.Stop();
                this.RequestNavigateToBoundingBox?.Invoke(
                    this,
                    new RequestNavigateToBoundingBoxEventArgs(initialBoundingBox));
            };
            timer.Start();
        }

        public static BoundingBox GetDefaultBoundingBox()
        {
            return new BoundingBox(
                -7637335.834571721, 1578946.9985836577,
                9063849.0976259485, 10564256.424126934);
        }

        public void ResetCamera(IEnumerable<ILoadedGpxFileTourInfo>? toursToFocus)
        {
            var boxBuilder = new NavigationBoundingBoxBuilder();
            if (toursToFocus != null)
            {
                foreach (var actTour in toursToFocus)
                {
                    foreach (var actTrackSegment in actTour.Segments)
                    {
                        var actGeometry = actTrackSegment.Points.GpxWaypointsToMapsuiGeometry();
                        if (actGeometry == null) { continue; }

                        boxBuilder.AddGeometry(actGeometry);
                    }
                }
            }

            if (boxBuilder.CanBuildBoundingBox)
            {
                this.RequestNavigateToBoundingBox?.Invoke(
                    this, 
                    new RequestNavigateToBoundingBoxEventArgs(boxBuilder.TryBuild()!));
            }
            else
            {
                this.RequestNavigateToBoundingBox?.Invoke(
                    this,
                    new RequestNavigateToBoundingBoxEventArgs(GetDefaultBoundingBox()));
            }
        }

        private void UpdateLayer_LoadedGpxFiles()
        {
            var newFeatureList = new List<IFeature>();
            foreach (var actTour in _loadedTours)
            {
                foreach (var actTrackSegment in actTour.Segments)
                {
                    var actGeometry = actTrackSegment.Points.GpxWaypointsToMapsuiGeometry();
                    if (actGeometry == null) { continue; }

                    newFeatureList.Add(new Feature()
                    {
                        Geometry = actGeometry,
                        Styles =
                        {
                            actTour.RawTourExtensionData.State == GpxTrackState.Succeeded ? _lineStyleSucceeded : _lineStyleInitial
                        }
                    });
                }
            }
            _layerLoadedGpxFilesProvider.ReplaceFeatures(newFeatureList);
            _layerLoadedGpxFiles.DataHasChanged();

            this.ResetCamera(_loadedTours);
        }

        private void UpdateLayer_SelectedGpxFiles()
        {
            var boxBuilder = new NavigationBoundingBoxBuilder();

            var newFeatureList = new List<IFeature>();
            foreach (var actTour in _selectedTours)
            {
                foreach (var actTourSegment in actTour.Segments)
                {
                    var actGeometry = actTourSegment.Points.GpxWaypointsToMapsuiGeometry();
                    if (actGeometry == null) { continue; }

                    boxBuilder.AddGeometry(actGeometry);

                    newFeatureList.Add(new Feature()
                    {
                        Geometry = actGeometry,
                        Styles = {_lineStyleSelected}
                    });
                }
            }
            _layerSelectedGpxFilesProvider.ReplaceFeatures(newFeatureList);
            _layerSelectedGpxFiles.DataHasChanged();

            var currentViewPort = this.TryGetCurrentViewport();
            var selectedRouteBox = boxBuilder.TryBuild();
            if ((selectedRouteBox != null) &&
                (currentViewPort != null) &&
                (!currentViewPort.Contains(selectedRouteBox)))
            {
                this.ResetCamera(_selectedTours);
            }
        }

        private BoundingBox? TryGetCurrentViewport()
        {
            var eArgs = new RequestCurrentViewportEventArgs();
            this.RequestCurrentViewport?.Invoke(this, eArgs);
            return eArgs.CurrentViewPort;
        }

        private void OnCommand_ResetCamera()
        {
            this.ResetCamera(_loadedTours);
        }

        private void OnCommand_FocusSelectedTour()
        {
            this.ResetCamera(_selectedTours);
        }

        private void OnMessageReceived(MessageGpxFileRepositoryContentsChanged message)
        {
            if (message.RemovedNodes != null)
            {
                foreach(var actRemovedNode in message.RemovedNodes)
                {
                    foreach (var actTour in actRemovedNode.GetAllAssociatedTours())
                    {
                        _loadedTours.Remove(actTour);
                    }
                }
            }

            if(message.AddedNodes != null)
            {
                foreach(var actAddedNode in message.AddedNodes)
                {
                    foreach(var actTour in actAddedNode.GetAllAssociatedTours())
                    {
                        _loadedTours.Add(actTour);
                    }
                }
            }

            this.UpdateLayer_LoadedGpxFiles();
        }

        private void OnMessageReceived(MessageGpxFileRepositoryNodeSelectionChanged message)
        {
            _selectedTours.Clear();

            if(message.SelectedNodes != null)
            {
                foreach (var actSelectedNode in message.SelectedNodes)
                {
                    _selectedTours.AddRange(actSelectedNode.GetAllAssociatedTours());
                }
            }

            this.UpdateLayer_SelectedGpxFiles();

            this.Command_FocusSelectedTour.RaiseCanExecuteChanged();
        }

        private void OnMessageReceived(MessageFocusFileRepositoryNodeRequest message)
        {
            this.ResetCamera(message.Node.GetAllAssociatedTours());
        }

        private void OnMessageReceived(MessageTourConfigurationChanged message)
        {
            
        }

        private void OnMessageReceived(MessageGpxViewerExitPreview message)
        {
            var currentViewPort = this.TryGetCurrentViewport();
            if (currentViewPort != null)
            {
                _config.LastViewportMinX = currentViewPort.MinX;
                _config.LastViewportMinY = currentViewPort.MinY;
                _config.LastViewportMaxX = currentViewPort.MaxX;
                _config.LastViewportMaxY = currentViewPort.MaxY;
            }
        }
    }
}