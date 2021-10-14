using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpxViewer.Modules.GpxFiles.Interface.Model
{
    public interface IGpxFileRepositoryNode
    {
        string NodeText { get; }

        bool HasError { get; }

        Exception? GetErrorDetails();

        ILoadedGpxFile? GetAssociatedGpxFile();

        IEnumerable<ILoadedGpxFile> GetAssociatedGpxFilesDeep();

        IEnumerable<ILoadedGpxFileTourInfo> GetAssociatedToursDeep();
    }
}
