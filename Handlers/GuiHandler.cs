using System.CommandLine.Invocation;
using System.Drawing;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;

namespace TLCMM;

public static class GuiHandler
{
    public static void Execute()
    {
        ConsoleManager.Setup();
        var list = new ScrollableList<LibraryControl>();

        for (int i = 0; i < 50; i++)
        {
            list.Add(new LibraryControl(new Library("Library", new Version(i, 50 - i), null!)));
        }

        var outerContainer = new DockPanel()
        {
            Placement = DockPanel.DockedControlPlacement.Bottom,
            FillingControl = new Background() { Content = new Boundary() { Content = list } },
            DockedControl = new Background()
            {
                Color = Color.Green.Convert(),
                Content = new TextBlock()
                {
                    Text =
                        "An exceedingly long text that spans across the terminal and makes the text go over the edge, that needs to be a lot longer than I thought to accomplish a goal I set for it.",
                }
            }
        };

        ConsoleManager.Content = outerContainer;

        var inputListners = new[] { list };

        while (true)
        {
            Thread.Sleep(10);
            ConsoleManager.AdjustBufferSize();

            ConsoleManager.ReadInput(inputListners);
        }
    }
}
