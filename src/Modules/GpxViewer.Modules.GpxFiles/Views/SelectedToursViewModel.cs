using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GpxViewer.Core.Patterns;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Views
{
    internal class SelectedToursViewModel : GpxViewerViewModelBase
    {
        public ObservableCollection<SelectedTourViewModel> SelectedTours { get; }
        public ObservableCollection<ILoadedGpxFileWaypointInfo> SelectedFileWaypoints { get; }

        public Visibility SelectedToursEditVisibility =>
            this.SelectedTours.Count == 1 ? Visibility.Visible : Visibility.Collapsed;

        public SelectedToursViewModel()
        {
            this.SelectedTours = new ObservableCollection<SelectedTourViewModel>();
            this.SelectedFileWaypoints = new ObservableCollection<ILoadedGpxFileWaypointInfo>();
        }

        private void OnMessageReceived(MessageGpxFileRepositoryNodeSelectionChanged message)
        {
            this.SelectedTours.Clear();
            this.SelectedFileWaypoints.Clear();

            if (message.SelectedNodes != null)
            {
                foreach(var actSelectedNode in message.SelectedNodes)
                {
                    foreach (var actTour in actSelectedNode.GetAssociatedToursDeep())
                    {
                        this.SelectedTours.Add(new SelectedTourViewModel(actTour));
                    }

                    foreach (var actFile in actSelectedNode.GetAssociatedGpxFilesDeep())
                    {
                        foreach (var actWaypoint in actFile.Waypoints)
                        {
                            this.SelectedFileWaypoints.Add(actWaypoint);
                        }
                    }
                }
            }

            this.RaisePropertyChanged(nameof(this.SelectedToursEditVisibility));
        }
    }
}
