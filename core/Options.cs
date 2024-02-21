using CommandLineParser.Arguments;

namespace TLCMM.Core;

public class Options
{
    public static readonly Options Parsed = new();

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
        Parsed.Directory ??= new FileInfo(System.Reflection.Assembly.GetEntryAssembly()!.Location)
            .Directory!
            .Parent!;

        var bepInExDirectory = Path.Combine(Parsed.Directory.FullName, "BepInEx");

        if (!System.IO.Directory.Exists(bepInExDirectory))
            throw new ArgumentException(
                $"'{Parsed.Directory}' is not a valid Lethal Company directory with BepInEx installed."
            );
    }
}
