using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FirLib.Core.Utils.Collections;
using GpxViewer.Core;
using GpxViewer.Core.GpxExtensions;
using GpxViewer.Core.Patterns;
using GpxViewer.Modules.GpxFiles.Logic;

namespace GpxViewer.Modules.GpxFiles.Views
{
    internal class FileTreeNodeViewModel : GpxViewerViewModelBase
    {
        public GpxFileRepositoryNode Model { get; }

        public string NodeText => this.Model.NodeText;

        public LoadedGpxFile? AssociatedGpxFile => this.Model.GetAssociatedGpxFile() as LoadedGpxFile;

        public TransformedObservableCollection<FileTreeNodeViewModel, GpxFileRepositoryNode> ChildNodes { get; }

        public GpxViewerIconKind IconKind
        {
            get
            {
                if (this.Model.GetAssociatedTour() != null) { return GpxViewerIconKind.Tour; }
                else if (this.Model.GetAssociatedGpxFile() != null) { return GpxViewerIconKind.GpxFile; }
                else { return GpxViewerIconKind.Folder; }
            }
        }

        public Visibility TourStatsVisibility =>
            this.Model.GetAssociatedGpxFile() != null ? Visibility.Visible : Visibility.Collapsed;

        public double ElevationUpMeters => 
            this.Model.GetAssociatedToursDeep()?.Sum(actTour => actTour.ElevationUpMeters) ?? 0.0;

        public double ElevationDownMeters =>
            this.Model.GetAssociatedToursDeep()?.Sum(actTour => actTour.ElevationDownMeters) ?? 0.0;

        public double DistanceKm =>
            this.Model.GetAssociatedToursDeep()?.Sum(actTour => actTour.DistanceKm) ?? 0.0;

        public Visibility TourFinishedVisibility =>
            this.Model.GetAssociatedToursDeep()?.All(actTour => actTour.RawTourExtensionData.State == GpxTrackState.Succeeded) ?? false
                ? Visibility.Visible
                : Visibility.Collapsed;

        public FileTreeNodeViewModel(GpxFileRepositoryNode model)
        {
            this.Model = model;
            this.ChildNodes = new TransformedObservableCollection<FileTreeNodeViewModel, GpxFileRepositoryNode>(
                model.ChildNodes,
                nodeModel => new FileTreeNodeViewModel(nodeModel));
        }

        public void RaiseNodeTextChanged()
        {
            this.RaisePropertyChanged(nameof(this.NodeText));
            this.RaisePropertyChanged(nameof(this.TourFinishedVisibility));
        }
    }
}
