using System.Drawing;
using Pastel;

namespace TLCMM;

public static class PrintHandler
{
    public static void Execute()
    {
        var homeDirectory = Options.Parsed.Directory;

        var plugins = Directory
            .EnumerateFiles(
                Path.Combine(homeDirectory.FullName, "BepInEx", "plugins"),
                "*.dll",
                new EnumerationOptions() { RecurseSubdirectories = false }
            )
            .Select(it => new Library(new FileInfo(it)))
            .OrderBy(it => it.Dependencies.Length);

        foreach (var plugin in plugins)
        {
            Console.Write(plugin.Name.Pastel(Color.Green));
            var dependencies = plugin.Dependencies.Select(it => it.Name);
            if (dependencies.Any())
                Console.WriteLine(
                    " " + ("(" + string.Join(", ", dependencies) + ")").Pastel(Color.Gray)
                );
            else
                Console.WriteLine();
        }
    }
}
