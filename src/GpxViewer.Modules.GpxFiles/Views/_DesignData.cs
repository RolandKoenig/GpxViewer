using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using FirLib.Core.Patterns.Messaging;
using FirLib.Formats.Gpx;
using FirLib.Formats.Gpx.Metadata;
using GpxViewer.Core.Commands;
using GpxViewer.Core.ValueObjects;
using GpxViewer.Modules.GpxFiles.Logic;

namespace GpxViewer.Modules.GpxFiles.Views
{
    internal static class DesignData
    {
        public static FileTreeViewModel FileTreeVM
        {
            get
            {
                var gpxFile = new GpxFile();
                gpxFile.Metadata = new GpxMetadata() { Name = "DummyFile" };
                var gpxTrack = gpxFile.CreateAndAddDummyTrack("DummyTrack", new GpxWaypoint[]
                {
                    new (0.0, 0.0, 100.0),
                    new (0.0, 0.0, 200.0),
                    new (0.0, 0.0, 300.0),
                    new (0.0, 0.0, 250.0),
                    new (0.0, 0.0, 260.0),
                    new (0.0, 0.0, 210.0)
                });

                var gpxFileRepo = new GpxFileRepository(A.Fake<IFirLibMessagePublisher>());
                gpxFileRepo.AddTopLevelNode(new GpxFileRepositoryNodeFile(gpxFile, new FileOrDirectoryPath("DummyPath")));
                gpxFileRepo.AddTopLevelNode(new GpxFileRepositoryNodeFile(gpxFile, new FileOrDirectoryPath("DummyPath")));
                gpxFileRepo.AddTopLevelNode(new GpxFileRepositoryNodeFile(gpxFile, new FileOrDirectoryPath("DummyPath")));

                return new FileTreeViewModel(
                    gpxFileRepo,
                    A.Fake<IGpxViewerCommands>());
            }
        }

        public static SelectedToursViewModel SelectedToursVM
        {
            get
            {
                var result = new SelectedToursViewModel();
                return result;
            }
        }
    }
}
