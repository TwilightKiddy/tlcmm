using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Input;
using ConsoleGUI.Space;
using ConsoleGUI.UserDefined;

namespace TLCMM;

public class TabPanel : SimpleControl, IInputListener
{
    private readonly List<Tab> _tabs = new();
    private readonly DockPanel _wrapper;
    private readonly HorizontalStackPanel _tabsPanel;

    private Tab currentTab;

    public TabPanel()
    {
        _tabsPanel = new HorizontalStackPanel();

        _wrapper = new DockPanel
        {
            Placement = DockPanel.DockedControlPlacement.Top,
            DockedControl = new Background
            {
                Color = new Color(25, 25, 52),
                Content = new Boundary
                {
                    MinHeight = 1,
                    MaxHeight = 1,
                    Content = _tabsPanel
                }
            }
        };

        Content = _wrapper;
    }

    public void AddTab(Tab tab)
    {
        _tabs.Add(tab);
        _tabsPanel.Add(tab.Header);
        if (_tabs.Count == 1)
            SelectTab(0);
    }

    public void SelectTab(int tab)
    {
        currentTab?.Select(false);
        currentTab = _tabs[tab];
        currentTab.Select(true);
        _wrapper.FillingControl = currentTab.Content;
    }

    public void OnInput(InputEvent inputEvent)
    {
        switch (inputEvent.Key.Key)
        {
            case ConsoleKey.Tab when (inputEvent.Key.Modifiers & ConsoleModifiers.Shift) != 0:
            case ConsoleKey.LeftArrow:
                SelectTab((_tabs.Count + _tabs.IndexOf(currentTab) - 1) % _tabs.Count);
                inputEvent.Handled = true;
                return;
            case ConsoleKey.Tab:
            case ConsoleKey.RightArrow:
                SelectTab((_tabs.IndexOf(currentTab) + 1) % _tabs.Count);
                inputEvent.Handled = true;
                return;
            default:
                currentTab.OnInput(inputEvent);
                return;
        }
    }
}
