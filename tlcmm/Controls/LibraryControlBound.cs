using TLCMM.Core;

namespace TLCMM.Gui;

public class LibraryControlBound(Library library) : LibraryControlSimple(library.Name, library.Version), ISelectable
{
    public Library Library { get; } = library;
}
