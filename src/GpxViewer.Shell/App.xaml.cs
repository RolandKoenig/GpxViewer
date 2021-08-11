using System;
using System.Reflection;
using GpxViewer.Shell.Views;
using Prism.Ioc;
using System.Windows;
using FirLib.Core;
using FirLib.Core.Infrastructure;
using FirLib.Core.Patterns.Messaging;
using FirLib.Core.Services.ConfigurationFiles;
using FirLib.Formats.Gpx;
using GpxViewer.Core;
using GpxViewer.Core.Commands;
using GpxViewer.Core.GpxExtensions;
using GpxViewer.Core.Messages;
using GpxViewer.Core.Utils;
using GpxViewer.Shell.Utils;
using Prism.DryIoc;
using Prism.Modularity;
using Prism.Mvvm;

namespace GpxViewer.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private AppSkin _skin;

        public AppSkin Skin
        {
            get => _skin;
            set
            {
                if (_skin != value)
                {
                    _skin = value;

                    foreach (var actResourceDict in this.Resources.MergedDictionaries)
                    {
                        if (actResourceDict is SkinResourceDictionary skinDict)
                        {
                            skinDict.UpdateSource();
                        }
                        else if(actResourceDict.Source != null)
                        {
                            actResourceDict.Source = actResourceDict.Source;
                        }
                    }
                }
            }
        }

        public static App CurrentApp => (App)Current;

        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            // Initialize base application logic
            FirLibApplication.Loader
                .ConfigureCurrentThreadAsMainGuiThread()
                .AttachToWpfEnvironment()
                .AddConfigurationFileService("RKGpxViewer")
                .Load();

            // Register GpxFile extensions
            GpxFile.RegisterExtensionType(typeof(TrackExtension));
            GpxFile.RegisterExtensionType(typeof(RouteExtension));
            GpxFile.RegisterNamespace("rkgpxv", "http://gpxviewer.rolandk.net/");

            // Trigger normal startup
            base.OnStartup(e);
        }

        /// <inheritdoc />
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            var messenger = FirLibMessenger.GetByName(FirLibConstants.MESSENGER_NAME_GUI);
            messenger.Publish(new MessageGpxViewerExitPreview());
            messenger.Publish(new MessageGpxViewerExit());
        }

        /// <inheritdoc />
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IGpxViewerCommands, GpxViewerCommands>();

            // Register existing services from FirLibApplication
            IConfigurationFileAccessors? configAccessors = null;
            foreach (var actService in FirLibApplication.Current!.Services.GetAllServices())
            {
                containerRegistry.RegisterSingleton(
                    actService.Item1,
                    () => actService.Item2);

                if (actService.Item2 is IConfigurationFileAccessors castedService)
                {
                    configAccessors = castedService;
                }
            }

            // Ensure that we have a ShellModuleConfiguration when MainWindow is created
            // (ShellModule is loaded to late)
            if (configAccessors != null)
            {
                var shellConfigObject = configAccessors.Application.TryReadFile("ShellModule", "json")
                    .ReadJsonAndClose<ShellModuleConfiguration>(true);
                containerRegistry.RegisterSingleton(
                    typeof(ShellModuleConfiguration),
                    () => shellConfigObject);
            }
            else
            {
                var shellConfigObject = new ShellModuleConfiguration();
                containerRegistry.RegisterSingleton(
                    typeof(ShellModuleConfiguration),
                    () => shellConfigObject);
            }
        }

        protected override Window CreateShell()
        {
            return this.Container.Resolve<MainWindow>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ShellModule>();
            moduleCatalog.AddModule<Modules.GpxFiles.GpxFilesModule>();
            moduleCatalog.AddModule<Modules.Map.MapModule>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            // Assume that ViewModel is located in the same namespace as the view
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewName = viewType.FullName;
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                if ((string.IsNullOrEmpty(viewName)) ||
                    (string.IsNullOrEmpty(viewAssemblyName)))
                {
                    throw new GpxViewerException(
                        $"Unable to resolve ViewModel for type {viewType}: TypeFullName or TypeAssemblyName is empty!");
                }

                string viewModelName;
                if (viewName.EndsWith("View")) { viewModelName = $"{viewName}Model, {viewAssemblyName}"; }
                else { viewModelName = $"{viewName}ViewModel, {viewAssemblyName}"; }
                return Type.GetType(viewModelName);
            });
        }
    }
}
