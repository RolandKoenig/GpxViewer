using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using FirLib.Core.Patterns;
using FirLib.Core.ViewServices;
using GpxViewer.Core.Patterns;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Views.TourViews
{
    internal class SelectedToursViewModel : GpxViewerViewModelBase
    {
        public ObservableCollection<SelectedTourViewModel> SelectedTours { get; }
        public ObservableCollection<ILoadedGpxFileWaypointInfo> SelectedFileWaypoints { get; }

        public Visibility SelectedToursNormalViewVisibility
        {
            get
            {
                if (SelectedTours.Count == 1) { return Visibility.Visible; }
                return Visibility.Collapsed;
            }
        }

        public Visibility SelectedToursErrorViewVisibility
        {
            get
            {
                if (SelectedTours.Count > 0) { return Visibility.Collapsed; }

                if (!string.IsNullOrEmpty(ErrorTextCompact)) { return Visibility.Visible; }
                else { return Visibility.Collapsed; }
            }
        }

        public string ErrorTextCompact { get; set; } = string.Empty;

        public Exception? ErrorDetails { get; set; }

        public DelegateCommand Command_ShowErrorDetails { get; }

        public SelectedToursViewModel()
        {
            SelectedTours = new ObservableCollection<SelectedTourViewModel>();
            SelectedFileWaypoints = new ObservableCollection<ILoadedGpxFileWaypointInfo>();

            Command_ShowErrorDetails = new DelegateCommand(
                () =>
                {
                    if (ErrorDetails == null) { return; }

                    var srvErrorDialog = GetViewService<IErrorDialogService>();
                    srvErrorDialog.ShowAsync(ErrorDetails);
                },
                () => ErrorDetails != null);
        }

        private void OnMessageReceived(MessageGpxFileRepositoryNodeSelectionChanged message)
        {
            SelectedTours.Clear();
            SelectedFileWaypoints.Clear();
            ErrorTextCompact = string.Empty;
            ErrorDetails = null;

            if (message.SelectedNodes != null)
            {
                foreach (var actSelectedNode in message.SelectedNodes)
                {
                    if (actSelectedNode.HasError)
                    {
                        var errorDetails = actSelectedNode.GetErrorDetails();
                        if (errorDetails != null)
                        {
                            ErrorTextCompact = errorDetails.Message;
                            ErrorDetails = errorDetails;
                        }
                        else
                        {
                            ErrorTextCompact = "Unknown error";
                        }
                        continue;
                    }

                    foreach (var actTour in actSelectedNode.GetAssociatedToursDeep())
                    {
                        SelectedTours.Add(new SelectedTourViewModel(actTour));
                    }

                    var associatedGpxFile = actSelectedNode.GetAssociatedGpxFile();
                    if (associatedGpxFile != null)
                    {
                        foreach (var actWaypoint in associatedGpxFile.Waypoints)
                        {
                            SelectedFileWaypoints.Add(actWaypoint);
                        }
                    }
                }
            }

            RaisePropertyChanged(nameof(SelectedToursNormalViewVisibility));
            RaisePropertyChanged(nameof(SelectedToursErrorViewVisibility));
            RaisePropertyChanged(nameof(ErrorDetails));
            RaisePropertyChanged(nameof(ErrorTextCompact));
            Command_ShowErrorDetails.RaiseCanExecuteChanged();
        }
    }
}
