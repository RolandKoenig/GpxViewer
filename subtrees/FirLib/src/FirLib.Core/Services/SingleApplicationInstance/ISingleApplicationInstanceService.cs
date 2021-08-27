using System;
using System.Collections.Generic;
using System.Text;

namespace FirLib.Core.Services.SingleApplicationInstance
{
    public interface ISingleApplicationInstanceService
    {
        bool IsMainInstance { get; }

        bool CanSendReceiveMessages { get; }

        public event EventHandler<ISingleApplicationInstanceService> MessageReceived;

        bool TrySendMessageToMainInstance(string message);
    }
}
