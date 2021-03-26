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
    internal class SelectedTrackOrRouteViewModel : GpxViewerViewModelBase
    {
        private ILoadedGpxFileTrackOrRouteInfo _trackOrRoute;

        public string Name
        {
            get => _trackOrRoute.RawTrackOrRoute.Name ?? string.Empty;
            set
            {
                if (_trackOrRoute.RawTrackOrRoute.Name != value)
                {
                    _trackOrRoute.RawTrackOrRoute.Name = value;
                    _trackOrRoute.File.ContentsChanged = true;

                    this.Messenger.BeginPublish(
                        new MessageTrackOrRouteConfigurationChanged(_trackOrRoute));
                }
            }
        }

        public string Description
        {
            get => _trackOrRoute.RawTrackOrRoute.Description ?? string.Empty;
            set
            {
                if (_trackOrRoute.RawTrackOrRoute.Description != value)
                {
                    _trackOrRoute.RawTrackOrRoute.Description = value;
                    _trackOrRoute.File.ContentsChanged = true;

                    this.Messenger.BeginPublish(
                        new MessageTrackOrRouteConfigurationChanged(_trackOrRoute));
                }
            }
        }

        public GpxTrackState State
        {
            get => _trackOrRoute.RawTrackExtensionData.State;
            set
            {
                if (_trackOrRoute.RawTrackExtensionData.State != value)
                {
                    _trackOrRoute.RawTrackExtensionData.State = value;
                    _trackOrRoute.File.ContentsChanged = true;

                    this.Messenger.BeginPublish(
                        new MessageTrackOrRouteConfigurationChanged(_trackOrRoute));
                }
            }
        }

        public SelectedTrackOrRouteViewModel(ILoadedGpxFileTrackOrRouteInfo trackOrRoute)
        {
            _trackOrRoute = trackOrRoute;
        }
    }
}
