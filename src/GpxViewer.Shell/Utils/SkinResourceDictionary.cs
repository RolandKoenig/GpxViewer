using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GpxViewer.Shell.Utils
{
    // A resource dictionary which can choose between skins
    // see https://michaelscodingspot.com/wpf-complete-guide-themes-skins/

    public class SkinResourceDictionary : ResourceDictionary
    {
        private Uri? _darkSource;
        private Uri? _lightSource;
        
        public Uri? DarkSource
        {
            get => _darkSource;
            set 
            {
                _darkSource = value;
                UpdateSource();
            }
        }
 
        public Uri? LightSource
        {
            get => _lightSource;
            set 
            {
                _lightSource = value;
                UpdateSource();
            }
        }

        public void UpdateSource()
        {
            Uri? chosenUri = null;
            switch (App.CurrentApp.Skin)
            {
                case AppSkin.Dark:
                    chosenUri = _darkSource;
                    break;

                case AppSkin.Light:
                    chosenUri = _lightSource;
                    break;
            }

            if (chosenUri != null) { this.Source = chosenUri; }
        }
    }
}
