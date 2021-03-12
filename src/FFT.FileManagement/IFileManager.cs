// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.FileManagement
{
  using System.IO;
  using System.Threading.Tasks;

  /// <summary>
  /// Use this class to manage the files present within the <see
  /// cref="BaseFolder"/>.
  /// </summary>
  public interface IFileManager
  {
    /// <summary>
    /// The base folder from which all relative file paths are calculated.
    /// </summary>
    string BaseFolder { get; }

    /// <summary>
    /// Gets the full "rooted" path of the given <paramref
    /// name="relativePath"/>.
    /// </summary>
    string ToFullPath(string relativePath);

    /// <summary>
    /// Gets the path relative to the <see cref="BaseFolder"/>.
    /// </summary>
    string ToRelativePath(string fullPath);

    /// <summary>
    /// Gets all the files within <paramref name="relativeFolder"/> subject to
    /// the given <paramref name="searchPattern"/> and <paramref
    /// name="searchOption"/>.
    /// </summary>
    ValueTask<FileInfo[]> GetFileInfos(string relativeFolder, string searchPattern, SearchOption searchOption);

    /// <summary>
    /// Gets the relative file names of all the files within <paramref
    /// name="relativeFolder"/> subject to the given <paramref
    /// name="searchPattern"/> and <paramref name="searchOption"/>.
    /// </summary>
    ValueTask<string[]> GetRelativePaths(string relativeFolder, string searchPattern, SearchOption searchOption);

    /// <summary>
    /// Writes the given <paramref name="bytes"/> to the file at the given
    /// <paramref name="relativePath"/> and waits for the file stream to be
    /// flushed. Existing files will be overwritten.
    /// </summary>
    ValueTask WriteBytesAsync(string relativePath, byte[] bytes);

    /// <summary>
    /// Appends the given <paramref name="bytes"/> to the file at the given
    /// <paramref name="relativePath"/> and waits for the file stream to be
    /// flushed. A file will be created if it doesn't already exist.
    /// </summary>
    ValueTask AppendBytesAsync(string relativePath, byte[] bytes);

    /// <summary>
    /// Reads the content of the file at the given <paramref
    /// name="relativePath"/>. <c>null</c> is returned if the file does not
    /// exist.
    /// </summary>
    ValueTask<byte[]?> ReadBytesAsync(string relativePath);
  }
}
