using System.Drawing;

namespace MetX.Controls
{
    public class ColorScheme
    {
        public Color PanelBG;
        public Color PanelFG;
        public Color ButtonBG;
        public Color ButtonFG;
        public Color TextBoxBG;
        public Color TextBoxFG;

        public static ColorScheme DarkThemeOne =>
            new()
            {
                PanelBG = Color.FromArgb(30, 30, 30),
                PanelFG = Color.FromArgb(97, 97, 104),
                ButtonBG = Color.FromArgb(21, 83, 152),
                ButtonFG = Color.FromArgb(168, 171, 173),
                TextBoxBG = Color.FromArgb(39, 39, 42),
                TextBoxFG = Color.FromArgb(203, 203, 203)
            };

        public static Color rgb(int red, int green, int blue) => Color.FromArgb(red, green, blue);
    }
}