using System.Drawing;

namespace TLCMM;

public static class ColorExtensions
{
    public static ConsoleGUI.Data.Color Convert(this Color color) => new(color.R, color.G, color.B);
}
