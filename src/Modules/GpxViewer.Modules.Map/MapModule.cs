using System;
using FirLib.Core.Services.ConfigurationFiles;
using GpxViewer.Core;
using GpxViewer.Core.Messages;
using GpxViewer.Core.Patterns;
using GpxViewer.Core.Utils;
using GpxViewer.Modules.Map.Views;
using Prism.Ioc;
using Prism.Regions;

namespace GpxViewer.Modules.Map
{
    public class MapModule : GpxViewerModuleBase
    {
        private IConfigurationFileAccessors _configAccessors;

        private MapModuleConfiguration? _config;

        public MapModule(IConfigurationFileAccessors configAccessors)
        {
            _configAccessors = configAccessors;
        }

        public override void OnInitializedCustom(IContainerProvider containerProvider)
        {
            _config = _configAccessors.Application.TryReadFile("MapModule", "json")
                .ReadJsonAndClose<MapModuleConfiguration>(true);

            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(
                GpxViewerConstants.REGION_MAP,
                typeof(MapView));
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton(
                typeof(MapModuleConfiguration),
                () => _config);
        }

        private void OnMessageReceived(MessageGpxViewerOnExit message)
        {
            if (_config == null) { return; }

            try
            {
                _configAccessors.Application.WriteFile("MapModule", "json")
                    .WriteJsonAndClose(_config);
            }
            catch (Exception)
            {
                // Don't break application's exit logic
                // Default or last configuration is loaded on next start
            }
        }
    }
}