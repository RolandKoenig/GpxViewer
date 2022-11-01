using GpxViewer.Core.GpxExtensions;
using GpxViewer.Core.Patterns;
using GpxViewer.Core.Utils;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Views
{
    internal class SelectedTourViewModel : GpxViewerViewModelBase
    {
        private ILoadedGpxFileTourInfo _tour;

        [LocalizableCategory(
            typeof(SelectedToursViewResources),
            nameof(SelectedToursViewResources.Category_Metadata))]
        [LocalizableDisplayName(
            typeof(SelectedToursViewResources),
            nameof(SelectedToursViewResources.Property_Name))]
        public string Name
        {
            get => _tour.RawTrackOrRoute.Name ?? string.Empty;
            set
            {
                if (_tour.RawTrackOrRoute.Name != value)
                {
                    _tour.RawTrackOrRoute.Name = value;
                    _tour.File.ContentsChanged = true;

                    this.Messenger.BeginPublish(
                        new MessageTourConfigurationChanged(_tour));
                }
            }
        }

        [LocalizableCategory(
            typeof(SelectedToursViewResources),
            nameof(SelectedToursViewResources.Category_Metadata))]
        [LocalizableDisplayName(
            typeof(SelectedToursViewResources),
            nameof(SelectedToursViewResources.Property_Description))]
        public string Description
        {
            get => _tour.RawTrackOrRoute.Description ?? string.Empty;
            set
            {
                if (_tour.RawTrackOrRoute.Description != value)
                {
                    _tour.RawTrackOrRoute.Description = value;
                    _tour.File.ContentsChanged = true;

                    this.Messenger.BeginPublish(
                        new MessageTourConfigurationChanged(_tour));
                }
            }
        }

        [LocalizableCategory(
            typeof(SelectedToursViewResources),
            nameof(SelectedToursViewResources.Category_Metadata))]
        [LocalizableDisplayName(
            typeof(SelectedToursViewResources),
            nameof(SelectedToursViewResources.Property_State))]
        public GpxTrackState State
        {
            get => _tour.RawTourExtensionData.State;
            set
            {
                if (_tour.RawTourExtensionData.State != value)
                {
                    _tour.RawTourExtensionData.State = value;
                    _tour.File.ContentsChanged = true;

                    this.Messenger.BeginPublish(
                        new MessageTourConfigurationChanged(_tour));
                }
            }
        }

        [LocalizableCategory(
            typeof(SelectedToursViewResources),
            nameof(SelectedToursViewResources.Category_Metrics))]
        [LocalizableDisplayName(
            typeof(SelectedToursViewResources),
            nameof(SelectedToursViewResources.Property_DistanceKm))]
        public string DistanceKm => _tour.DistanceKm.ToString("N1");

        [LocalizableCategory(
            typeof(SelectedToursViewResources),
            nameof(SelectedToursViewResources.Category_Metrics))]
        [LocalizableDisplayName(
            typeof(SelectedToursViewResources),
            nameof(SelectedToursViewResources.Property_ElevationUpMeters))]
        public string ElevationUpMeters => _tour.ElevationUpMeters.ToString("N0");

        [LocalizableCategory(
            typeof(SelectedToursViewResources),
            nameof(SelectedToursViewResources.Category_Metrics))]
        [LocalizableDisplayName(
            typeof(SelectedToursViewResources),
            nameof(SelectedToursViewResources.Property_ElevationDownMeters))]
        public string ElevationDownMeters => _tour.ElevationDownMeters.ToString("N0");

        public SelectedTourViewModel(ILoadedGpxFileTourInfo tour)
        {
            _tour = tour;
        }
    }
}
