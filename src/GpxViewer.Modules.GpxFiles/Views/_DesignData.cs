using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using FirLib.Core.Patterns.Messaging;
using GpxViewer.Core.Commands;
using GpxViewer.Modules.GpxFiles.Logic;

namespace GpxViewer.Modules.GpxFiles.Views
{
    internal static class DesignData
    {
        public static FileTreeViewModel FileTreeVM
        {
            get
            {
                return new FileTreeViewModel(
                    new GpxFileRepository(new FirLibMessenger()),
                    A.Fake<IGpxViewerCommands>());
            }
        }

        public static SelectedTracksAndRoutesViewModel SelectedTracksAndRoutesVM
        {
            get
            {
                var result = new SelectedTracksAndRoutesViewModel();
                return result;
            }
        }
    }
}
