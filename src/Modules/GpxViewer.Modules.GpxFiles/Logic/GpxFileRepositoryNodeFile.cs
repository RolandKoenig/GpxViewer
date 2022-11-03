﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;
using GpxViewer.Core.ValueObjects;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class GpxFileRepositoryNodeFile : GpxFileRepositoryNode
    {
        private LoadedGpxFile? _gpxFile;
        private LoadedGpxFileTourInfo? _tour;
        private Exception? _fileLoadError;

        public FileOrDirectoryPath FilePath { get; }

        /// <inheritdoc />
        public override bool CanSave => true;

        /// <inheritdoc />
        public override bool HasError => _fileLoadError != null;

        public GpxFileRepositoryNodeFile(FileOrDirectoryPath filePath)
        {
            this.FilePath = filePath;

            try
            {
                _gpxFile = new LoadedGpxFile(
                    Path.GetFileName(filePath.Path),
                    GpxFile.Deserialize(
                        filePath.Path, 
                        GpxFileDeserializationMethod.Compatibility));
            }
            catch (Exception e)
            {
                _gpxFile = null;
                _fileLoadError = e;
            }
            
            this.InitializeProperties();
        }

        public GpxFileRepositoryNodeFile(string fileName, GpxFile gpxFile, FileOrDirectoryPath filePath)
        {
            this.FilePath = filePath;
            _gpxFile = new LoadedGpxFile(fileName, gpxFile);

            this.InitializeProperties();
        }

        private void InitializeProperties()
        {
            if (_gpxFile == null)
            {
                _tour = null;
                return;
            }

            switch (_gpxFile.Tours.Count)
            {
                case 1:
                    _tour = _gpxFile.Tours[0];
                    break;

                case > 1:
                {
                    foreach (var actTour in _gpxFile.Tours)
                    {
                        var newChildNode = new GpxFileRepositoryNodeTour(_gpxFile, actTour);
                        newChildNode.Parent = this;
                        this.ChildNodes.Add(newChildNode);
                    }
                    break;
                }
            }
        }

        /// <inheritdoc />
        protected override async ValueTask SaveThisNodesContentsAsync()
        {
            if (_gpxFile == null) { return; }

            await Task.Factory.StartNew(
                () => GpxFile.Serialize(_gpxFile.RawGpxFile, this.FilePath.Path));
            _gpxFile.ContentsChanged = false;
        }

        /// <inheritdoc />
        protected override bool HasThisNodesContentsChanged()
        {
            return _gpxFile?.ContentsChanged ?? false;
        }

        /// <inheritdoc />
        protected override string GetNodeText()
        {
            return Path.GetFileName(this.FilePath.Path);
        }

        /// <inheritdoc />
        public override Exception? GetErrorDetails()
        {
            return _fileLoadError;
        }

        /// <inheritdoc />
        public override ILoadedGpxFile? GetAssociatedGpxFile()
        {
            return _gpxFile;
        }

        /// <inheritdoc />
        public override ILoadedGpxFileTourInfo? GetAssociatedTour()
        {
            return _tour;
        }
    }
}
