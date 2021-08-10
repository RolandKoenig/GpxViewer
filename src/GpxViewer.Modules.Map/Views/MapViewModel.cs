using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using FirLib.Core.Patterns;
using FirLib.Core.Utils.ConfigurationFiles;
using GpxViewer.Core;
using GpxViewer.Core.GpxExtensions;
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

        private MemoryLayer _layerLoadedGpxFiles;
        private MemoryProvider _layerLoadedGpxFilesProvider;
        private List<ILoadedGpxFileTrackOrRouteInfo> _loadedTracksAndRoutes;

        private MemoryLayer _layerSelectedGpxFiles;
        private MemoryProvider _layerSelectedGpxFilesProvider;
        private List<ILoadedGpxFileTrackOrRouteInfo> _selectedTracksOrRoutes;

        private VectorStyle _lineStyleSelected;
        private VectorStyle _lineStyleInitial;
        private VectorStyle _lineStyleSucceeded;

        public ObservableCollection<ILayer> AdditionalMapLayers { get; }

        public DelegateCommand Command_ResetCamera { get; }

        public event EventHandler<RequestNavigateToBoundingBoxEventArgs>? RequestNavigateToBoundingBox;

        public MapViewModel(MapModuleConfiguration config, IGpxFileRepository gpxFileRepo)
        {
            _gpxFileRepo = gpxFileRepo;

            _layerLoadedGpxFiles = new MemoryLayer();
            _layerLoadedGpxFilesProvider = new MemoryProvider();
            _layerLoadedGpxFiles.DataSource = _layerLoadedGpxFilesProvider;
            _loadedTracksAndRoutes = new List<ILoadedGpxFileTrackOrRouteInfo>();

            _layerSelectedGpxFiles = new MemoryLayer();
            _layerSelectedGpxFilesProvider = new MemoryProvider();
            _layerSelectedGpxFiles.DataSource = _layerSelectedGpxFilesProvider;
            _selectedTracksOrRoutes = new List<ILoadedGpxFileTrackOrRouteInfo>();

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

            this.AdditionalMapLayers = new ObservableCollection<ILayer>();
            this.AdditionalMapLayers.Add(_layerSelectedGpxFiles);
            this.AdditionalMapLayers.Add(_layerLoadedGpxFiles);
        }

        /// <inheritdoc />
        protected override void OnMvvmViewAttached()
        {
            base.OnMvvmViewAttached();

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += (sender, eArgs) =>
            {
                timer.Stop();
                this.RequestNavigateToBoundingBox?.Invoke(
                    this,
                    new RequestNavigateToBoundingBoxEventArgs(GetDefaultBoundingBox()));
            };
            timer.Start();
        }

        public static BoundingBox GetDefaultBoundingBox()
        {
            return new BoundingBox(
                -7637335.834571721, 1578946.9985836577,
                9063849.0976259485, 10564256.424126934);
        }

        private void UpdateLayer_LoadedGpxFiles()
        {
            var boxBuilder = new NavigationBoundingBoxBuilder();

            var newFeatureList = new List<IFeature>();
            foreach (var actTrackOrRoute in _loadedTracksAndRoutes)
            {
                foreach (var actTrackSegment in actTrackOrRoute.Segments)
                {
                    var actGeometry = actTrackSegment.Points.GpxWaypointsToMapsuiGeometry();
                    if (actGeometry == null) { continue; }

                    boxBuilder.AddGeometry(actGeometry);
                    
                    newFeatureList.Add(new Feature()
                    {
                        Geometry = actGeometry,
                        Styles =
                        {
                            actTrackOrRoute.RawTrackExtensionData.State == GpxTrackState.Succeeded ? _lineStyleSucceeded : _lineStyleInitial
                        }
                    });
                }
            }
            _layerLoadedGpxFilesProvider.ReplaceFeatures(newFeatureList);
            _layerLoadedGpxFiles.DataHasChanged();

            if (boxBuilder.CanBuildBoundingBox)
            {
                this.RequestNavigateToBoundingBox?.Invoke(
                    this, 
                    new RequestNavigateToBoundingBoxEventArgs(boxBuilder.TryBuild()!));
            }
        }

        private void UpdateLayer_SelectedGpxFiles()
        {
            var boxBuilder = new NavigationBoundingBoxBuilder();

            var newFeatureList = new List<IFeature>();
            foreach (var actTrackOrRoute in _selectedTracksOrRoutes)
            {
                foreach (var actTrackSegment in actTrackOrRoute.Segments)
                {
                    var actGeometry = actTrackSegment.Points.GpxWaypointsToMapsuiGeometry();
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

            if (boxBuilder.CanBuildBoundingBox)
            {
                this.RequestNavigateToBoundingBox?.Invoke(
                    this, 
                    new RequestNavigateToBoundingBoxEventArgs(boxBuilder.TryBuild()!));
            }
        }

        private void OnCommand_ResetCamera()
        {
            var boxBuilder = new NavigationBoundingBoxBuilder();

            foreach (var actTrackOrRoute in _loadedTracksAndRoutes)
            {
                foreach (var actTrackSegment in actTrackOrRoute.Segments)
                {
                    var actGeometry = actTrackSegment.Points.GpxWaypointsToMapsuiGeometry();
                    if (actGeometry == null) { continue; }

                    boxBuilder.AddGeometry(actGeometry);
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

        private void OnMessageReceived(MessageGpxFileRepositoryContentsChanged message)
        {
            if (message.RemovedNodes != null)
            {
                foreach(var actRemovedNode in message.RemovedNodes)
                {
                    foreach (var actTrackOrRoute in actRemovedNode.GetAllAssociatedTracksAndRoutes())
                    {
                        _loadedTracksAndRoutes.Remove(actTrackOrRoute);
                    }
                }
            }

            if(message.AddedNodes != null)
            {
                foreach(var actAddedNode in message.AddedNodes)
                {
                    foreach(var actTrackOrRoute in actAddedNode.GetAllAssociatedTracksAndRoutes())
                    {
                        _loadedTracksAndRoutes.Add(actTrackOrRoute);
                    }
                }
            }

            this.UpdateLayer_LoadedGpxFiles();
        }

        private void OnMessageReceived(MessageGpxFileRepositoryNodeSelectionChanged message)
        {
            _selectedTracksOrRoutes.Clear();

            if(message.SelectedNodes != null)
            {
                foreach (var actSelectedNode in message.SelectedNodes)
                {
                    _selectedTracksOrRoutes.AddRange(actSelectedNode.GetAllAssociatedTracksAndRoutes());
                }
            }

            this.UpdateLayer_SelectedGpxFiles();
        }

        private void OnMessageReceived(MessageTrackOrRouteConfigurationChanged message)
        {
            
        }
    }
}
