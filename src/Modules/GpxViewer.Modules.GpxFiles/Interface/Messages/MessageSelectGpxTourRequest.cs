using FirLib.Core;
using FirLib.Core.Patterns.Messaging;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Interface.Messages
{
    [FirLibMessage]
    [MessagePossibleSource(FirLibConstants.MESSENGER_NAME_GUI)]
    public record MessageSelectGpxTourRequest(ILoadedGpxFileTourInfo? TourToSelect);
}
