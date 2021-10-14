using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpxViewer.Core.ValueObjects
{
    /// <summary>
    /// This ValueType ensures that more paths to the same file can not differ from each other.
    /// </summary>
    public readonly struct FileOrDirectoryPath
    {
        private readonly string? _path;

        public string Path => _path ?? string.Empty;

        public FileOrDirectoryPath(string path)
        {
            // Bring given path to a standard, absolute format
            _path = System.IO.Path.GetFullPath(path);
        }

        public bool Equals(FileOrDirectoryPath other)
        {
            return _path == other._path;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is FileOrDirectoryPath other && this.Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (_path != null ? _path.GetHashCode() : 0);
        }

        public static bool operator ==(FileOrDirectoryPath left, FileOrDirectoryPath right)
        {
            return left._path == right._path;
        }

        public static bool operator !=(FileOrDirectoryPath left, FileOrDirectoryPath right)
        {
            return left._path != right._path;
        }
    }
}
