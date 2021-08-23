﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class LoadedGpxFile : ILoadedGpxFile
    {
        public GpxFile RawGpxFile { get; }

        public List<LoadedGpxFileTourInfo> Tours { get; }

        IEnumerable<ILoadedGpxFileTourInfo> ILoadedGpxFile.Tours => this.Tours;

        public bool ContentsChanged { get; set; }

        public LoadedGpxFile(GpxFile gpxFile)
        {
            this.RawGpxFile = gpxFile;
            this.Tours = new List<LoadedGpxFileTourInfo>();

            foreach (var actRawRouteInfo in gpxFile.Routes)
            {
                this.Tours.Add(new LoadedGpxFileTourInfo(this, actRawRouteInfo));
            }
            foreach(var actRawTrackData in gpxFile.Tracks)
            {
                this.Tours.Add(new LoadedGpxFileTourInfo(this, actRawTrackData));
            }
        }
    }
}