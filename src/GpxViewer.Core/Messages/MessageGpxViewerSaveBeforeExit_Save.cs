using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core;
using FirLib.Core.Patterns.Messaging;

namespace GpxViewer.Core.Messages
{
    [FirLibMessage]
    [MessagePossibleSource(FirLibConstants.MESSENGER_NAME_GUI)]
    public class MessageGpxViewerSaveBeforeExit_Save
    {
        public List<Task> SaveTasks { get; }

        public MessageGpxViewerSaveBeforeExit_Save()
        {
            this.SaveTasks = new List<Task>();
        }
    }
}
