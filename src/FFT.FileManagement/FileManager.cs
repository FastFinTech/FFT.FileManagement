// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.FileManagement
{
  using static System.Environment;
  using static System.IO.Path;

  /// <summary>
  /// Use this class to create instances of <see cref="IFileManager"/>.
  /// </summary>
  public static class FileManager
  {
    /// <summary>
    /// Creates an <see cref="IFileManager"/> based at the given <paramref
    /// name="baseFolder"/>. Base folder can be a rooted folder. If it is not
    /// rooted, it will be assumed relative to the location of the
    /// currently-running application.
    /// </summary>
    public static IFileManager Create(string baseFolder)
        => new FileManagerImplementation(baseFolder);

    /// <summary>
    /// Creates an <see cref="IFileManager"/> based within the
    /// specialFolder/subFolder path.
    /// </summary>
    public static IFileManager Create(SpecialFolder specialFolder, string subFolder)
        => new FileManagerImplementation(Combine(GetFolderPath(specialFolder), subFolder));
  }
}
