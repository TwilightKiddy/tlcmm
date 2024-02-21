using ConsoleGUI;
using TLCMM.Core;
using TLCMM.Gui;

Execution.StartUp(args);

ConsoleManager.Setup();

ConsoleManager.Content = Layout.Build();

var globalHotkeys = new GlobalHotkeys();

while (!globalHotkeys.TerminationRequested)
{
    Thread.Sleep(50);
    ConsoleManager.AdjustBufferSize();

    ConsoleManager.ReadInput(Layout.InputListeners.Prepend(globalHotkeys).ToArray());
}

Console.BackgroundColor = (ConsoleColor)(-1);
Console.ForegroundColor = (ConsoleColor)(-1);
Console.Clear();
Console.CursorVisible = true;

Execution.Finish();
