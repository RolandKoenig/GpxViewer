using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class GpxFileRepositoryNodeTour : GpxFileRepositoryNode
    {
        private LoadedGpxFile _parentFile;
        private LoadedGpxFileTourInfo _tour;

        /// <inheritdoc />
        public override bool CanSave => false;

        public GpxFileRepositoryNodeTour(LoadedGpxFile parentFile, LoadedGpxFileTourInfo tour)
        {
            _parentFile = parentFile;
            _tour = tour;
        }

        /// <inheritdoc />
        protected override bool HasThisNodesContentsChanged()
        {
            return _parentFile.ContentsChanged;
        }

        /// <inheritdoc />
        protected override string GetNodeText()
        {
            return _tour.RawTrackOrRoute.Name ?? "-";
        }

        /// <inheritdoc />
        public override ILoadedGpxFile? GetAssociatedGpxFile()
        {
            return _parentFile;
        }

        /// <inheritdoc />
        public override ILoadedGpxFileTourInfo? GetAssociatedTour()
        {
            return _tour;
        }
    }
}
