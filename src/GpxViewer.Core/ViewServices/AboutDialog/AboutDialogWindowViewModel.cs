using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Patterns;
using FirLib.Core.Patterns.Mvvm;

namespace GpxViewer.Core.ViewServices.AboutDialog
{
    internal class AboutDialogWindowViewModel : ViewModelBase
    {
        public string Name
        {
            get
            {
                var asmAttribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyProductAttribute>();
                return asmAttribute?.Product ?? string.Empty;
            }
        }

        public string Version
        {
            get
            {
                var asmAttribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                return asmAttribute?.InformationalVersion ?? string.Empty;
            }
        }

        public string Description
        {
            get
            {
                var asmAttribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyDescriptionAttribute>();
                return asmAttribute?.Description ?? string.Empty;
            }
        }

        public string Homepage
        {
            get => "https://github.com/RolandKoenig/GpxViewer";
        }

        public string Author
        {
            get
            {
                var asmAttribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCompanyAttribute>();
                return asmAttribute?.Company ?? string.Empty;
            }
        }

        public string Copyright
        {
            get
            {
                var asmAttribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCopyrightAttribute>();
                return asmAttribute?.Copyright ?? string.Empty;
            }
        }

        public string TargetFramework
        {
            get
            {
                var asmAttribute = Assembly.GetExecutingAssembly().GetCustomAttribute<TargetFrameworkAttribute>();
                return asmAttribute?.FrameworkName ?? string.Empty;
            }
        }

        public string NetFrameworkVersion
        {
            get
            {
                return Environment.Version.ToString();
            }
        }

        [Browsable(false)]
        public DelegateCommand Command_Close { get; }

        public AboutDialogWindowViewModel()
        {
            this.Command_Close = new DelegateCommand(
                () => this.CloseWindow(null));
        }
    }
}
