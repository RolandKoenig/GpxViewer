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
using FirLib.Core.Patterns.ObjectPooling;

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

        public string BasedOn
        {
            get
            {
                var strBuilder = PooledStringBuilders.Current.TakeStringBuilder(2048);
                try
                {
                    strBuilder.AppendLine(
                        "Prism: Prism is a framework for building loosely coupled, maintainable, and testable XAML applications in WPF, Xamarin Forms, and Uno / Win UI Applications");
                    strBuilder.AppendLine("https://github.com/PrismLibrary/Prism");
                    strBuilder.AppendLine();

                    strBuilder.AppendLine(
                        "Mapsui: Mapsui is a .NET Map component for WPF, Xamarin.Forms, Xamarin.Android, Xamarin.iOS and UWP");
                    strBuilder.AppendLine("https://github.com/Mapsui/Mapsui");
                    strBuilder.AppendLine();

                    strBuilder.AppendLine(
                        "Live-Charts: Simple, flexible, interactive & powerful charts, maps and gauges for .Net");
                    strBuilder.AppendLine("https://github.com/Live-Charts/Live-Charts");
                    strBuilder.AppendLine();

                    strBuilder.AppendLine("MaterialDesignInXaml: Google's Material Design in XAML & WPF, for C# & VB.Net");
                    strBuilder.AppendLine("https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit");
                    strBuilder.AppendLine();

                    strBuilder.AppendLine("Json.NET: Popular high-performance JSON framework for .NET");
                    strBuilder.AppendLine("https://www.newtonsoft.com/json");
                    strBuilder.AppendLine();

                    strBuilder.AppendLine("FirLib: Utility library for most of my own open source applications");
                    strBuilder.AppendLine("https://github.com/RolandKoenig/FirLib");

                    return strBuilder.ToString();
                }
                finally
                {
                    PooledStringBuilders.Current.ReRegisterStringBuilder(strBuilder);
                }
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
