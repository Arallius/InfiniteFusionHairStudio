using System;
using System.Windows.Media;

namespace InfiniteFusionHairStudio.Core
{
    public static class PaletteDatabase
    {
        public static ColorPalette GetPalette(
            HairColor color,
            HairShade shade)
        {
            return color switch
            {
                HairColor.Brown => GetBrown(shade),
                HairColor.Black => GetBlack(shade),

                _ => throw new NotImplementedException(
                    $"Palette '{color}' has not been implemented yet.")
            };
        }

        private static ColorPalette GetBrown(HairShade shade)
        {
            return shade switch
            {
                HairShade.Light => new ColorPalette(
                    Color.FromRgb(176, 115, 82),
                    Color.FromRgb(222, 174, 125),
                    Color.FromRgb(239, 207, 154),
                    Color.FromRgb(255, 236, 170)
                ),

                HairShade.Normal => new ColorPalette(
                    Color.FromRgb(143, 92, 63),
                    Color.FromRgb(214, 167, 118),
                    Color.FromRgb(230, 199, 147),
                    Color.FromRgb(245, 226, 142)
                ),

                HairShade.Dark => new ColorPalette(
                    Color.FromRgb(92, 56, 40),
                    Color.FromRgb(148, 103, 73),
                    Color.FromRgb(183, 146, 106),
                    Color.FromRgb(214, 185, 137)
                ),

                _ => throw new ArgumentOutOfRangeException(nameof(shade))
            };
        }

        private static ColorPalette GetBlack(HairShade shade)
        {
            return shade switch
            {
                HairShade.Light => new ColorPalette(
                    Color.FromRgb(55, 55, 55),
                    Color.FromRgb(95, 95, 95),
                    Color.FromRgb(145, 145, 145),
                    Color.FromRgb(205, 205, 205)
                ),

                HairShade.Normal => new ColorPalette(
                    Color.FromRgb(30, 30, 30),
                    Color.FromRgb(70, 70, 70),
                    Color.FromRgb(120, 120, 120),
                    Color.FromRgb(185, 185, 185)
                ),

                HairShade.Dark => new ColorPalette(
                    Color.FromRgb(10, 10, 10),
                    Color.FromRgb(35, 35, 35),
                    Color.FromRgb(75, 75, 75),
                    Color.FromRgb(130, 130, 130)
                ),

                _ => throw new ArgumentOutOfRangeException(nameof(shade))
            };
        }
    }
}