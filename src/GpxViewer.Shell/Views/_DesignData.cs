using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using GpxViewer.Core.Commands;

namespace GpxViewer.Shell.Views
{
    internal static class DesignData
    {
        public static MainWindowViewModel MainWindowVM
        {
            get
            {
                return new MainWindowViewModel(
                    A.Fake<IGpxViewerCommands>());
            }
        }
    }
}
