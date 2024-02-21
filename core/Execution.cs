namespace Tlcmm.Core;

public static class Execution
{
    public static void StartUp(string[] args)
    {
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
    }

    public static void Finish()
    {
        if (!Options.Parsed.Pause)
            return;
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);
    }
}
