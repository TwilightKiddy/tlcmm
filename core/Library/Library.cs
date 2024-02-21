using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Tlcmm.Core;

public class Library
{
    private readonly string _name;

    private readonly Version _version;

    private readonly (string Name, Version Version)[] _dependencies;

    private readonly FileInfo EnabledFile;

    private readonly FileInfo StashedFile;

    public string Name => _name;

    public Version Version => _version;

    public (string Name, Version Version)[] Dependencies => _dependencies;

    public string FileName { get; }

    public bool Enabled
    {
        get
        {
            EnabledFile.Refresh();
            return EnabledFile.Exists;
        }
        set
        {
            if (value == Enabled)
                return;
            if (value)
                EnabledFile.CreateAsSymbolicLink(
                    Path.Combine(LibraryOverlord.StashDirectory.FullName, FileName)
                );
            else
            {
                if (EnabledFile.LinkTarget != null)
                {
                    EnabledFile.Delete();
                    return;
                }

                File.Move(
                    EnabledFile.FullName,
                    Path.Combine(LibraryOverlord.StashDirectory.FullName, FileName)
                );
            }
        }
    }

    private Library(string name, Version version, (string, Version)[] dependencies, string fileName)
    {
        _name = name;
        _version = version;
        _dependencies = dependencies;
        FileName = fileName;
        EnabledFile = new FileInfo(Path.Combine(LibraryOverlord.ModsDirectory.FullName, FileName));
        StashedFile = new FileInfo(Path.Combine(LibraryOverlord.StashDirectory.FullName, FileName));
    }

    public Library(FileInfo libraryPath)
    {
        using var stream = libraryPath.Open(FileMode.Open);
        using var peReader = new PEReader(stream);
        GetMetadata(peReader, out _name, out _version, out _dependencies);
        FileName = libraryPath.Name;
        EnabledFile = new FileInfo(Path.Combine(LibraryOverlord.ModsDirectory.FullName, FileName));
        StashedFile = new FileInfo(Path.Combine(LibraryOverlord.StashDirectory.FullName, FileName));
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

        library = new Library(name, version, dependencies, libraryPath.Name);
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
