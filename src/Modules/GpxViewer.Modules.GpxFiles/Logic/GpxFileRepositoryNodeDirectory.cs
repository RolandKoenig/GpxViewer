using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpxViewer.Core.ValueObjects;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class GpxFileRepositoryNodeDirectory : GpxFileRepositoryNode
    {
        public FileOrDirectoryPath DirectoryPath { get; }

        /// <inheritdoc />
        public override bool CanSave
        {
            get
            {
                return this.ChildNodes.Any(actChild => actChild.CanSave);
            }
        }

        public GpxFileRepositoryNodeDirectory(FileOrDirectoryPath directory)
        {
            this.DirectoryPath = directory;

            foreach (var actDirectory in Directory.GetDirectories(this.DirectoryPath.Path))
            {
                var childDirectory = new GpxFileRepositoryNodeDirectory(new FileOrDirectoryPath(actDirectory));
                childDirectory.Parent = this;
                this.ChildNodes.Add(childDirectory);
            }

            foreach(var actFilePath in Directory.GetFiles(this.DirectoryPath.Path))
            {
                var actFileExtension = Path.GetExtension(actFilePath);
                if (!actFileExtension.Equals(".gpx", StringComparison.OrdinalIgnoreCase)){ continue; }

                var childFile = new GpxFileRepositoryNodeFile(new FileOrDirectoryPath(actFilePath));
                childFile.Parent = this;
                this.ChildNodes.Add(childFile);
            }
        }

        /// <inheritdoc />
        protected override bool HasThisNodesContentsChanged()
        {
            return false;
        }

        /// <inheritdoc />
        protected override string GetNodeText()
        {
            return Path.GetFileName(this.DirectoryPath.Path);
        }

        /// <inheritdoc />
        public override ILoadedGpxFile? GetAssociatedGpxFile()
        {
            return null;
        }

        /// <inheritdoc />
        public override ILoadedGpxFileTourInfo? GetAssociatedTour()
        {
            return null;
        }
    }
}
