using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Patterns.Messaging;

namespace FirLib.Tests.Core.Patterns.Messaging
{
    public class TestMessage : FirLibMessage
    {
        public string TestArg { get; set; }

        public TestMessage(string testArg)
        {
            this.TestArg = testArg;
        }
    }
}
