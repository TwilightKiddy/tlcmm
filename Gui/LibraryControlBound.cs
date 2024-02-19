using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.UserDefined;

namespace TLCMM;

public class LibraryControlBound : LibraryControlSimple, ISelectable
{
    public Library Library { get; }

    public LibraryControlBound(Library library)
        : base(library.Name, library.Version)
    {
        Library = library;
    }
}
