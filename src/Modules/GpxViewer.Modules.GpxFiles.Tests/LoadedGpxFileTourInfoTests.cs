using System;
using FirLib.Formats.Gpx;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GpxViewer.Modules.GpxFiles.Logic;

namespace GpxViewer.Modules.GpxFiles.Tests
{
    [TestClass]
    public class LoadedGpxFileTourInfoTests
    {
        [TestMethod]
        public void Check_TourMetrics_Elevation()
        {
            var gpxFile = new GpxFile();
            var gpxTrack = gpxFile.CreateAndAddDummyTrack("DummyTrack", new GpxWaypoint[]
            {
                new GpxWaypoint(0.0, 0.0, 100.0),
                new GpxWaypoint(0.0, 0.0, 200.0),
                new GpxWaypoint(0.0, 0.0, 300.0),
                new GpxWaypoint(0.0, 0.0, 250.0),
                new GpxWaypoint(0.0, 0.0, 260.0),
                new GpxWaypoint(0.0, 0.0, 210.0)
            });

            var tourInfo = new LoadedGpxFileTourInfo(
                new LoadedGpxFile(gpxFile), 
                gpxTrack);

            Assert.AreEqual(210.0, tourInfo.ElevationUpMeters, nameof(tourInfo.ElevationUpMeters));
            Assert.AreEqual(100.0, tourInfo.ElevationDownMeters, nameof(tourInfo.ElevationDownMeters));
        }
    }
}
