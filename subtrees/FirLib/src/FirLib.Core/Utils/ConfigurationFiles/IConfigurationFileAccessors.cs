using System;
using System.Collections.Generic;
using System.Text;

namespace FirLib.Core.Utils.ConfigurationFiles
{
    public interface IConfigurationFileAccessors
    {
        IConfigurationFileAccessor Application { get; }
    }
}
