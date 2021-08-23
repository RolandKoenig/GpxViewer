using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Patterns.ObjectPooling;
using FirLib.Formats.Gpx;
using GpxViewer.Core.ValueObjects;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class GpxFileRepositoryNodeFile : GpxFileRepositoryNode
    {
        private Exception? _fileLoadError;

        public override LoadedGpxFile? AssociatedGpxFile { get; }

        public FileOrDirectoryPath FilePath { get; }

        public GpxFileRepositoryNodeFile(FileOrDirectoryPath filePath)
        {
            this.FilePath = filePath;

            try
            {
                this.AssociatedGpxFile = new LoadedGpxFile(GpxFile.Deserialize(filePath.Path));
            }
            catch (Exception e)
            {
                this.AssociatedGpxFile = null;
                _fileLoadError = e;
            }
        }

        public GpxFileRepositoryNodeFile(GpxFile gpxFile, FileOrDirectoryPath filePath)
        {
            this.FilePath = filePath;
            this.AssociatedGpxFile = new LoadedGpxFile(gpxFile);
        }

        /// <inheritdoc />
        protected override bool AreThisNodesContentsChanged()
        {
            return this.AssociatedGpxFile?.ContentsChanged ?? false;
        }

        /// <inheritdoc />
        protected override string GetNodeText()
        {
            if (this.AssociatedGpxFile != null)
            {
                return Path.GetFileName(this.FilePath.Path);
            }
            else
            {
                return $"{Path.GetFileName(this.FilePath.Path)} ** Loading Error **";
            }
        }
    }
}
