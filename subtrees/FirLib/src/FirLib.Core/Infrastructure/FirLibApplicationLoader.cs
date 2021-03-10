using System;
using System.Collections.Generic;
using System.Text;

namespace FirLib.Core.Infrastructure
{
    public class FirLibApplicationLoader
    {
        private FirLibApplicationContext _context;

        internal FirLibApplicationLoader()
        {
            _context = new FirLibApplicationContext();
        }

        internal FirLibApplicationContext GetContext() => _context;

        public void AddStartupAction(Action action)
        {
            _context.StartupActions ??= new List<Action>();
            _context.StartupActions.Add(action);
        }

        public void Load()
        {
            FirLibApplication.Load(this);
        }
    }
}
