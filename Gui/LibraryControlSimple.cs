using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.UserDefined;
using ConsoleGUI.Space;

namespace TLCMM;

public class LibraryControlSimple : SimpleControl, ISelectable
{
    public string Name { get; }

    public Version Version { get; }

    private readonly Background _background;

    private readonly HorizontalStackPanel _stack;

    private readonly TextBlock _nameTextBlock;

    private readonly TextBlock _versionTextBlock;

    private readonly Boundary _boundary;

    public LibraryControlSimple(string name, Version version)
    {
        Name = name;
        Version = version;

        _nameTextBlock = new() { Color = Color.White, Text = name};

        _versionTextBlock = new()
        {
            Color = System.Drawing.Color.Gray.Convert(),
            Text = $"({version})"
        };

        _stack = new();

        _boundary = new() { MaxHeight = 1, Content = _stack };

        _stack.Add(new Margin() { Content = _nameTextBlock, Offset = new Offset(0, 0, 1, 0) });
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
