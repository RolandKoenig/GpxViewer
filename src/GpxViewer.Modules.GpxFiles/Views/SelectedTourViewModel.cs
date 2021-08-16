using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpxViewer.Core.GpxExtensions;
using GpxViewer.Core.Patterns;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Views
{
    internal class SelectedTourViewModel : GpxViewerViewModelBase
    {
        private ILoadedGpxFileTourInfo _tour;

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

        public double DistanceKm => _tour.DistanceKm;

        public double ElevationUpMeters => _tour.ElevationUpMeters;

        public double ElevationDownMeters => _tour.ElevationDownMeters;

        public SelectedTourViewModel(ILoadedGpxFileTourInfo tour)
        {
            _tour = tour;
        }
    }
}
