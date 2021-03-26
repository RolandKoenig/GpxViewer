using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpxViewer.Core;
using GpxViewer.Core.GpxExtensions;
using GpxViewer.Core.Patterns;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using GpxViewer.Modules.GpxFiles.Interface.Model;
using Mapsui.Layers;
using Mapsui.Providers;
using Mapsui.Styles;

namespace GpxViewer.Modules.Map.Views
{
    public class MapViewModel : GpxViewerViewModelBase
    {
        private IGpxFileRepository _gpxFileRepo;

        private MemoryLayer _layerLoadedGpxFiles;
        private MemoryProvider _layerLoadedGpxFilesProvider;
        private List<ILoadedGpxFile> _loadedGpxFiles;

        private MemoryLayer _layerSelectedGpxFiles;
        private MemoryProvider _layerSelectedGpxFilesProvider;
        private List<ILoadedGpxFile> _selectedGpxFiles;

        private VectorStyle _lineStyleSelected;
        private VectorStyle _lineStyleInitial;
        private VectorStyle _lineStyleSucceeded;

        public ObservableCollection<ILayer> AdditionalMapLayers { get; }

        public MapViewModel(IGpxFileRepository gpxFileRepo)
        {
            _gpxFileRepo = gpxFileRepo;

            _layerLoadedGpxFiles = new MemoryLayer();
            _layerLoadedGpxFilesProvider = new MemoryProvider();
            _layerLoadedGpxFiles.DataSource = _layerLoadedGpxFilesProvider;
            _loadedGpxFiles = new List<ILoadedGpxFile>();

            _layerSelectedGpxFiles = new MemoryLayer();
            _layerSelectedGpxFilesProvider = new MemoryProvider();
            _layerSelectedGpxFiles.DataSource = _layerSelectedGpxFilesProvider;
            _selectedGpxFiles = new List<ILoadedGpxFile>();

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

            this.AdditionalMapLayers = new ObservableCollection<ILayer>();
            this.AdditionalMapLayers.Add(_layerSelectedGpxFiles);
            this.AdditionalMapLayers.Add(_layerLoadedGpxFiles);
        }

        private void UpdateLayer_LoadedGpxFiles()
        {
            var newFeatureList = new List<IFeature>();
            foreach(var actLoadedFile in _loadedGpxFiles)
            {
                foreach (var actTrackOrRoute in actLoadedFile.TracksAndRoutes)
                {
                    foreach (var actTrackSegment in actTrackOrRoute.Segments)
                    {
                        var actGeometry = actTrackSegment.Points.GpxWaypointsToMapsuiGeometry();
                        if (actGeometry == null) { continue; }

                        newFeatureList.Add(new Feature()
                        {
                            Geometry = actGeometry,
                            Styles =
                            {
                                actTrackOrRoute.State == GpxTrackState.Succeeded ? _lineStyleSucceeded : _lineStyleInitial
                            }
                        });
                    }
                }
            }
            _layerLoadedGpxFilesProvider.ReplaceFeatures(newFeatureList);
            _layerLoadedGpxFiles.DataHasChanged();
        }

        private void UpdateLayer_SelectedGpxFiles()
        {
            var newFeatureList = new List<IFeature>();
            foreach(var actLoadedFile in _selectedGpxFiles)
            {
                foreach (var actTrack in actLoadedFile.TracksAndRoutes)
                {
                    foreach (var actTrackSegment in actTrack.Segments)
                    {
                        var actGeometry = actTrackSegment.Points.GpxWaypointsToMapsuiGeometry();
                        if (actGeometry == null) { continue; }

                        newFeatureList.Add(new Feature()
                        {
                            Geometry = actGeometry,
                            Styles = { _lineStyleSelected }
                        });
                    }
                }
            }
            _layerSelectedGpxFilesProvider.ReplaceFeatures(newFeatureList);
            _layerSelectedGpxFiles.DataHasChanged();
        }

        private void OnMessageReceived(MessageGpxFileRepositoryContentsChanged message)
        {
            if (message.RemovedNodes != null)
            {
                foreach(var actRemovedNode in message.RemovedNodes)
                {
                    foreach (var actGpxFile in actRemovedNode.GetAllAssociatedGpxFiles())
                    {
                        _loadedGpxFiles.Remove(actGpxFile);
                    }
                }
            }

            if(message.AddedNodes != null)
            {
                foreach(var actAddedNode in message.AddedNodes)
                {
                    foreach(var actAddedGpxFile in actAddedNode.GetAllAssociatedGpxFiles())
                    {
                        _loadedGpxFiles.Add(actAddedGpxFile);
                    }
                }
            }

            this.UpdateLayer_LoadedGpxFiles();
        }

        private void OnMessageReceived(MessageGpxFileRepositoryNodeSelectionChanged message)
        {
            _selectedGpxFiles.Clear();

            if(message.SelectedNodes != null)
            {
                foreach (var actSelectedNode in message.SelectedNodes)
                {
                    _selectedGpxFiles.AddRange(actSelectedNode.GetAllAssociatedGpxFiles());
                }
            }

            this.UpdateLayer_SelectedGpxFiles();
        }
    }
}
