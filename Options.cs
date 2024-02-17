using System.Runtime.CompilerServices;
using CommandLineParser.Arguments;
using Microsoft.VisualBasic;

namespace TLCMM;

public class Options
{
    public static readonly Options Parsed = new();

    [ValueArgument(
        typeof(Mode),
        'm',
        "mode",
        Description = "The mode for program to run in.",
        DefaultValue = Mode.Gui
    )]
    public Mode Mode;

    [DirectoryArgument(
        'd',
        "directory",
        Description = "Path to Lethal Company directory.",
        DirectoryMustExist = true
    )]
    public DirectoryInfo Directory = default!;

    [SwitchArgument(
        'p',
        "pause",
        false,
        Description = "Wait for the user input before program termination."
    )]
    public bool Pause;

    public static void ValidateOptions()
    {
        Parsed.Directory ??= new FileInfo(
            System.Reflection.Assembly.GetEntryAssembly()!.Location
        ).Directory!;

        var bepInExDirectory = Path.Combine(Parsed.Directory.FullName, "BepInEx");

        if (!System.IO.Directory.Exists(bepInExDirectory))
            throw new ArgumentException(
                $"'{Parsed.Directory}' is not a valid Lethal Company directory with BepInEx installed."
            );
    }
}

public enum Mode
{
    Print,

    Gui,
}
