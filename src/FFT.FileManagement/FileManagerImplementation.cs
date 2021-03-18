// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.FileManagement
{
  using System;
  using System.Buffers;
  using System.IO;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using static System.Math;

  internal sealed class FileManagerImplementation : IFileManager
  {
    private static readonly string _singleSeparator = string.Empty + Path.DirectorySeparatorChar;
    private static readonly string _doubleSeparater = string.Empty + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar;

    internal FileManagerImplementation(string baseFolder)
    {
      baseFolder = Cleanup(baseFolder);
      if (!baseFolder.EndsWith(_singleSeparator))
        baseFolder += _singleSeparator;
      if (!Directory.Exists(baseFolder))
      {
        Directory.CreateDirectory(baseFolder);
        // Prevents bugs that happens when code continues executing before the
        // directly actually creates itself.
        Thread.Sleep(1);
      }

      BaseFolder = baseFolder;
    }

    public string BaseFolder { get; }

    /// <inheritdoc/>
    public string ToFullPath(string relativePath)
    {
      var fullPath = Path.Combine(BaseFolder, Cleanup(relativePath));
      var directoryName = Path.GetDirectoryName(fullPath);
      if (!Directory.Exists(directoryName))
      {
        Directory.CreateDirectory(directoryName);
        // Prevents bugs that happens when code continues executing before the
        // directly actually creates itself.
        Thread.Sleep(1);
      }

      return fullPath;
    }

    /// <inheritdoc/>
    public string ToRelativePath(string fullPath)
        => Cleanup(fullPath).Replace(BaseFolder, string.Empty);

    /// <inheritdoc/>
    public async ValueTask<FileInfo[]> GetFileInfos(string relativeFolder, string searchPattern, SearchOption searchOption)
      // C# doesn't yet have an api for doing this asynchronously, however, we
      // do wrap this inside Task.Run and await it so that we can prevent
      // blocking the UI if this method is being called from a UI
      // synchronization context.
      => await Task.Run(() => new DirectoryInfo(ToFullPath(relativeFolder)).GetFiles(searchPattern, searchOption));

    /// <inheritdoc/>
    public async ValueTask<string[]> GetRelativePaths(string relativeFolder, string searchPattern, SearchOption searchOption)
      => (await GetFileInfos(relativeFolder, searchPattern, searchOption))
        .Select(x => x.FullName.Replace(BaseFolder, string.Empty)).ToArray();

    /// <inheritdoc/>
    public async ValueTask<byte[]?> ReadBytesAsync(string relativePath)
    {
      using var stream = new FileStream(ToFullPath(relativePath), FileMode.Open);
      var bytes = new byte[stream.Length];
      var offset = 0;
      while (offset < bytes.Length)
      {
        var numBytesRead = await stream.ReadAsync(bytes, offset, bytes.Length - offset);
        if (numBytesRead == 0)
          return null;
        offset += numBytesRead;
      }

      return bytes;
    }

    /// <inheritdoc/>
    public async ValueTask<int> ReadBytesAsync(string relativePath, IBufferWriter<byte> writer)
    {
      using var stream = new FileStream(ToFullPath(relativePath), FileMode.Open);

      if (stream.Length > int.MaxValue)
        throw new System.Exception("Stream is too long.");

      var bufferSize = Max(1024, Min((int)stream.Length, 64 * 1024));
      while (stream.Position < stream.Length)
      {
        var memory = writer.GetMemory(bufferSize);
        writer.Advance(await stream.ReadAsync(memory));
      }

      return (int)stream.Length;
    }

    /// <inheritdoc/>
    public async ValueTask WriteBytesAsync(string relativePath, Memory<byte> bytes)
    {
      using var stream = new FileStream(ToFullPath(relativePath), FileMode.Create);
      await stream.WriteAsync(bytes);
      await stream.FlushAsync();
    }

    /// <inheritdoc/>
    public async ValueTask WriteBytesAsync(string relativePath, ReadOnlySequence<byte> bytes)
    {
      using var stream = new FileStream(ToFullPath(relativePath), FileMode.Create);
      foreach (var segment in bytes)
        await stream.WriteAsync(segment);
      await stream.FlushAsync();
    }

    /// <inheritdoc/>
    public async ValueTask AppendBytesAsync(string relativePath, Memory<byte> bytes)
    {
      using var stream = new FileStream(ToFullPath(relativePath), FileMode.Append);
      await stream.WriteAsync(bytes);
      await stream.FlushAsync();
    }

    /// <inheritdoc/>
    public async ValueTask AppendBytesAsync(string relativePath, ReadOnlySequence<byte> bytes)
    {
      using var stream = new FileStream(ToFullPath(relativePath), FileMode.Append);
      foreach (var segment in bytes)
        await stream.WriteAsync(segment);
      await stream.FlushAsync();
    }

    private static string Cleanup(string path)
    {
      path = path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
      while (path.Contains(_doubleSeparater))
        path = path.Replace(_doubleSeparater, _singleSeparator);
      return path;
    }
  }
}
