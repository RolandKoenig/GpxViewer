using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core;
using FirLib.Core.Patterns;
using FirLib.Core.Utils.Mathematics;

namespace GpxViewer.Shell
{
    internal class ShellModuleConfiguration : PropertyChangedBase
    {
        private double _leftBlockWidth = 150.0;
        private double _bottomBlockHeight = 125.0;

        public string Skin
        {
            get;
            set;
        } = string.Empty;

        public double LeftBlockWidth
        {
            get => _leftBlockWidth;
            set
            {
                if (!_leftBlockWidth.Equals7DigitPrecision(value))
                {
                    _leftBlockWidth = value;
                    this.RaisePropertyChanged(nameof(this.LeftBlockWidth));
                }
            }
        }

        public double BottomBlockHeight
        {
            get => _bottomBlockHeight;
            set
            {
                if (!_bottomBlockHeight.Equals7DigitPrecision(value))
                {
                    _bottomBlockHeight = value;
                    this.RaisePropertyChanged(nameof(this.BottomBlockHeight));
                }
            }
        }
    }
}
