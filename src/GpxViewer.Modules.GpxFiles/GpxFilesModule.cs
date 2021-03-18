using GpxViewer.Core;
using GpxViewer.Core.Model;
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
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IGpxFileRepository, GpxFileRepository>();
        }
    }
}