using Tlcmm.Core;

namespace Tlcmm.Gui;

public class LibraryControlBound(Library library)
    : LibraryControlSimple(library.Name, library.Version, library.Enabled),
        ISelectable
{
    public Library Library { get; } = library;
}
