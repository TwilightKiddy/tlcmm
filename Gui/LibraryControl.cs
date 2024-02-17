using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.UserDefined;

namespace TLCMM;

public class LibraryControl : SimpleControl, ISelectable
{
    public Library Library { get; }

    private readonly Background _background;

    private readonly HorizontalStackPanel _stack;

    private readonly TextBlock _nameTextBlock;

    private readonly TextBlock _versionTextBlock;

    private readonly Boundary _boundary;

    public LibraryControl(Library library)
    {
        Library = library;

        _nameTextBlock = new() { Color = Color.White, Text = library.Name + " " };

        _versionTextBlock = new()
        {
            Color = System.Drawing.Color.Gray.Convert(),
            Text = $"({library.Version})"
        };

        _stack = new();

        _boundary = new() { MaxHeight = 1, Content = _stack };

        _stack.Add(_nameTextBlock);
        _stack.Add(_versionTextBlock);

        _background = new() { Content = _boundary };

        Content = _background;
    }

    public void Select(bool state)
    {
        if (state)
        {
            _background.Color = System.Drawing.Color.DimGray.Convert();
        }
        else
        {
            _background.Color = Color.Black;
        }
    }
}
