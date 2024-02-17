using System.Collections;
using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Input;
using ConsoleGUI.UserDefined;

namespace TLCMM;

public class ScrollableList<T> : SimpleControl, IList<T>, IInputListener
    where T : IControl, ISelectable
{
    public Color SelectedBackground { private get; init; } =
        System.Drawing.Color.DarkSlateGray.Convert();

    public Color Background { private get; init; }

    public Color SelectedForeground { private get; init; } = Color.White;

    public Color Foreground { private get; init; } = Color.White;

    public int Count => _rows.Count;

    public bool IsReadOnly => false;

    public T this[int index]
    {
        get => _rows[index];
        set => throw new NotImplementedException();
    }

    private readonly VerticalScrollPanel _scrollPanel;

    private readonly List<T> _rows;

    private readonly VerticalStackPanel _stack;

    private int _selectionIndex;

    public ScrollableList()
    {
        _selectionIndex = 0;
        _rows = [];
        _stack = new VerticalStackPanel();

        _scrollPanel = new VerticalScrollPanel()
        {
            Content = _stack,
            ScrollBarForeground = new Character('█', System.Drawing.Color.DimGray.Convert()),
            ScrollBarBackground = new Character('║', System.Drawing.Color.DimGray.Convert())
        };

        Content = _scrollPanel;
    }

    public void Add(T item)
    {
        if (_selectionIndex == 0 && _rows.Count == 0)
            item.Select(true);

        _rows.Add(item);
        _stack.Add(item);
    }

    public void Select(int index)
    {
        if (index == _selectionIndex)
            return;

        if (index < 0 | index >= _rows.Count)
            throw new IndexOutOfRangeException();

        _rows[_selectionIndex].Select(false);

        _rows[index].Select(true);

        _selectionIndex = index;
    }

    public void OnInput(InputEvent inputEvent)
    {
        if (inputEvent.Key.Key == ConsoleKey.UpArrow)
        {
            if (_selectionIndex <= 0)
            {
                inputEvent.Handled = true;
                return;
            }

            if (_selectionIndex <= _scrollPanel.Top)
                _scrollPanel.Top -= 1;

            Select(_selectionIndex - 1);
            inputEvent.Handled = true;
        }
        if (inputEvent.Key.Key == ConsoleKey.DownArrow)
        {
            if (_selectionIndex >= _rows.Count - 1)
            {
                inputEvent.Handled = true;
                return;
            }

            if (_selectionIndex >= _scrollPanel.Top + _scrollPanel.Size.Height - 1)
                _scrollPanel.Top += 1;

            Select(_selectionIndex + 1);
            inputEvent.Handled = true;
        }
    }

    public int IndexOf(T item) => _rows.IndexOf(item);

    public void Insert(int index, T item)
    {
        throw new NotImplementedException();
    }

    public void RemoveAt(int index)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(T item) => _rows.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => _rows.CopyTo(array, arrayIndex);

    public bool Remove(T item)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<T> GetEnumerator() => _rows.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _rows.GetEnumerator();
}
