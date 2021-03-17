using GpxViewer.Shell.Views;
using Prism.Ioc;
using System.Windows;
using FirLib.Core;
using FirLib.Core.Infrastructure;
using FirLib.Formats.Gpx;
using GpxViewer.Core.GpxExtensions;
using Prism.Modularity;

namespace GpxViewer.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            // Initialize base application logic
            FirLibApplication.Loader
                .AttachToWpfEnvironment()
                .Load();

            // Register GpxFile extensions
            GpxFile.RegisterExtensionType(typeof(TrackExtension));
            GpxFile.RegisterNamespace("rkgpxv", "http://gpxviewer.rolandk.net/");

            // Trigger normal startup
            base.OnStartup(e);
        }

        protected override Window CreateShell()
        {
            return this.Container.Resolve<MainWindow>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<GpxViewer.Modules.GpxFiles.GpxFilesModule>();
            moduleCatalog.AddModule<GpxViewer.Modules.Map.MapModule>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
