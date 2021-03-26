using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Patterns.Mvvm;
using GpxViewer.Core.GpxExtensions;
using GpxViewer.Core.Patterns;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Views
{
    internal class SelectedTrackOrRouteViewModel : GpxViewerViewModelBase
    {
        private ILoadedGpxFileTrackOrRouteInfo _trackOrRoute;

        public GpxTrackState State
        {
            get => _trackOrRoute.State;
            set
            {
                if (_trackOrRoute.State != value)
                {
                    _trackOrRoute.State = value;

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
