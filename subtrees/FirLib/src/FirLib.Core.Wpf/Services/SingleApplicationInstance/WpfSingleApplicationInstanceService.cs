using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Windows.Interop;

namespace FirLib.Core.Services.SingleApplicationInstance
{
    public class WpfSingleApplicationInstanceService : ISingleApplicationInstanceService, IDisposable
    {
        /// <inheritdoc />
        public bool IsMainInstance { get; } = true;

        /// <inheritdoc />
        public bool CanSendReceiveMessages { get; } = false;

        /// <inheritdoc />
        public event EventHandler<ISingleApplicationInstanceService>? MessageReceived;

        public WpfSingleApplicationInstanceService()
        {
            
        }

        /// <inheritdoc />
        public bool TrySendMessageToMainInstance(string message)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }
    }
}
