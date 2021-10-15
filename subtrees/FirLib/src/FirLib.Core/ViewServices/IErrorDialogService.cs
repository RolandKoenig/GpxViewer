using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FirLib.Core.ViewServices
{
    public interface IErrorDialogService
    {
        Task ShowAsync(Exception errorDetails);
    }
}
