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

            this.AdditionalMapLayers = new ObservableCollection<ILayer>();
            this.AdditionalMapLayers.Add(_layerLoadedGpxFiles);
        }

        private void UpdateLayer_LoadedGpxFiles()
        {
            var newFeatureList = new List<IFeature>();
            foreach(var actLoadedFile in _loadedGpxFiles)
            {
                foreach (var actTrack in actLoadedFile.Tracks)
                {
                    foreach (var actTrackSegment in actTrack.RawTrackData.Segments)
                    {
                        var actGeometry = actTrackSegment.Points.GpxWaypointsToMapsuiGeometry();
                        if (actGeometry == null) { continue; }

                        newFeatureList.Add(new Feature()
                        {
                            Geometry = actGeometry,
                            Styles =
                            {
                                actTrack.State == GpxTrackState.Succeeded ? _lineStyleSucceeded : _lineStyleInitial
                            }
                        });
                    }
                }
            }
            _layerLoadedGpxFilesProvider.ReplaceFeatures(newFeatureList);
            _layerLoadedGpxFiles.DataHasChanged();
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
    }
}
