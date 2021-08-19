using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using FirLib.Core.Patterns.Messaging;
using GpxViewer.Core.Commands;
using GpxViewer.Shell.Interface.Services;

namespace GpxViewer.Shell.Views
{
    internal static class DesignData
    {
        public static MainWindowViewModel MainWindowVM
        {
            get
            {
                return new MainWindowViewModel(
                    new ShellModuleConfiguration(),
                    A.Fake<IGpxViewerCommands>(),
                    A.Fake<IGpxViewerSkinService>());
            }
        }
    }
}
