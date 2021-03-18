using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core;
using FirLib.Core.Patterns.Messaging;
using GpxViewer.Core.Model;

namespace GpxViewer.Core.Messages
{
    [MessagePossibleSource(FirLibConstants.MESSENGER_NAME_GUI)]
    public class MessageGpxFileRepositoryContentsChanged : FirLibMessage
    {
        public IGpxFileRepository GpxFileRepository { get; }

        public MessageGpxFileRepositoryContentsChanged(IGpxFileRepository gpxFileRepo)
        {
            this.GpxFileRepository = gpxFileRepo;
        }
    }
}
