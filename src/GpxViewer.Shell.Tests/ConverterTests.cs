using System;
using GpxViewer.Core;
using GpxViewer.Core.Controls;
using GpxViewer.Shell.Converters;
using MaterialDesignThemes.Wpf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GpxViewer.Shell.Tests
{
    [TestClass]
    public class ConverterTests
    {
        [TestMethod]
        public void Check_GpxViewerIconToMaterialDesignIconConverter_FullIconRange()
        {
            var converter = new GpxViewerIconToMaterialDesignIconConverter();
            foreach (var actGpxViewerIcon in Enum.GetValues<GpxViewerIconKind>())
            {
                object? actConvResult = null;

                try
                {
                    actConvResult = converter.Convert(actGpxViewerIcon, typeof(PackIconKind), null, null);
                }
                catch (Exception e)
                {
                    Assert.Fail($"Icon {actGpxViewerIcon} not supported! Converter threw exception of type {e.GetType().FullName}");
                }

                if (actConvResult == null)
                {
                    Assert.Fail($"Icon {actGpxViewerIcon} not supported! Converter returned null");
                }
                else if (actConvResult is not PackIconKind)
                {
                    Assert.Fail($"Icon {actGpxViewerIcon} not supported! Converter returned unexpected type {actConvResult.GetType().FullName}");
                }
            }
        }
    }
}
