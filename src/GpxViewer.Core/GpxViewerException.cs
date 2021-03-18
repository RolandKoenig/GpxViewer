using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpxViewer.Core
{
    public class GpxViewerException : ApplicationException
    {
        public GpxViewerException(string message)
            : base(message)
        {
        }

        public GpxViewerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
