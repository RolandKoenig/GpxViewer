using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpxViewer.Core.Messages;
using GpxViewer.Core.Model;
using GpxViewer.Core.Patterns;
using Mapsui.Layers;
using Mapsui.Providers;

namespace GpxViewer.Modules.Map.Views
{
    public class MapViewModel : GpxViewerViewModelBase
    {
        private IGpxFileRepository _gpxFileRepo;

        private MemoryLayer _layerLoadedGpxFiles;
        private MemoryProvider _layerLoadedGpxFilesProvider;
        private List<IFeature> _layerLoadedGpxFilesFeatures;
        private Dictionary<IGpxFileRepositoryNode, IFeature> _layerLoadedGpxFilesMapper;
        //private MemoryLayer _layerSelectedGpxFiles;

        public ObservableCollection<ILayer> AddditionalMapLayers { get; }

        public MapViewModel(IGpxFileRepository gpxFileRepo)
        {
            _gpxFileRepo = gpxFileRepo;

            _layerLoadedGpxFiles = new MemoryLayer();
            _layerLoadedGpxFilesProvider = new MemoryProvider();
            _layerLoadedGpxFiles.DataSource = _layerLoadedGpxFilesProvider;
            _layerLoadedGpxFilesFeatures = new List<IFeature>();
            _layerLoadedGpxFilesMapper = new Dictionary<IGpxFileRepositoryNode, IFeature>();

            this.AddditionalMapLayers = new ObservableCollection<ILayer>();
            this.AddditionalMapLayers.Add(_layerLoadedGpxFiles);
        }

        private void OnMessageReceived(MessageGpxFileRepositoryContentsChanged message)
        {
            if (message.RemovedNodes != null)
            {
                foreach(var actRemovedNode in message.RemovedNodes)
                {
                    if(!_layerLoadedGpxFilesMapper.ContainsKey(actRemovedNode)){ continue; }
                    var actRemovedFeature = _layerLoadedGpxFilesMapper[actRemovedNode];
                    _layerLoadedGpxFilesFeatures.Remove(actRemovedFeature);
                }
            }

            if(message.AddedNodes != null)
            {
                foreach(var actAddedNode in message.AddedNodes)
                {
         
                }
            }
        }
    }
}
