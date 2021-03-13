using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FirLib.Core;
using FirLib.Core.Patterns.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FirLib.Tests.Core.Patterns.Messaging
{
    [TestClass]
    public class MessageSourceTests
    {
        [TestMethod]
        public void MessageSourceWithCustomTarget()
        {
            // Prepare
            var customHandlerCalled = false;
            var messageSource = new FirLibMessageSource<TestMessage>(FirLibConstants.MESSENGER_NAME_GUI);
            messageSource.UnitTesting_ReplaceByCustomMessageTarget(
                msg => customHandlerCalled = true);

            // Execute test
            messageSource.Publish(new TestMessage("Testing argument"));

            // Check results
            Assert.IsTrue(customHandlerCalled);
        }

        [TestMethod]
        public void MessageSourceWithRealTarget()
        {
            // Prepare
            var dummyMessenger = new FirLibMessenger();
            dummyMessenger.ConnectToGlobalMessaging(
                FirLibMessengerThreadingBehavior.Ignore,
                "DummyMessenger",
                null);
            try
            {
                var realHandlerCalled = false;
                dummyMessenger.Subscribe<TestMessage>(msg => realHandlerCalled = true);

                // Execute test
                var messageSource = new FirLibMessageSource<TestMessage>("DummyMessenger");
                messageSource.Publish(new TestMessage("Testing argument"));

                // Check results
                Assert.IsTrue(realHandlerCalled);
            }
            finally
            {
                // Cleanup
                dummyMessenger.DisconnectFromGlobalMessaging();
            }
        }
    }
}
