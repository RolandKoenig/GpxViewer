using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FirLib.Formats.Gpx.Metadata;

namespace FirLib.Formats.Gpx
{
    [XmlType("gpx")]
    public class GpxFile
    {
        private static List<Type>? s_extensionTypes;
        private static List<(string, string)>? s_extensionNamespaces;
        private static XmlSerializer? s_cachedSerializer;

        [XmlAttribute("version")]
        public string Version { get; set; } = string.Empty;

        [XmlAttribute("creator")]
        public string Creator { get; set; } = string.Empty;

        [XmlElement("metadata")]
        public GpxMetadata? Metadata { get; set; }

        [XmlElement("wpt")]
        public List<GpxWaypoint> Waypoints { get; } = new();

        [XmlElement("rte")]
        public List<GpxRoute> Routes { get; } = new();

        [XmlElement("trk")]
        public List<GpxTrack> Tracks { get; } = new();

        [XmlElement("extensions")]
        public GpxExtensions? Extensions { get; set; }

        [XmlNamespaceDeclarations]  
        public XmlSerializerNamespaces? Xmlns { get; set; }

        public GpxTrack CreateAndAddDummyTrack(string name, params GpxWaypoint[] waypoints)
        {
            var gpxTrack = new GpxTrack();
            gpxTrack.Name = name;
            this.Tracks.Add(gpxTrack);

            var gpxTrackSegment = new GpxTrackSegment();
            gpxTrack.Segments.Add(gpxTrackSegment);

            gpxTrackSegment.Points.AddRange(waypoints);

            return gpxTrack;
        }

        public GpxRoute CreateAndAddDummyRoute(string name, params GpxWaypoint[] waypoints)
        {
            var gpxRoute = new GpxRoute();
            gpxRoute.Name = name;
            this.Routes.Add(gpxRoute);

            gpxRoute.RoutePoints.AddRange(waypoints);

            return gpxRoute;
        }

        public void EnsureNamespaceDeclarations()
        {
            if(s_extensionNamespaces != null)
            {
                this.Xmlns ??= new XmlSerializerNamespaces();
                for(var loop=0; loop<s_extensionNamespaces.Count; loop++)
                {
                    var actTuple = s_extensionNamespaces[loop];
                    this.Xmlns.Add(actTuple.Item1, actTuple.Item2);
                }
            }
        }

        public static void RegisterNamespace(string namespacePrefix, string namespaceUri)
        {
            s_extensionNamespaces ??= new List<(string, string)>();
            for(var loop=0; loop<s_extensionNamespaces.Count; loop++)
            {
                var actEntry = s_extensionNamespaces[loop];
                if (actEntry.Item1 == namespacePrefix)
                {
                    if (actEntry.Item2 != namespaceUri)
                    {
                        throw new GpxFileException(
                            $"Namespace with prefix {namespacePrefix} already registered for {actEntry.Item2}!");
                    }
                    return;
                }
            }

            s_extensionNamespaces.Add((namespacePrefix, namespaceUri));
            s_cachedSerializer = null;
        }

        public static void RegisterExtensionType(Type extensionType)
        {
            s_extensionTypes ??= new List<Type>();
            if (s_extensionTypes.Contains(extensionType)) { return; }

            s_extensionTypes.Add(extensionType);
            s_cachedSerializer = null;
        }

        public static XmlSerializer GetSerializer()
        {
            var cachedSerializer = s_cachedSerializer;
            if (cachedSerializer != null) { return cachedSerializer; }

            XmlSerializer result;
            if (s_extensionTypes != null)
            {
                result = new XmlSerializer(typeof(GpxFile), null, s_extensionTypes.ToArray(), null, "http://www.topografix.com/GPX/1/1");
            }
            else
            {
                result = new XmlSerializer(typeof(GpxFile), "http://www.topografix.com/GPX/1/1");
            }

            s_cachedSerializer = result;
            return result;
        }

        public static void Serialize(GpxFile gpxFile, TextWriter textWriter)
        {
            gpxFile.EnsureNamespaceDeclarations();

            GetSerializer().Serialize(textWriter, gpxFile);
        }

        public static void Serialize(GpxFile gpxFile, Stream stream)
        {
            using(var streamWriter = new StreamWriter(stream))
            {
                Serialize(gpxFile, streamWriter);
            }
        }

        public static void Serialize(GpxFile gpxFile, string targetFile)
        {
            using(var streamWriter = new StreamWriter(File.Create(targetFile)))
            {
                Serialize(gpxFile, streamWriter);
            }
        }

        public static GpxFile Deserialize(TextReader textReader)
        {
            if (!(GetSerializer().Deserialize(textReader) is GpxFile result))
            {
                throw new GpxFileException($"Unable to deserialize {nameof(GpxFile)}: Unknown error");
            }
            return result;
        }

        public static GpxFile Deserialize(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                return Deserialize(streamReader);
            }
        }

        public static GpxFile Deserialize(string sourceFile)
        {
            using (var streamReader = new StreamReader(File.OpenRead(sourceFile)))
            {
                return Deserialize(streamReader);
            }
        }
    }
}
