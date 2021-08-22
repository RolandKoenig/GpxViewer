using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing.MSTest;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GpxViewer.Analyzers.Test.Verifiers;

namespace GpxViewer.Analyzers.Test
{
    [TestClass]
    public class ModuleTypeAccessAnalyzerTests
    {
        [TestMethod]
        public async Task MainModuleType_BadCase()
        {
            var test = @"
namespace GpxViewer.Modules.Testing
{
    internal class {|#0:TestingModule|}
    {
        //...
    }
}";
            var expected = CSharpAnalyzerVerifier<ModuleTypeAccessAnalyzer>
                .Diagnostic(ModuleTypeAccessAnalyzer.DIAGNOSTIC_ID)
                .WithLocation(0)
                .WithArguments("TestingModule", "Internal", "Public");

            await CSharpAnalyzerVerifier<ModuleTypeAccessAnalyzer>.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task MainModuleType_GoodCase()
        {
            var test = @"
namespace GpxViewer.Modules.Testing
{
    public class {|#0:TestingModule|}
    {
        //...
    }
}";

            await CSharpAnalyzerVerifier<ModuleTypeAccessAnalyzer>.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task MainModuleType_InterfaceType_BadCase()
        {
            var test = @"
namespace GpxViewer.Modules.Testing.Interface.Messages
{
    internal class {|#0:TestMessage|}
    {
        //...
    }
}";
            var expected = CSharpAnalyzerVerifier<ModuleTypeAccessAnalyzer>
                .Diagnostic(ModuleTypeAccessAnalyzer.DIAGNOSTIC_ID)
                .WithLocation(0)
                .WithArguments("TestMessage", "Internal", "Public");

            await CSharpAnalyzerVerifier<ModuleTypeAccessAnalyzer>.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task MainModuleType_InterfaceType_GoodCase()
        {
            var test = @"
namespace GpxViewer.Modules.Testing.Interface.Messages
{
    public class {|#0:TestMessage|}
    {
        //...
    }
}";

            await CSharpAnalyzerVerifier<ModuleTypeAccessAnalyzer>.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task MainModuleType_LogicType_BadCase()
        {
            var test = @"
namespace GpxViewer.Modules.Testing.Logic
{
    public class {|#0:TestLogicClass|}
    {
        //...
    }
}";
            var expected = CSharpAnalyzerVerifier<ModuleTypeAccessAnalyzer>
                .Diagnostic(ModuleTypeAccessAnalyzer.DIAGNOSTIC_ID)
                .WithLocation(0)
                .WithArguments("TestLogicClass", "Public", "Internal");

            await CSharpAnalyzerVerifier<ModuleTypeAccessAnalyzer>.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task MainModuleType_LogicType_GoodCase()
        {
            var test = @"
namespace GpxViewer.Modules.Testing.Logic
{
    internal class {|#0:TestLogicClass|}
    {
        //...
    }
}";

            await CSharpAnalyzerVerifier<ModuleTypeAccessAnalyzer>.VerifyAnalyzerAsync(test);
        }
    }
}
