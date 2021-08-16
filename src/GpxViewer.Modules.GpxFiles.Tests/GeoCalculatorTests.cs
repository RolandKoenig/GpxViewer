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
    }
}
