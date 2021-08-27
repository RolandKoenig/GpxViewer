using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpxViewer.Core.ValueObjects;

namespace GpxViewer.Shell.Utils
{
    internal class RecentlyOpenedInfo
    {
        public string Path { get; set; } = string.Empty;

        public RecentlyOpenedType Type { get; set; }
    }
}
