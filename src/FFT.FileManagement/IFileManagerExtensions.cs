// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.FileManagement
{
  using System.Text;
  using System.Threading.Tasks;

  /// <summary>
  /// Contains common extension methods for <see cref="IFileManager"/>.
  /// </summary>
  public static class IFileManagerExtensions
  {
    /// <summary>
    /// Writes the given <paramref name="value"/> in UTF-8 format to the given
    /// <paramref name="relativePath"/>.
    /// </summary>
    public static ValueTask WriteUtf8StringAsync(this IFileManager dataFiles, string relativePath, string value)
      => dataFiles.WriteBytesAsync(relativePath, value.GetBytes());

    /// <summary>
    /// Appends the given <paramref name="value"/> in UTF-8 format to the given
    /// <paramref name="relativePath"/>.
    /// </summary>
    public static ValueTask AppendUtf8StringAsync(this IFileManager dataFiles, string relativePath, string value)
      => dataFiles.AppendBytesAsync(relativePath, value.GetBytes());

    /// <summary>
    /// Reads a string from the given UTF-8-formatted <paramref
    /// name="relativePath"/>. The file is assumed to contain a string with UTF8
    /// encoding. Returns <c>null</c> if the file does not exist.
    /// </summary>
    public static async ValueTask<string?> ReadUtf8StringAsync(this IFileManager dataFiles, string relativePath)
      => (await dataFiles.ReadBytesAsync(relativePath))?.GetString();

    private static byte[] GetBytes(this string value)
      => Encoding.UTF8.GetBytes(value);

    private static string GetString(this byte[] bytes)
      => Encoding.UTF8.GetString(bytes);
  }
}
