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
        private GpxFile? _fileContents;
        private Exception? _fileLoadError;

        public override string NodeText => Path.GetFileName(_filePath);

        public GpxFileRepositoryNodeFile(string filePath)
        {
            _filePath = filePath;

            try
            {
                _fileContents = GpxFile.Deserialize(filePath);
            }
            catch (Exception e)
            {
                _fileContents = null;
                _fileLoadError = e;
            }
            
        }


    }
}
