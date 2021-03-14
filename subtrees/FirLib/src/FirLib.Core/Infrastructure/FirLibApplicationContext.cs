using System;
using System.Collections.Generic;
using System.Text;
using FirLib.Core.Infrastructure.Services;

namespace FirLib.Core.Infrastructure
{
    internal class FirLibApplicationContext
    {
        public List<Action>? StartupActions { get; set; }

        public FirLibServiceContainer Services { get; } = new FirLibServiceContainer();
    }
}
