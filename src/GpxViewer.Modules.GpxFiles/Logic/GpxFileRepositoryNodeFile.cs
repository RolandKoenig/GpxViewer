using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class GpxFileRepositoryNodeFile : GpxFileRepositoryNode
    {
        private string _filePath;
        private Exception? _fileLoadError;

        public override string NodeText => Path.GetFileName(_filePath);

        public override LoadedGpxFile? AssociatedGpxFile { get; }

        public GpxFileRepositoryNodeFile(string filePath)
        {
            _filePath = filePath;

            try
            {
                this.AssociatedGpxFile = new LoadedGpxFile(GpxFile.Deserialize(filePath));
            }
            catch (Exception e)
            {
                this.AssociatedGpxFile = null;
                _fileLoadError = e;
            }
        }
    }
}
