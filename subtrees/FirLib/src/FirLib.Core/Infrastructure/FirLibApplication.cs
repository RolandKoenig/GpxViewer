using System;
using System.Collections.Generic;
using System.Text;
using FirLib.Core.Infrastructure.Services;

namespace FirLib.Core.Infrastructure
{
    public class FirLibApplication
    {
        private static FirLibApplication? s_current;

        public static FirLibApplication Current
        {
            get
            {
                if (s_current == null)
                {
                    throw new InvalidOperationException($"{nameof(FirLibApplication)} is not initialized!");
                }
                return s_current;
            }
        }

        public static FirLibApplicationLoader Loader { get; } = new();

        public static bool IsLoaded => s_current != null;

        private FirLibApplicationContext _context;

        public FirLibServiceContainer Services => _context.Services;

        internal static void Load(FirLibApplicationLoader loader)
        {
            if (s_current != null)
            {
                throw new FirLibException($"{nameof(FirLibApplication)} is already loaded!");
            }

            s_current = new FirLibApplication(loader.GetContext());
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
