using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;

namespace GpxViewer.Core.Commands
{
    public class GpxViewerCommands : IGpxViewerCommands
    {
        /// <inheritdoc />
        public CompositeCommand LoadFile { get; } = new();

        /// <inheritdoc />
        public CompositeCommand LoadDirectory { get; } = new();

        /// <inheritdoc />
        public CompositeCommand SaveAll { get; } = new();

        /// <inheritdoc />
        public CompositeCommand CloseAll { get; } = new();
    }
}
