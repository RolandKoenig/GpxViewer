﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FirLib.Formats.Gpx
{
    public class GpxTrack
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
        public int? GpsTrackNumber { get; set; }

        [XmlIgnore]
        public bool GpsTrackNumberSpecified => this.GpsTrackNumber.HasValue;

        [XmlElement("type")]
        public string? Type { get; set; }

        [XmlElement("extensions")]
        public GpxExtensions? Extensions { get; set; }

        [XmlElement("trkseg")]
        public List<GpxTrackSegment> Segments { get; } = new();

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Track: Name={this.Name}, SegmentCount={this.Segments.Count}";
        }
    }
}
