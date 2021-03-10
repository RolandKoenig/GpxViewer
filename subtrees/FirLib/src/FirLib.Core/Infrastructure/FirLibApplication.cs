using System;
using System.Collections.Generic;
using System.Text;

namespace FirLib.Core.Infrastructure
{
    public class FirLibApplication
    {
        public static FirLibApplication? Current { get; private set; }

        public static FirLibApplicationLoader Loader { get; } = new();

        public static bool IsInitialized => Current != null;

        private FirLibApplicationContext _context;

        internal static void Load(FirLibApplicationLoader loader)
        {
            if (Current != null)
            {
                throw new FirLibException($"{nameof(FirLibApplication)} is already loaded!");
            }

            Current = new FirLibApplication(loader.GetContext());
        }

        internal FirLibApplication(FirLibApplicationContext context)
        {
            _context = context;

            if(_context.StartupActions != null)
            {
                foreach(var actStartupAction in _context.StartupActions)
                {
                    actStartupAction();
                }
            }
        }
    }
}
