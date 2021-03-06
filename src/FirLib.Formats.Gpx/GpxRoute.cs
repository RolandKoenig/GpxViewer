using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FirLib.Formats.Gpx
{
    public class GpxRoute
    {
        [XmlElement("name")]
        public string? Name { get; set; }

        [XmlElement("cmt")]
        public string? Comment { get; set; }

        [XmlElement("desc")]
        public string? Description { get; set; }

        [XmlElement("src")]
        public string? Source { get; set; }

        [XmlElement("link")]
        public List<string> Links { get; } = new();

        [XmlElement("number")]
        public int? GpsRouteNumber { get; set; }

        [XmlIgnore]
        public bool GpsRouteNumberSpecified => this.GpsRouteNumber.HasValue;

        [XmlElement("type")]
        public string? Type { get; set; }

        [XmlElement("extensions")]
        public GpxExtensions? Extensions { get; set; }

        [XmlElement("rtept")]
        public List<GpxWaypoint> RoutePoints { get; } = new();

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Route: Name={this.Name}, PointCount={this.RoutePoints.Count}";
        }
    }
}
