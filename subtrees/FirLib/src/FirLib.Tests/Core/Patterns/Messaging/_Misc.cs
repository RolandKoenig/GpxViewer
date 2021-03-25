using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Patterns.Messaging;

namespace FirLib.Tests.Core.Patterns.Messaging
{
    [FirLibMessage]
    public class TestMessageClass
    {
        public string TestArg { get; set; }

        public TestMessageClass(string testArg)
        {
            this.TestArg = testArg;
        }
    }

    [FirLibMessage]
    public struct TestMessageStruct
    {
        public string TestArg { get; set; }

        public TestMessageStruct(string testArg)
        {
            this.TestArg = testArg;
        }
    }

#if NET5_0_OR_GREATER
    [FirLibMessage]
    public record TestMessageRecord(string TestArg);
#endif
}
