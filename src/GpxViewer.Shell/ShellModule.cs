using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Services.ConfigurationFiles;
using GpxViewer.Core.Messages;
using GpxViewer.Core.Patterns;
using GpxViewer.Core.Utils;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using Prism.Ioc;

namespace GpxViewer.Shell
{
    internal class ShellModule : GpxViewerModuleBase
    {
        private IConfigurationFileAccessors _configAccessors;

        private ShellModuleConfiguration _config;

        public ShellModule(ShellModuleConfiguration config, IConfigurationFileAccessors configAccessors)
        {
            _config = config;
            _configAccessors = configAccessors;
        }

        /// <inheritdoc />
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        /// <inheritdoc />
        public override void OnInitializedCustom(IContainerProvider containerProvider)
        {

        }

        private void OnMessageReceived(MessageGpxViewerOnExit message)
        {
            try
            {
                _configAccessors.Application.WriteFile("ShellModule", "json")
                    .WriteJsonAndClose(_config);
            }
            catch (Exception)
            {
                // Don't break application's exit logic
                // Default or last configuration is loaded on next start
            }
        }
    }
}
