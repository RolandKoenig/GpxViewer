using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using FirLib.Core.Patterns.Mvvm;
using GpxViewer.GpxExtensions;
using PropertyTools.DataAnnotations;
using FirLib.Formats.Gpx;

namespace GpxViewer.View.Map
{
    public class GpxTrackViewModel : ViewModelBase
    {
        private MapViewModel _mapViewModel;
        private bool _hasChanged;

        [Browsable(false)]
        public GpxFile GpxFile { get; }

        [Browsable(false)]
        public int TrackIndex { get; }

        [Browsable(false)]
        public GpxFileLayer GpxMapLayer { get; }

        [Browsable(false)]
        public string DisplayName
        {
            get
            {
                if (!_hasChanged) { return this.Name; }
                else { return $"{this.Name}*"; }
            }
        }

        [Browsable(false)]
        public string FilePath
        {
            get;
        }

        public string Name
        {
            get => this.GpxFile.Tracks[0].Name ?? string.Empty;
            set
            {
                if (this.Name != value)
                {
                    this.GpxFile.Tracks[0].Name = value;

                    this.HasChanged = true;

                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(this.DisplayName));
                }
            }
        }

        public GpxTrackState State
        {
            get => this.GpxFile.Tracks[this.TrackIndex].Extensions?.TryGetSingleExtension<TrackExtension>()?.State ?? GpxTrackState.Unknown;
            set
            {
                if(this.State != value)
                {
                    // Ensure objects are created
                    var gpxTrack = this.GpxFile.Tracks[this.TrackIndex];
                    if (gpxTrack.Extensions == null)
                    {
                        gpxTrack.Extensions = new FirLib.Formats.Gpx.GpxExtensions();
                    }
                    var trackExtension = gpxTrack.Extensions.GetOrCreateExtension<TrackExtension>();

                    // Apply new status value
                    trackExtension.State = value;

                    // Mark change
                    this.HasChanged = true;

                    // Update route display
                    this.UpdateTrackColor();

                    // Notify changes to UI
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(this.DisplayName));
                }
            }
        }

        //[Browsable(false)]
        //public bool IsVisible
        //{
        //    get => _mapViewModel.ContainsGpxFile(this);
        //    set
        //    {
        //        if (value != this.IsVisible)
        //        {
        //            if (value) { _mapViewModel.AddGpxFile(this); }
        //            else { _mapViewModel.RemoveGpxFile(this); }

        //            this.RaisePropertyChanged();
        //        }
        //    }
        //}

        [Browsable(false)]
        public bool HasChanged
        {
            get => _hasChanged;
            private set
            {
                if (_hasChanged != value)
                {
                    _hasChanged = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public GpxTrackViewModel(string filePath, GpxFile gpxFile, int trackIndex, MapViewModel mapViewModel)
        {
            this.TrackIndex = trackIndex;
            this.FilePath = filePath;
            this.GpxFile = gpxFile;
            this.GpxMapLayer = new GpxFileLayer(this.GpxFile);

            this.UpdateTrackColor();

            _mapViewModel = mapViewModel;
        }

        private void UpdateTrackColor()
        {
            switch (this.State)
            {
                case GpxTrackState.Unknown:
                    this.GpxMapLayer.Color = Colors.Gray;
                    break;

                case GpxTrackState.Planned:
                    this.GpxMapLayer.Color = Colors.Yellow;
                    break;

                case GpxTrackState.Succeeded:
                    this.GpxMapLayer.Color = Colors.Green;
                    break;
            }
        }
    }
}
