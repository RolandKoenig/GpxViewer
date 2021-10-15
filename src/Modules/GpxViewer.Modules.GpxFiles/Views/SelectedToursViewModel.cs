using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using FirLib.Core.Patterns;
using GpxViewer.Core.Patterns;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Views
{
    internal class SelectedToursViewModel : GpxViewerViewModelBase
    {
        public ObservableCollection<SelectedTourViewModel> SelectedTours { get; }
        public ObservableCollection<ILoadedGpxFileWaypointInfo> SelectedFileWaypoints { get; }

        public Visibility SelectedToursNormalViewVisibility
        {
            get
            {
                if (this.SelectedTours.Count == 1) { return Visibility.Visible; }
                return Visibility.Collapsed;
            }
        }

        public Visibility SelectedToursErrorViewVisibility
        {
            get
            {
                if (this.SelectedTours.Count > 0) { return Visibility.Collapsed; }

                if (!string.IsNullOrEmpty(this.ErrorTextCompact)) { return Visibility.Visible; }
                else { return Visibility.Collapsed; }
            }
        }

        public string ErrorTextCompact { get; set; } = string.Empty;

        public Exception? ErrorDetails { get; set; }

        public DelegateCommand Command_ShowErrorDetails { get; }

        public SelectedToursViewModel()
        {
            this.SelectedTours = new ObservableCollection<SelectedTourViewModel>();
            this.SelectedFileWaypoints = new ObservableCollection<ILoadedGpxFileWaypointInfo>();

            this.Command_ShowErrorDetails = new DelegateCommand(
                () =>
                {
                    
                }, 
                () => this.ErrorDetails != null);
        }

        private void OnMessageReceived(MessageGpxFileRepositoryNodeSelectionChanged message)
        {
            this.SelectedTours.Clear();
            this.SelectedFileWaypoints.Clear();
            this.ErrorTextCompact = string.Empty;
            this.ErrorDetails = null;

            if (message.SelectedNodes != null)
            {
                foreach(var actSelectedNode in message.SelectedNodes)
                {
                    if (actSelectedNode.HasError)
                    {
                        var errorDetails = actSelectedNode.GetErrorDetails();
                        if (errorDetails != null)
                        {
                            this.ErrorTextCompact = errorDetails.Message;
                            this.ErrorDetails = errorDetails;
                        }
                        else
                        {
                            this.ErrorTextCompact = "Unknown error";
                        }
                        continue;
                    }

                    foreach (var actTour in actSelectedNode.GetAssociatedToursDeep())
                    {
                        this.SelectedTours.Add(new SelectedTourViewModel(actTour));
                    }

                    var associatedGpxFile = actSelectedNode.GetAssociatedGpxFile();
                    if (associatedGpxFile != null)
                    {
                        foreach (var actWaypoint in associatedGpxFile.Waypoints)
                        {
                            this.SelectedFileWaypoints.Add(actWaypoint);
                        }
                    }
                }
            }

            this.RaisePropertyChanged(nameof(this.SelectedToursNormalViewVisibility));
            this.RaisePropertyChanged(nameof(this.SelectedToursErrorViewVisibility));
            this.RaisePropertyChanged(nameof(this.ErrorDetails));
            this.RaisePropertyChanged(nameof(this.ErrorTextCompact));
            this.Command_ShowErrorDetails.RaiseCanExecuteChanged();
        }
    }
}
