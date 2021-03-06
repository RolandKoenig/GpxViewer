using System;
using System.Collections.Generic;
using System.Text;

namespace FirLib.Core.Infrastructure
{
    public class FirLibApplication
    {
        public static FirLibApplication? Current { get; private set; }

        public static bool IsInitialized => Current != null;

        public static void Initialize()
        {
            if (Current != null)
            {
                throw new FirLibException($"{nameof(FirLibApplication)} is already initialized!");
            }

            Current = new FirLibApplication();
        }
    }
}
