using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Tlcmm.Core;

public static class LibraryOverlord
{
    private static readonly Lazy<HashSet<string>> DefaultLibrariesLazy = new(GetDefaultLibraries);

    private static readonly Lazy<DirectoryInfo> ModsDirectoryLazy = new(GetModsDirectory);

    public static DirectoryInfo ModsDirectory => ModsDirectoryLazy.Value;

    private static readonly Lazy<DirectoryInfo> StashDirectoryLazy = new(GetStashDirectory);

    public static DirectoryInfo StashDirectory => StashDirectoryLazy.Value;

    public static HashSet<string> DefaultLibraries => DefaultLibrariesLazy.Value;

    public static IOrderedEnumerable<Library> GetLibraries()
    {
        return ModsDirectory
            .EnumerateFiles(
                "*.dll",
                new EnumerationOptions()
                {
                    RecurseSubdirectories = false,
                    AttributesToSkip =
                        FileAttributes.Hidden | FileAttributes.System | FileAttributes.ReparsePoint
                }
            )
            .Concat(
                StashDirectory.EnumerateFiles(
                    "*.dll",
                    new EnumerationOptions()
                    {
                        RecurseSubdirectories = false,
                        AttributesToSkip =
                            FileAttributes.Hidden
                            | FileAttributes.System
                            | FileAttributes.ReparsePoint
                    }
                )
            )
            .Select(it => new Library(it))
            .OrderBy(it => it.Dependencies.Length);
    }

    private static HashSet<string> GetDefaultLibraries()
    {
        var defaultLibraryPaths = Directory
            .EnumerateFiles(
                Path.Combine(Options.Parsed.Directory.FullName, "Lethal Company_Data"),
                "*.dll",
                new EnumerationOptions() { RecurseSubdirectories = true }
            )
            .Concat(
                Directory.EnumerateFiles(
                    Path.Combine(Options.Parsed.Directory.FullName, "BepInEx", "core"),
                    "*.dll",
                    new EnumerationOptions() { RecurseSubdirectories = true }
                )
            );

        var defaultLibraries = new HashSet<string>();

        foreach (var path in defaultLibraryPaths)
        {
            using var stream = new FileInfo(path).Open(FileMode.Open);
            using var peReader = new PEReader(stream);
            if (!peReader.HasMetadata)
            {
                continue;
            }

            var metaReader = peReader.GetMetadataReader();

            var definition = metaReader.GetAssemblyDefinition();

            defaultLibraries.Add(metaReader.GetString(definition.Name));
        }

        return defaultLibraries;
    }

    private static DirectoryInfo GetModsDirectory()
    {
        var directory = new DirectoryInfo(
            Path.Combine(Options.Parsed.Directory.FullName, "BepInEx", "plugins")
        );
        if (!directory.Exists)
            directory.Create();

        return directory;
    }

    private static DirectoryInfo GetStashDirectory()
    {
        var directory = new DirectoryInfo(
            Path.Combine(Options.Parsed.Directory.FullName, "BepInEx", "tlcmm_stash")
        );

        if (!directory.Exists)
            directory.Create();

        return directory;
    }
}
