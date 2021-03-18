using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core;
using FirLib.Core.Infrastructure;
using FirLib.Core.Patterns.Messaging;
using FirLib.Core.Patterns.Mvvm;

namespace GpxViewer.Core.Patterns
{
    public class GpxViewerViewModelBase : ViewModelBase
    {
        private IEnumerable<MessageSubscription>? _messageSubscriptions;

        public FirLibMessenger Messenger => FirLibMessenger.GetByName(FirLibConstants.MESSENGER_NAME_GUI);

        /// <inheritdoc />
        protected override void OnMvvmViewAttached()
        {
            base.OnMvvmViewAttached();

            if (FirLibApplication.IsLoaded)
            {
                _messageSubscriptions = this.Messenger.SubscribeAll(this);
            }
        }

        /// <inheritdoc />
        protected override void OnMvvmViewDetaching()
        {
            base.OnMvvmViewDetaching();

            if (_messageSubscriptions != null)
            {
                foreach(var actSubscription in _messageSubscriptions)
                {
                    actSubscription.Unsubscribe();
                }
                _messageSubscriptions = null;
            }
        }
    }
}
