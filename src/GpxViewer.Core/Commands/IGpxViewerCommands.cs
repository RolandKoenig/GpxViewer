using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;

namespace GpxViewer.Core.Commands
{
    public interface IGpxViewerCommands
    {
        CompositeCommand LoadFile { get; }

        CompositeCommand LoadDirectory { get; }

        CompositeCommand SaveAll { get; }

        CompositeCommand CloseAll { get; }
    }
}
