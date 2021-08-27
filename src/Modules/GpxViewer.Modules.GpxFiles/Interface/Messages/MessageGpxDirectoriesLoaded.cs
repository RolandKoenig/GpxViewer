using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core;
using FirLib.Core.Patterns.Messaging;
using GpxViewer.Core.ValueObjects;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Interface.Messages
{
    [FirLibMessage]
    [MessagePossibleSource(FirLibConstants.MESSENGER_NAME_GUI)]
    public record MessageGpxDirectoriesLoaded(
        IEnumerable<FileOrDirectoryPath> DirectoryPaths,
        IEnumerable<IGpxFileRepositoryNode> Nodes);
}
