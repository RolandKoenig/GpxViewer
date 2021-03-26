using FirLib.Core;
using FirLib.Core.Patterns.Messaging;
using GpxViewer.Core;
using GpxViewer.Modules.GpxFiles.Interface.Model;
using GpxViewer.Modules.GpxFiles.Logic;
using GpxViewer.Modules.GpxFiles.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GpxViewer.Modules.GpxFiles
{
    public class GpxFilesModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(
                GpxViewerConstants.REGION_FILE_TREE,
                typeof(FileTreeView));
            regionManager.RegisterViewWithRegion(
                GpxViewerConstants.REGION_TRACK_OR_ROUTE_INFO,
                typeof(SelectedTracksAndRoutesView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var uiMessenger = FirLibMessenger.GetByName(FirLibConstants.MESSENGER_NAME_GUI);
            var gpxFileRepo = new GpxFileRepository(uiMessenger);

            containerRegistry.RegisterSingleton<IGpxFileRepository>(
                containerProvider => gpxFileRepo);
            containerRegistry.RegisterSingleton<GpxFileRepository>(
                containerProvider => gpxFileRepo);
        }
    }
}