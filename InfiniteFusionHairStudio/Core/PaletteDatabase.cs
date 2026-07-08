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
                HairColor.Blonde => GetBlonde(shade),
                HairColor.Black => GetBlack(shade),
                HairColor.Purple => GetPurple(shade),
                HairColor.Red => GetRed(shade),
                HairColor.Pink => GetPink(shade),
                HairColor.Blue => GetBlue(shade),
                HairColor.Green => GetGreen(shade),
                HairColor.White => GetWhite(shade),
                HairColor.Silver => GetSilver(shade),
                HairColor.Orange => GetOrange(shade),

                _ => throw new ArgumentOutOfRangeException(nameof(color))
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

        private static ColorPalette GetBlonde(HairShade shade) =>
            Build(shade,
                Color.FromRgb(255, 225, 130),
                Color.FromRgb(235, 205, 90),
                Color.FromRgb(190, 155, 55));

        private static ColorPalette GetPurple(HairShade shade) =>
            Build(shade,
                Color.FromRgb(185, 130, 255),
                Color.FromRgb(130, 70, 210),
                Color.FromRgb(80, 35, 140));

        private static ColorPalette GetRed(HairShade shade) =>
            Build(shade,
                Color.FromRgb(255, 120, 120),
                Color.FromRgb(205, 55, 55),
                Color.FromRgb(130, 25, 25));

        private static ColorPalette GetPink(HairShade shade) =>
            Build(shade,
                Color.FromRgb(255, 190, 220),
                Color.FromRgb(235, 115, 170),
                Color.FromRgb(180, 65, 120));

        private static ColorPalette GetBlue(HairShade shade) =>
            Build(shade,
                Color.FromRgb(135, 190, 255),
                Color.FromRgb(65, 120, 220),
                Color.FromRgb(35, 65, 145));

        private static ColorPalette GetGreen(HairShade shade) =>
            Build(shade,
                Color.FromRgb(145, 235, 145),
                Color.FromRgb(65, 170, 65),
                Color.FromRgb(35, 105, 35));

        private static ColorPalette GetWhite(HairShade shade) =>
            Build(shade,
                Color.FromRgb(255, 255, 255),
                Color.FromRgb(225, 225, 225),
                Color.FromRgb(185, 185, 185));

        private static ColorPalette GetSilver(HairShade shade) =>
            Build(shade,
                Color.FromRgb(230, 230, 240),
                Color.FromRgb(175, 175, 190),
                Color.FromRgb(120, 120, 140));

        private static ColorPalette GetOrange(HairShade shade) =>
            Build(shade,
                Color.FromRgb(255, 190, 120),
                Color.FromRgb(235, 130, 45),
                Color.FromRgb(170, 75, 15));

        private static ColorPalette Build(
            HairShade shade,
            Color light,
            Color normal,
            Color dark)
        {
            return shade switch
            {
                HairShade.Light => new ColorPalette(
                    Lighten(light, 35),
                    Lighten(light, 20),
                    Lighten(light, 10),
                    Lighten(light, 50)),

                HairShade.Normal => new ColorPalette(
                    Darken(normal, 40),
                    normal,
                    Lighten(normal, 25),
                    Lighten(normal, 50)),

                HairShade.Dark => new ColorPalette(
                    Darken(dark, 50),
                    Darken(dark, 25),
                    dark,
                    Lighten(dark, 25)),

                _ => throw new ArgumentOutOfRangeException(nameof(shade))
            };
        }

        private static Color Lighten(
            Color color,
            byte amount)
        {
            return Color.FromRgb(
                Add(color.R, amount),
                Add(color.G, amount),
                Add(color.B, amount));
        }

        private static Color Darken(
            Color color,
            byte amount)
        {
            return Color.FromRgb(
                Subtract(color.R, amount),
                Subtract(color.G, amount),
                Subtract(color.B, amount));
        }

        private static byte Add(byte value, byte amount)
        {
            int result = value + amount;
            return (byte)Math.Min(result, 255);
        }

        private static byte Subtract(byte value, byte amount)
        {
            int result = value - amount;
            return (byte)Math.Max(result, 0);
        }
    }
}