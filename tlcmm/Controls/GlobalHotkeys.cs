using ConsoleGUI.Input;

namespace Tlcmm.Gui;

public class GlobalHotkeys : IInputListener
{
    public bool TerminationRequested { get; private set; }

    public void OnInput(InputEvent inputEvent)
    {
        switch (inputEvent.Key.Key)
        {
            case ConsoleKey.X when (inputEvent.Key.Modifiers & ConsoleModifiers.Control) != 0:
                TerminationRequested = true;
                break;
            default:
                return;
        }

        inputEvent.Handled = true;
    }
}
