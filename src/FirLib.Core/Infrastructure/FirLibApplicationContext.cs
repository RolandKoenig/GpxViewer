using System;
using System.Collections.Generic;
using System.Text;

namespace FirLib.Core.Infrastructure
{
    internal class FirLibApplicationContext
    {
        public List<Action>? StartupActions { get; set; }
    }
}
