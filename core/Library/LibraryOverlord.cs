using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace TLCMM.Core;

public static class LibraryOverlord
{
    private static readonly Lazy<HashSet<string>> DefaultLibrariesLazy = new(GetDefaultLibraries);

    public static HashSet<string> DefaultLibraries => DefaultLibrariesLazy.Value;

    public static IOrderedEnumerable<Library> GetLibraries()
    {
        return Directory
            .EnumerateFiles(
                Path.Combine(Options.Parsed.Directory.FullName, "BepInEx", "plugins"),
                "*.dll",
                new EnumerationOptions() { RecurseSubdirectories = false }
            )
            .Select(it => new Library(new FileInfo(it)))
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
}
