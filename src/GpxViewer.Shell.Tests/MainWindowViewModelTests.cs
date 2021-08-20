using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DryIoc;
using FakeItEasy;
using FakeItEasy.Sdk;
using GpxViewer.Shell.Interface.Services;
using GpxViewer.Shell.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GpxViewer.Shell.Tests
{
    [TestClass]
    public class MainWindowViewModelTests
    {
        private Container _container;

        public MainWindowViewModelTests()
        {
            // Prepare DryIoc container
            _container = new Container(Rules.Default
                .WithDynamicRegistrationsAsFallback((serviceType, _) => 
                    new[] 
                    {
                        new DynamicRegistration(new DelegateFactory(_ => Create.Fake(serviceType))) 
                    }));
            _container.Register<MainWindowViewModel>(Reuse.Transient);
        }

        [TestMethod]
        public void Check_SetInitialSkin_Correct()
        {
            // Create fakes
            var blackSkinSet = false;
            var fakedSkinService = A.Fake<IGpxViewerSkinService>();
            A.CallToSet(() => fakedSkinService.Skin).To(AppSkin.Dark)
                .Invokes(_ => blackSkinSet = true);

            // Register fakes on container
            _container.RegisterDelegate(_ => new ShellModuleConfiguration() { Skin = AppSkin.Dark.ToString() });
            _container.RegisterDelegate(_ => fakedSkinService);

            // Execute tests
            var mainWindowVM = _container.Resolve<MainWindowViewModel>();

            // Check result
            Assert.IsNotNull(mainWindowVM);
            Assert.IsTrue(blackSkinSet);
        }

        [TestMethod]
        public void Check_SetInitialSkin_Invalid()
        {
            // Create fakes
            var skinSet = false;
            var fakedSkinService = A.Fake<IGpxViewerSkinService>();
            A.CallToSet(() => fakedSkinService.Skin).To(AppSkin.Dark)
                .Invokes(_ => skinSet = true);

            // Register fakes on container
            _container.RegisterDelegate(_ => new ShellModuleConfiguration() { Skin = "This skin does not exist" });
            _container.RegisterDelegate(_ => fakedSkinService);

            // Execute tests
            var mainWindowVM = _container.Resolve<MainWindowViewModel>();

            // Check result
            Assert.IsNotNull(mainWindowVM);
            Assert.IsFalse(skinSet);
        }

        [TestMethod]
        public void Check_SetSkinByCommand()
        {
            // Create fakes
            var skinSet = false;
            var fakedSkinService = A.Fake<IGpxViewerSkinService>();
            A.CallToSet(() => fakedSkinService.Skin).To(AppSkin.Light)
                .Invokes(_ => skinSet = true);

            var shellConfig = new ShellModuleConfiguration();
            shellConfig.Skin = AppSkin.Dark.ToString();

            // Register fakes on container
            _container.RegisterDelegate(_ => shellConfig);
            _container.RegisterDelegate(_ => fakedSkinService);

            // Execute tests
            var mainWindowVM = _container.Resolve<MainWindowViewModel>();
            mainWindowVM.Command_SetSkin.Execute(AppSkin.Light.ToString());

            // Check result
            Assert.IsTrue(skinSet);
        }
    }
}
