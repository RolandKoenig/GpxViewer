using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core;
using FirLib.Core.Infrastructure;
using FirLib.Core.Patterns.Messaging;
using Prism.Ioc;
using Prism.Modularity;

namespace GpxViewer.Core.Patterns
{
    public abstract class GpxViewerModuleBase : IModule
    {
        private IEnumerable<MessageSubscription>? _messageSubscriptions;

        [Browsable(false)]
        public FirLibMessenger Messenger => FirLibMessenger.GetByName(FirLibConstants.MESSENGER_NAME_GUI);

        /// <inheritdoc />
        public abstract void RegisterTypes(IContainerRegistry containerRegistry);

        /// <inheritdoc />
        public void OnInitialized(IContainerProvider containerProvider)
        {
            if (FirLibApplication.IsLoaded)
            {
                _messageSubscriptions = this.Messenger.SubscribeAll(this);
            }

            this.OnInitializedCustom(containerProvider);
        }

        public abstract void OnInitializedCustom(IContainerProvider containerProvider);
    }
}
