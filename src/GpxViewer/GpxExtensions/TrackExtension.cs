using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace GpxViewer.GpxExtensions
{
    [XmlType("TrackExtension", Namespace = "http://gpxviewer.rolandk.net/")]
    public class TrackExtension
    {
        public GpxTrackState State { get; set; } = GpxTrackState.Unknown;
    }
}
