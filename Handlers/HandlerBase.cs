using System.CommandLine.Invocation;

namespace TLCMM;

public abstract class HandlerBase
{
    public void Handle(InvocationContext context)
    {
        Options.Directory = context.ParseResult.GetValueForOption(Options.DirectoryOption)!;

        Execute(context);

        var pause = context.ParseResult.GetValueForOption(Options.PauseOption);

        if (!pause)
            return;
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);
    }

    protected abstract void Execute(InvocationContext context);
}
