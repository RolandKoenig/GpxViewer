using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FirLib.Core.Patterns.Mvvm;

namespace FirLib.Core.ViewServices
{
    public class WpfMessageBoxService : ViewServiceBase, IMessageBoxService
    {
        private Window _owner;

        public WpfMessageBoxService(Window owner)
        {
            _owner = owner;
        }

        /// <inheritdoc />
        public Task<MessageBoxResult> ShowAsync(string title, string message, MessageBoxButtons buttons)
        {
            //MessageBoxButton b;
            //switch (buttons)
            //{
            //    case MessageBoxButtons.Ok:
            //        break;

            //    case MessageBoxButtons.OkCancel:
            //        break;
                
            //    case MessageBoxButtons.YesNo:
            //        break;

            //    case MessageBoxButtons.YesNoCancel:
            //        break;
            //}

            //MessageBox.Show(message, title, )

            throw new NotImplementedException();
        }
    }
}
