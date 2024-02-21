using System.Drawing;
using Pastel;
using Tlcmm.Core;

Execution.StartUp(args);

var libraries = LibraryOverlord.GetLibraries();

foreach (var library in libraries)
{
    Console.Write(library.Name.Pastel(Color.Green));
    var dependencies = library.Dependencies.Select(it => it.Name);
    if (dependencies.Any())
        Console.WriteLine(" " + ("(" + string.Join(", ", dependencies) + ")").Pastel(Color.Gray));
    else
        Console.WriteLine();
}

Execution.Finish();
