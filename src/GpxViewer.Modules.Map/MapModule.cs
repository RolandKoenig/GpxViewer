using GpxViewer.Core;
using GpxViewer.Modules.Map.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GpxViewer.Modules.Map
{
    public class MapModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(
                GpxViewerConstants.REGION_MAP,
                typeof(MapView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}