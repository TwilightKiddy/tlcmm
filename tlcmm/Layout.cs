using System.Drawing;
using ConsoleGUI.Common;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using Tlcmm.Core;

namespace Tlcmm.Gui;

public static class Layout
{
    public static readonly List<IInputListener> InputListeners = [];

    public static Control Build()
    {
        var modsList = new ScrollableList<LibraryControlBound>();

        modsList.OnExecute += libraryControl =>
        {
            if (libraryControl.Library.Enabled)
            {
                libraryControl.Library.Enabled = false;
                libraryControl.Disable();
            }
            else
            {
                libraryControl.Library.Enabled = true;
                libraryControl.Enable();
            }
        };

        foreach (var library in LibraryOverlord.GetLibraries())
        {
            modsList.Add(new LibraryControlBound(library));
        }

        var dependenciesList = new ScrollableList<LibraryControlSimple>();
        var dependenciesHeader = new TextBlock();

        var tabPanel = new TabPanel();

        tabPanel.AddTab(new Tab("Mods", modsList, [modsList]));
        var dependenciesTab = new Tab(
            "Dependencies",
            new DockPanel()
            {
                Placement = DockPanel.DockedControlPlacement.Top,
                DockedControl = dependenciesHeader,
                FillingControl = dependenciesList
            },
            [dependenciesList]
        );
        dependenciesTab.OnFocus += () =>
        {
            dependenciesList.Clear();
            dependenciesHeader.Text = $"Dependencies for {modsList.SelectedItem.Library.Name}:";
            foreach (var dependency in modsList.SelectedItem.Library.Dependencies)
            {
                dependenciesList.Add(
                    new LibraryControlSimple(dependency.Name, dependency.Version, true)
                );
            }
        };
        tabPanel.AddTab(dependenciesTab);

        tabPanel.AddTab(new Tab("Help", new Background(), []));
        tabPanel.AddTab(new Tab("About", new Background(), []));

        var outerContainer = new DockPanel()
        {
            Placement = DockPanel.DockedControlPlacement.Bottom,
            FillingControl = tabPanel,
            DockedControl = new Background()
            {
                Color = Color.Green.Convert(),
                Content = new TextBlock() { Text = "", }
            }
        };

        InputListeners.Add(tabPanel);

        return outerContainer;
    }
}
