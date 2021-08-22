using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpxViewer.Core;
using GpxViewer.Core.Patterns;
using GpxViewer.Modules.ElevationProfile.Views;
using Prism.Ioc;
using Prism.Regions;

namespace GpxViewer.Modules.ElevationProfile
{
    public class ElevationProfileModule : GpxViewerModuleBase
    {
        /// <inheritdoc />
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }

        /// <inheritdoc />
        public override void OnInitializedCustom(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(
                GpxViewerConstants.REGION_ELEVATION_PROFILE,
                typeof(ElevationProfileView));
        }
    }
}
