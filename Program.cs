using System.CommandLine;
using TLCMM;

var printCommand = new Command("print", "Print current installed mods.");
printCommand.AddAlias("p");
printCommand.SetHandler(new PrintHandler().Handle);

var rootCommand = new RootCommand("TwilightKiddy's Lethal Company Mod Manager");
rootCommand.AddCommand(printCommand);

rootCommand.AddGlobalOption(Options.DirectoryOption);
rootCommand.AddGlobalOption(Options.PauseOption);

rootCommand.Invoke(args);
