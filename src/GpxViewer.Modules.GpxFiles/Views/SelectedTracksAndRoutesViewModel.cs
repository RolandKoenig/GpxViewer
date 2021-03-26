using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GpxViewer.Core.Patterns;
using GpxViewer.Modules.GpxFiles.Interface.Messages;

namespace GpxViewer.Modules.GpxFiles.Views
{
    internal class SelectedTracksAndRoutesViewModel : GpxViewerViewModelBase
    {
        public ObservableCollection<SelectedTrackOrRouteViewModel> SelectedTracksAndRoutes { get; }
 
        public SelectedTracksAndRoutesViewModel()
        {
            this.SelectedTracksAndRoutes = new ObservableCollection<SelectedTrackOrRouteViewModel>();
        }

        private void OnMessageReceived(MessageGpxFileRepositoryNodeSelectionChanged message)
        {
            this.SelectedTracksAndRoutes.Clear();

            if (message.SelectedNodes != null)
            {
                foreach(var actSelectedNode in message.SelectedNodes)
                {
                    foreach (var actTrackOrRoute in actSelectedNode.GetAllAssociatedTracksAndRoutes())
                    {
                        this.SelectedTracksAndRoutes.Add(new SelectedTrackOrRouteViewModel(actTrackOrRoute));
                    }
                }
            }
        }
    }
}
