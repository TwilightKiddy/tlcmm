using TLCMM;

var parser = new CommandLineParser.CommandLineParser();

parser.ExtractArgumentAttributes(Options.Parsed);

try
{
    parser.ParseCommandLine(args);
    if (!parser.ParsingSucceeded)
        return;
    Options.ValidateOptions();
}
catch (Exception ex)
{
    Console.Error.WriteLine("Error: " + ex.Message);
    parser.ShowUsage();
    return;
}

switch (Options.Parsed.Mode)
{
    case Mode.Print:
        PrintHandler.Execute();
        break;
    case Mode.Gui:
        PrintHandler.Execute();
        break;
    default:
        Console.Error.WriteLine($"Error: '{Options.Parsed.Mode}' is not a valid mode.");
        parser.ShowUsage();
        return;
}

if (!Options.Parsed.Pause)
    return;
Console.WriteLine("Press any key to continue...");
Console.ReadKey(true);
