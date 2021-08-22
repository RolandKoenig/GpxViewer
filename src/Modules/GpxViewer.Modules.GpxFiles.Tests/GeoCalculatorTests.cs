using System;
using FirLib.Formats.Gpx;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GpxViewer.Modules.GpxFiles.Logic;

namespace GpxViewer.Modules.GpxFiles.Tests
{
    [TestClass]
    public class GeoCalculatorTests
    {
        [TestMethod]
        public void Check_CalculateDistance()
        {
            var point1 = new GpxWaypoint();
            point1.Latitude = 49.638005001470447;
            point1.Longitude = 12.266169972717762;
            
            var point2 = new GpxWaypoint();
            point2.Latitude = 49.6377810370177;
            point2.Longitude = 12.265615006908774;

            var distance = GeoCalculator.CalculateDistanceMeters(point1, point2);
            Assert.AreEqual(47.0, Math.Round(distance, 0));
        }

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
