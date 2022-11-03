using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using FirLib.Core.Patterns;
using FirLib.Core.Utils.IO.AssemblyResources;
using GpxViewer.Core.GpxExtensions;
using GpxViewer.Core.Messages;
using GpxViewer.Core.Patterns;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using GpxViewer.Modules.GpxFiles.Interface.Model;
using GpxViewer.Modules.Map.Util;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;

namespace GpxViewer.Modules.Map.Views
{
    internal class MapViewModel : GpxViewerViewModelBase
    {
        private MapModuleConfiguration _config;

        private MemoryLayer _layerLoadedGpxFiles;
        private MemoryProvider _layerLoadedGpxFilesProvider;
        private List<ILoadedGpxFileTourInfo> _loadedTours;

        private MemoryLayer _layerSelectedGpxFiles;
        private MemoryProvider _layerSelectedGpxFilesProvider;
        private List<ILoadedGpxFileTourInfo> _selectedTours;

        private MemoryLayer _layerPoints;
        private MemoryProvider _layerPointsProvider;

        private VectorStyle _lineStyleSelected;
        private VectorStyle _lineStyleInitial;
        private VectorStyle _lineStylePlanned;
        private VectorStyle _lineStyleSucceeded;

        public ObservableCollection<ILayer> AdditionalMapLayers { get; }

        public MapViewSettings ViewSettings { get; }

        public DelegateCommand Command_ResetCamera { get; }

        public DelegateCommand Command_FocusSelectedTour { get; }

        public event EventHandler<RequestNavigateToBoundingBoxEventArgs>? RequestNavigateToBoundingBox;

        public event EventHandler<RequestCurrentViewportEventArgs>? RequestCurrentViewport;

        public MapViewModel(MapModuleConfiguration config)
        {
            _config = config;

            this.ViewSettings = new MapViewSettings(_config);

            _layerLoadedGpxFiles = new MemoryLayer();
            _layerLoadedGpxFiles.IsMapInfoLayer = true;
            _layerLoadedGpxFilesProvider = new MemoryProvider();
            _layerLoadedGpxFiles.DataSource = _layerLoadedGpxFilesProvider;
            _loadedTours = new List<ILoadedGpxFileTourInfo>();

            _layerSelectedGpxFiles = new MemoryLayer();
            _layerSelectedGpxFiles.IsMapInfoLayer = true;
            _layerSelectedGpxFilesProvider = new MemoryProvider();
            _layerSelectedGpxFiles.DataSource = _layerSelectedGpxFilesProvider;
            _selectedTours = new List<ILoadedGpxFileTourInfo>();

            _layerPoints = new MemoryLayer();
            _layerPointsProvider = new MemoryProvider();
            _layerPoints.DataSource = _layerPointsProvider;
            _layerPoints.Style = new SymbolStyle()
            {
                BitmapId = BitmapRegistry.Instance.Register(
                    new AssemblyResourceLink(typeof(MapModule), "Assets", "TourStartGray.png").OpenRead()),
                SymbolScale = 0.15,
                Opacity = 0.9f,
                SymbolOffset = new Offset(0, 88)
            };

            _lineStyleInitial = new VectorStyle
            {
                Fill = null,
                Outline = null,
                Line = { Color = Color.Gray, Width = 4 }
            };
            _lineStylePlanned = new VectorStyle()
            {
                Fill = null,
                Outline = null,
                Line = { Color = Color.Yellow, Width = 4 }
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
            this.AdditionalMapLayers.Add(_layerPoints);
        }

        public void OnGpxTourSelected(ILoadedGpxFileTourInfo? selectedTour)
        {
            this.Messenger.Publish(
                new MessageSelectGpxTourRequest(selectedTour));
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

                if (_loadedTours.Count > 0) { return; }
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
                    foreach (var actTourSegment in actTour.Segments)
                    {
                        var actGeometry = actTourSegment.Points.GpxWaypointsToMapsuiGeometry(actTour, actTourSegment);
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

        private VectorStyle GetLineStyle(GpxTrackState trackState)
        {
            switch (trackState)
            {
                case GpxTrackState.Succeeded: return _lineStyleSucceeded;
                case GpxTrackState.Planned: return _lineStylePlanned;
                case GpxTrackState.Unknown: return _lineStyleInitial;
            }

            return _lineStyleInitial;
        }

        private void UpdateLayer_Points()
        {
            var newFeaturesNormalPoints = new List<IFeature>();

            var selectedTour = _selectedTours.FirstOrDefault();
            if ((selectedTour != null) &&
                (_selectedTours.Count == 1))
            {
                foreach (var actWaypoint in selectedTour.Waypoints)
                {
                    // Create point
                    var point = SphericalMercator.FromLonLat(
                        actWaypoint.RawWaypointData.Longitude,
                        actWaypoint.RawWaypointData.Latitude);
                    var feature = new Feature();
                    feature.Geometry = point;
                    feature.Styles.Add(new LabelStyle()
                    {
                        Text = actWaypoint.RawWaypointData.Name, 
                        Opacity = 0.8f,
                        Offset = new Offset(0, 1.0, isRelative:true)
                    });
                    newFeaturesNormalPoints.Add(feature);
                }
            }

            _layerPointsProvider.ReplaceFeatures(newFeaturesNormalPoints);
            _layerPoints.DataHasChanged();
        }

        private void UpdateLayer_LoadedGpxFiles()
        {
            var newFeatures = new List<IFeature>();
            foreach (var actTour in _loadedTours)
            {
                foreach (var actTourSegment in actTour.Segments)
                {
                    var actGeometry = actTourSegment.Points.GpxWaypointsToMapsuiGeometry(actTour, actTourSegment);
                    if (actGeometry == null) { continue; }

                    newFeatures.Add(new Feature()
                    {
                        Geometry = actGeometry,
                        Styles =
                        {
                            this.GetLineStyle(actTour.RawTourExtensionData.State)
                        }
                    });
                }
            }
            _layerLoadedGpxFilesProvider.ReplaceFeatures(newFeatures);
            _layerLoadedGpxFiles.DataHasChanged();
        }

        private void UpdateLayer_SelectedGpxFiles()
        {
            var boxBuilder = new NavigationBoundingBoxBuilder();

            var newFeatures = new List<IFeature>();
            foreach (var actTour in _selectedTours)
            {
                foreach (var actTourSegment in actTour.Segments)
                {
                    var actGeometry = actTourSegment.Points.GpxWaypointsToMapsuiGeometry(actTour, actTourSegment);
                    if (actGeometry == null) { continue; }

                    boxBuilder.AddGeometry(actGeometry);

                    newFeatures.Add(new Feature()
                    {
                        Geometry = actGeometry,
                        Styles = {_lineStyleSelected}
                    });
                }
            }
            _layerSelectedGpxFilesProvider.ReplaceFeatures(newFeatures);
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
                    foreach (var actTour in actRemovedNode.GetAssociatedToursDeep())
                    {
                        _loadedTours.Remove(actTour);
                    }
                }
            }

            if(message.AddedNodes != null)
            {
                foreach(var actAddedNode in message.AddedNodes)
                {
                    foreach(var actTour in actAddedNode.GetAssociatedToursDeep())
                    {
                        _loadedTours.Add(actTour);
                    }
                }
            }

            this.UpdateLayer_LoadedGpxFiles();
            this.UpdateLayer_Points();
            this.ResetCamera(_loadedTours);
        }

        private void OnMessageReceived(MessageGpxFileRepositoryNodeSelectionChanged message)
        {
            _selectedTours.Clear();
            if(message.SelectedNodes != null)
            {
                foreach (var actSelectedNode in message.SelectedNodes)
                {
                    _selectedTours.AddRange(actSelectedNode.GetAssociatedToursDeep());
                }
            }

            this.UpdateLayer_SelectedGpxFiles();
            this.UpdateLayer_Points();

            this.Command_FocusSelectedTour.RaiseCanExecuteChanged();
        }

        private void OnMessageReceived(MessageFocusFileRepositoryNodeRequest message)
        {
            this.ResetCamera(message.Node.GetAssociatedToursDeep());
        }

        private void OnMessageReceived(MessageTourConfigurationChanged message)
        {
            this.UpdateLayer_LoadedGpxFiles();
            this.UpdateLayer_Points();
        }

        private void OnMessageReceived(MessageGpxViewerOnExitPreview message)
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
