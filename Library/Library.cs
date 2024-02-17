using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace TLCMM;

public class Library
{
    private static readonly Lazy<HashSet<string>> DefaultLibraries = new(GetDefaultLibraries);

    public string Name { get; }

    public Version Version { get; }

    public (string Name, Version Version)[] Dependencies { get; }

    public Library(string name, Version version, (string, Version)[] dependencies)
    {
        Name = name;
        Version = version;
        Dependencies = dependencies;
    }

    public Library(FileInfo libraryPath)
    {
        using var stream = libraryPath.Open(FileMode.Open);
        using var peReader = new PEReader(stream);

        var metaReader = peReader.GetMetadataReader();

        var definition = metaReader.GetAssemblyDefinition();

        var dependencies = metaReader
            .AssemblyReferences.Select<AssemblyReferenceHandle, (string, Version)>(it =>
            {
                var dependency = metaReader.GetAssemblyReference(it);

                return (metaReader.GetString(dependency.Name), dependency.Version);
            })
            .Where(it => !DefaultLibraries.Value.Contains(it.Item1))
            .ToArray();

        Name = metaReader.GetString(definition.Name);
        Version = definition.Version;
        Dependencies = dependencies;
    }

    public static bool TryGetLibrary(FileInfo libraryPath, out Library? library)
    {
        using var stream = libraryPath.Open(FileMode.Open);
        using var peReader = new PEReader(stream);
        if (!peReader.HasMetadata)
        {
            library = null;
            return false;
        }

        var metaReader = peReader.GetMetadataReader();

        var definition = metaReader.GetAssemblyDefinition();

        var dependencies = metaReader
            .AssemblyReferences.Select<AssemblyReferenceHandle, (string, Version)>(it =>
            {
                var dependency = metaReader.GetAssemblyReference(it);

                return (metaReader.GetString(dependency.Name), dependency.Version);
            })
            .Where(it => !DefaultLibraries.Value.Contains(it.Item1))
            .ToArray();

        library = new Library(
            metaReader.GetString(definition.Name),
            definition.Version,
            dependencies
        );
        return true;
    }

    private static HashSet<string> GetDefaultLibraries()
    {
        var defaultLibraryPaths = Directory
            .EnumerateFiles(
                Options.Parsed.Directory.FullName,
                "*.dll",
                new EnumerationOptions() { RecurseSubdirectories = false }
            )
            .Concat(
                Directory.EnumerateFiles(
                    Path.Combine(Options.Parsed.Directory.FullName, "Lethal Company_Data"),
                    "*.dll",
                    new EnumerationOptions() { RecurseSubdirectories = true }
                )
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
