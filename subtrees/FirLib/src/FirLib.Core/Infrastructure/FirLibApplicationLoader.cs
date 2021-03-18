using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FirLib.Core.Infrastructure.Services;

namespace FirLib.Core.Infrastructure
{
    public class FirLibApplicationLoader
    {
        private FirLibApplicationContext _context;

        public FirLibServiceContainer Services => _context.Services;

        internal FirLibApplicationLoader()
        {
            _context = new FirLibApplicationContext();
        }

        internal FirLibApplicationContext GetContext() => _context;

        public FirLibApplicationLoader AddStartupAction(Action action)
        {
            _context.StartupActions ??= new List<Action>();
            _context.StartupActions.Add(action);

            return this;
        }

        public FirLibApplicationLoader ConfigureCurrentThreadAsMainGuiThread()
        {
            Thread.CurrentThread.Name = FirLibConstants.MESSENGER_NAME_GUI;

            return this;
        }

        public void Load()
        {
            FirLibApplication.Load(this);
        }
    }
}
