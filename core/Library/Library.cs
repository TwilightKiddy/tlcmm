using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace TLCMM.Core;

public class Library
{
    private readonly string _name;

    private readonly Version _version;

    private readonly (string Name, Version Version)[] _dependencies;

    public string Name => _name;

    public Version Version => _version;

    public (string Name, Version Version)[] Dependencies => _dependencies;

    private Library(string name, Version version, (string, Version)[] dependencies)
    {
        _name = name;
        _version = version;
        _dependencies = dependencies;
    }

    public Library(FileInfo libraryPath)
    {
        using var stream = libraryPath.Open(FileMode.Open);
        using var peReader = new PEReader(stream);
        GetMetadata(peReader, out _name, out _version, out _dependencies);
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

        GetMetadata(peReader, out var name, out var version, out var dependencies);

        library = new Library(name, version, dependencies);
        return true;
    }

    private static void GetMetadata(
        PEReader peReader,
        out string name,
        out Version version,
        out (string, Version)[] dependencies
    )
    {
        var metaReader = peReader.GetMetadataReader();

        var definition = metaReader.GetAssemblyDefinition();

        dependencies = metaReader
            .AssemblyReferences.Select<AssemblyReferenceHandle, (string Name, Version Version)>(
                it =>
                {
                    var dependency = metaReader.GetAssemblyReference(it);

                    return (metaReader.GetString(dependency.Name), dependency.Version);
                }
            )
            .Where(it => !LibraryOverlord.DefaultLibraries.Contains(it.Name))
            .ToArray();

        name = metaReader.GetString(definition.Name);
        version = definition.Version;
    }
}
