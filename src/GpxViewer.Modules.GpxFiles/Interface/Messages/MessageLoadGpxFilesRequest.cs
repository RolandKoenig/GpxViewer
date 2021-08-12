using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core;
using FirLib.Core.Patterns.Messaging;

namespace GpxViewer.Modules.GpxFiles.Interface.Messages
{
    [FirLibMessage]
    [MessagePossibleSource(FirLibConstants.MESSENGER_NAME_GUI)]
    public record MessageLoadGpxFilesRequest(
        IEnumerable<string>? Files, 
        IEnumerable<string>? Directories);
}
