﻿// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.FileManagement
{
  using System;
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
    Task<FileInfo[]> GetFileInfos(string relativeFolder, string searchPattern, SearchOption searchOption);

    /// <summary>
    /// Gets the relative file names of all the files within <paramref
    /// name="relativeFolder"/> subject to the given <paramref
    /// name="searchPattern"/> and <paramref name="searchOption"/>.
    /// </summary>
    Task<string[]> GetRelativePaths(string relativeFolder, string searchPattern, SearchOption searchOption);

#if NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Writes the given <paramref name="bytes"/> to the file at the given
    /// <paramref name="relativePath"/> and waits for the file stream to be
    /// flushed. Existing files will be overwritten.
    /// </summary>
    Task WriteBytesAsync(string relativePath, Memory<byte> bytes);

    /// <summary>
    /// Writes the given <paramref name="bytes"/> to the file at the given
    /// <paramref name="relativePath"/> and waits for the file stream to be
    /// flushed. Existing files will be overwritten.
    /// </summary>
    Task WriteBytesAsync(string relativePath, ReadOnlySequence<byte> bytes);

    /// <summary>
    /// Appends the given <paramref name="bytes"/> to the file at the given
    /// <paramref name="relativePath"/> and waits for the file stream to be
    /// flushed. A file will be created if it doesn't already exist.
    /// </summary>
    Task AppendBytesAsync(string relativePath, Memory<byte> bytes);

    /// <summary>
    /// Appends the given <paramref name="bytes"/> to the file at the given
    /// <paramref name="relativePath"/> and waits for the file stream to be
    /// flushed. A file will be created if it doesn't already exist.
    /// </summary>
    Task AppendBytesAsync(string relativePath, ReadOnlySequence<byte> bytes);

    /// <summary>
    /// Reads the content of the file at the given <paramref
    /// name="relativePath"/>.
    /// </summary>
    Task<byte[]?> ReadBytesAsync(string relativePath);

    /// <summary>
    /// Reads the content of the file at the given <paramref
    /// name="relativePath"/> into the given <paramref name="writer"/>.
    /// Returns the number of bytes written.
    /// </summary>
    Task<int> ReadBytesAsync(string relativePath, IBufferWriter<byte> writer);
#else
    /// <summary>
    /// Writes the given <paramref name="bytes"/> to the file at the given
    /// <paramref name="relativePath"/> and waits for the file stream to be
    /// flushed. Existing files will be overwritten.
    /// </summary>
    Task WriteBytesAsync(string relativePath, byte[] bytes);

    /// <summary>
    /// Appends the given <paramref name="bytes"/> to the file at the given
    /// <paramref name="relativePath"/> and waits for the file stream to be
    /// flushed. A file will be created if it doesn't already exist.
    /// </summary>
    Task AppendBytesAsync(string relativePath, byte[] bytes);

    /// <summary>
    /// Reads the content of the file at the given <paramref
    /// name="relativePath"/>, returning <see langword="null"/> if the file doesn't exist.
    /// </summary>
    Task<byte[]?> ReadBytesAsync(string relativePath);
#endif
  }
}
