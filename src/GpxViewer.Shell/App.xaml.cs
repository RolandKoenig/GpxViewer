using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using GpxViewer.Shell.Views;
using Prism.Ioc;
using System.Windows;
using FirLib.Core;
using FirLib.Core.Infrastructure;
using FirLib.Core.Patterns.Messaging;
using FirLib.Core.Services;
using FirLib.Core.Services.ConfigurationFiles;
using FirLib.Core.Services.SingleApplicationInstance;
using FirLib.Formats.Gpx;
using GpxViewer.Core;
using GpxViewer.Core.Commands;
using GpxViewer.Core.GpxExtensions;
using GpxViewer.Core.Messages;
using GpxViewer.Core.Utils;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using GpxViewer.Shell.Interface.Services;
using GpxViewer.Shell.Utils;
using Prism.DryIoc;
using Prism.Modularity;
using Prism.Mvvm;

namespace GpxViewer.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication, IGpxViewerSkinService
    {
        private string[]? _startupArgs;
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
            _startupArgs = e.Args;

            // Initialize base application logic
            FirLibApplication.Loader
                .ConfigureCurrentThreadAsMainGuiThread()
                .AttachToWpfEnvironment()
                .AddConfigurationFileService("RKGpxViewer")
                .AddSingleApplicationInstanceService_Using_WM_COPYDATA("GpxViewer_SingleInstance")
                .Load();

            // Check for single instance
            var srvSingleInstance = FirLibApplication.Current.Services.GetService<ISingleApplicationInstanceService>();
            if (!srvSingleInstance.IsMainInstance)
            {
                var strBuilder = new StringBuilder();
                foreach (var actArg in e.Args)
                {
                    if (strBuilder.Length > 0) { strBuilder.Append(';'); }
                    if (actArg.Length > 0) { strBuilder.Append(actArg); }
                }
                srvSingleInstance.TrySendMessageToMainInstance(strBuilder.ToString());

                // Cancel startup here
                this.Shutdown();
                return;
            }
            else
            {
                srvSingleInstance.MessageReceived += this.OnSrvSingleInstance_MessageReceived;
            }

            // Register GpxFile extensions
            GpxFile.RegisterExtensionType(typeof(TrackExtension));
            GpxFile.RegisterExtensionType(typeof(RouteExtension));
            GpxFile.RegisterNamespace("rkgpxv", "http://gpxviewer.rolandk.net/");

            // Trigger normal startup
            base.OnStartup(e);
        }

        private void OnSrvSingleInstance_MessageReceived(object? sender, MessageReceivedEventArgs e)
        {
            var messenger = FirLibMessenger.GetByName(FirLibConstants.MESSENGER_NAME_GUI);

            try
            {
                // Process incoming message
                if (!string.IsNullOrEmpty(e.Message))
                {
                    var filesOrDirectories = e.Message.Split(';').Where(actPart => !string.IsNullOrEmpty(actPart));

                    messenger.Publish(new MessageLoadGpxFilesRequest(
                        filesOrDirectories.Where(File.Exists),
                        filesOrDirectories.Where(Directory.Exists)));
                }

                // Bring main window to front
                var mainWindow = this.MainWindow;
                if (mainWindow != null)
                {
                    if (mainWindow.WindowState == WindowState.Minimized)
                    {
                        mainWindow.WindowState = WindowState.Normal;
                    }
                    mainWindow.Activate();
                }
            }
            catch
            {
                // Do nothing if this failed
            }
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

            // Register skin service
            containerRegistry.RegisterSingleton(
                typeof(IGpxViewerSkinService),
                () => this);

            // Register existing services from FirLibApplication
            IConfigurationFileAccessors? configAccessors = null;
            foreach (var actService in FirLibApplication.Current.Services.GetAllServices())
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

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (_startupArgs is { Length: > 0 })
            {
                var messenger = FirLibMessenger.GetByName(FirLibConstants.MESSENGER_NAME_GUI);
                messenger.Publish(
                    new MessageLoadGpxFilesRequest(
                        _startupArgs.Where(File.Exists), 
                        null));
            }
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ShellModule>();
            moduleCatalog.AddModule<Modules.GpxFiles.GpxFilesModule>();
            moduleCatalog.AddModule<Modules.Map.MapModule>();
            moduleCatalog.AddModule<Modules.ElevationProfile.ElevationProfileModule>();
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
