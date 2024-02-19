using ConsoleGUI;

namespace TLCMM;

public static class GuiHandler
{
    public static void Execute()
    {
        ConsoleManager.Setup();

        ConsoleManager.Content = Layout.Build();

        while (true)
        {
            Thread.Sleep(10);
            ConsoleManager.AdjustBufferSize();

            ConsoleManager.ReadInput(Layout.InputListeners);
        }
    }
}
