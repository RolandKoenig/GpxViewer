using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FirLib.Core;
using FirLib.Core.Infrastructure;
using GpxViewer.GpxExtensions;
using FirLib.Formats.Gpx;

namespace GpxViewer
{
    public partial class App : Application
    {
        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize base application logic
            FirLibApplication.Loader
                .AttachToWpfEnvironment()
                .Load();

            // Register GpxFile extensions
            GpxFile.RegisterExtensionType(typeof(TrackExtension));
            GpxFile.RegisterNamespace("rkgpxv", "http://gpxviewer.rolandk.net/");
        }
    }
}
