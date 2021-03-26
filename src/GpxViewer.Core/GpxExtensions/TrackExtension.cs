using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace GpxViewer.Core.GpxExtensions
{
    [XmlType("TrackExtension", Namespace = "http://gpxviewer.rolandk.net/")]
    public class TrackExtension : TrackOrRouteExtension
    {
        
    }
}
