using System.Drawing;
using Pastel;

namespace TLCMM;

public static class PrintHandler
{
    public static void Execute()
    {
        var libraries = LibraryOverlord.GetLibraries();

        foreach (var library in libraries)
        {
            Console.Write(library.Name.Pastel(Color.Green));
            var dependencies = library.Dependencies.Select(it => it.Name);
            if (dependencies.Any())
                Console.WriteLine(
                    " " + ("(" + string.Join(", ", dependencies) + ")").Pastel(Color.Gray)
                );
            else
                Console.WriteLine();
        }
    }
}
