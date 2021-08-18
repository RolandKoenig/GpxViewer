using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public string DistanceKm => _tour.DistanceKm.ToString("N1");

        public string ElevationUpMeters => _tour.ElevationUpMeters.ToString("N0");

        public string ElevationDownMeters => _tour.ElevationDownMeters.ToString("N0");

        public SelectedTourViewModel(ILoadedGpxFileTourInfo tour)
        {
            _tour = tour;
        }
    }
}
