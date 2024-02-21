using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Input;
using ConsoleGUI.Space;

namespace TLCMM.Gui;

public class Tab : IInputListener, ISelectable
{
    private readonly Background _headerBackground;

    private readonly IReadOnlyCollection<IInputListener> _inputListeners;

    public IControl Header { get; }

    public IControl Content { get; }

    public event Action OnFocus = default!;

    public Tab(string name, IControl content, IReadOnlyCollection<IInputListener> inputListeners)
    {
        _headerBackground = new Background
        {
            Content = new Margin
            {
                Offset = new Offset(1, 0, 1, 0),
                Content = new TextBlock { Text = name }
            }
        };

        Header = new Margin { Offset = new Offset(0, 0, 1, 0), Content = _headerBackground };
        Content = content;

        _inputListeners = inputListeners;

        Select(false);
    }

    public void Select(bool state)
    {
        if (state)
        {
            _headerBackground.Color = new Color(25, 54, 65);
            OnFocus?.Invoke();
        }
        else
            _headerBackground.Color = new Color(65, 24, 25);
    }

    public void OnInput(InputEvent inputEvent)
    {
        foreach (var listner in _inputListeners)
        {
            listner.OnInput(inputEvent);
            if (inputEvent.Handled)
                return;
        }
    }
}
