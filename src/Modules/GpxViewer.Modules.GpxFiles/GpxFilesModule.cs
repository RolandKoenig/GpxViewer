﻿using FirLib.Core;
using FirLib.Core.Patterns.Messaging;
using GpxViewer.Core;
using GpxViewer.Core.Patterns;
using GpxViewer.Core.ValueObjects;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using GpxViewer.Modules.GpxFiles.Interface.Model;
using GpxViewer.Modules.GpxFiles.Logic;
using GpxViewer.Modules.GpxFiles.Views;
using GpxViewer.Modules.GpxFiles.Views.TourViews;
using Prism.Ioc;
using Prism.Regions;

namespace GpxViewer.Modules.GpxFiles
{
    public class GpxFilesModule : GpxViewerModuleBase
    {
        private GpxFileRepository? _gpxFileRepo;

        public override void OnInitializedCustom(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(
                GpxViewerConstants.REGION_FILE_TREE,
                typeof(FileTreeView));
            regionManager.RegisterViewWithRegion(
                GpxViewerConstants.REGION_TOUR_INFO,
                typeof(SelectedToursView));
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var uiMessenger = FirLibMessenger.GetByName(FirLibConstants.MESSENGER_NAME_GUI);
            _gpxFileRepo = new GpxFileRepository(uiMessenger);

            containerRegistry.RegisterSingleton<IGpxFileRepository>(
                _ => _gpxFileRepo);
            containerRegistry.RegisterSingleton<GpxFileRepository>(
                _ => _gpxFileRepo);
        }

        private async void OnMessageReceived(MessageLoadGpxFilesRequest message)
        {
            if (_gpxFileRepo == null) { return; }

            GpxFileRepositoryNode? lastLoadedNode = null;
            if (message.Files != null)
            {
                foreach (var actFile in message.Files)
                {
                    lastLoadedNode = await _gpxFileRepo.LoadFile(new FileOrDirectoryPath(actFile));
                }
            }
            if (message.Directories != null)
            {
                foreach (var actDirectory in message.Directories)
                {
                    lastLoadedNode = await _gpxFileRepo.LoadDirectory(new FileOrDirectoryPath(actDirectory));
                }
            }

            if (lastLoadedNode != null) { _gpxFileRepo.SelectedNode = lastLoadedNode; }
        }
    }
}