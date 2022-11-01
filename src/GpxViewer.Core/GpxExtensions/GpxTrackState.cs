using System.ComponentModel;
using GpxViewer.Core.Utils;

namespace GpxViewer.Core.GpxExtensions
{
    public enum GpxTrackState
    {
        [LocalizableDescription(
            typeof(GpxTrackStateResources),
            nameof(GpxTrackStateResources.State_Unknown))]
        Unknown,

        [LocalizableDescription(
            typeof(GpxTrackStateResources),
            nameof(GpxTrackStateResources.State_Planned))]
        Planned,

        [LocalizableDescription(
            typeof(GpxTrackStateResources),
            nameof(GpxTrackStateResources.State_Succeeded))]
        Succeeded
    }
}
