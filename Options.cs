using System.CommandLine;

namespace TLCMM;

public static class Options
{
    private static readonly DirectoryInfo _home = new FileInfo(
        System.Reflection.Assembly.GetEntryAssembly()!.Location
    ).Directory!;

    // Global options

    public static readonly Option<DirectoryInfo> DirectoryOption =
        new(new[] { "--directory", "-d" }, () => _home, "Path to Lethal Company directory.");

    public static readonly Option<bool> PauseOption =
        new(
            new[] { "--pause", "-p" },
            () => false,
            "Wait for the user input before program termination."
        );

    // Values for global options

    public static DirectoryInfo Directory { get; set; }
}
