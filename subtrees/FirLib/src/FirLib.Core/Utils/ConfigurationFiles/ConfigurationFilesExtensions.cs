using System;
using System.Collections.Generic;
using System.Text;
using FirLib.Core.Infrastructure;

namespace FirLib.Core.Utils.ConfigurationFiles
{
    public static class ConfigurationFilesExtensions
    {
        public static FirLibApplicationLoader AddConfigurationFileService(
            this FirLibApplicationLoader loader, 
            string appName)
        {
            loader.Services.Register(
                typeof(IConfigurationFileAccessors),
                new ConfigurationFileAccessors(appName));

            return loader;
        }
    }
}
