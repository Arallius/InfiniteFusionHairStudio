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
            ColorPalette light = new(
                Color.FromRgb(176, 115, 82),
                Color.FromRgb(222, 174, 125),
                Color.FromRgb(239, 207, 154),
                Color.FromRgb(255, 236, 170));

            ColorPalette normal = new(
                Color.FromRgb(143, 92, 63),
                Color.FromRgb(214, 167, 118),
                Color.FromRgb(230, 199, 147),
                Color.FromRgb(245, 226, 142));

            ColorPalette dark = new(
                Color.FromRgb(92, 56, 40),
                Color.FromRgb(148, 103, 73),
                Color.FromRgb(183, 146, 106),
                Color.FromRgb(214, 185, 137));

            return InterpolatePalette(
                shade,
                light,
                normal,
                dark);
        }

        private static ColorPalette GetBlack(HairShade shade)
        {
            ColorPalette light = new(
                Color.FromRgb(55, 55, 55),
                Color.FromRgb(95, 95, 95),
                Color.FromRgb(145, 145, 145),
                Color.FromRgb(205, 205, 205));

            ColorPalette normal = new(
                Color.FromRgb(30, 30, 30),
                Color.FromRgb(70, 70, 70),
                Color.FromRgb(120, 120, 120),
                Color.FromRgb(185, 185, 185));

            ColorPalette dark = new(
                Color.FromRgb(10, 10, 10),
                Color.FromRgb(35, 35, 35),
                Color.FromRgb(75, 75, 75),
                Color.FromRgb(130, 130, 130));

            return InterpolatePalette(
                shade,
                light,
                normal,
                dark);
        }

        private static ColorPalette GetBlonde(HairShade shade) =>
            Build(shade,
                Color.FromRgb(245, 215, 125),
                Color.FromRgb(220, 185, 90),
                Color.FromRgb(175, 140, 55));

        private static ColorPalette GetPurple(HairShade shade) =>
            Build(shade,
                Color.FromRgb(170, 120, 235),
                Color.FromRgb(120, 65, 185),
                Color.FromRgb(75, 35, 120));

        private static ColorPalette GetRed(HairShade shade) =>
            Build(shade,
                Color.FromRgb(225, 110, 110),
                Color.FromRgb(180, 55, 55),
                Color.FromRgb(115, 25, 25));

        private static ColorPalette GetPink(HairShade shade) =>
            Build(shade,
                Color.FromRgb(240, 175, 205),
                Color.FromRgb(210, 110, 155),
                Color.FromRgb(160, 65, 110));

        private static ColorPalette GetBlue(HairShade shade) =>
            Build(shade,
                Color.FromRgb(125, 175, 235),
                Color.FromRgb(60, 110, 195),
                Color.FromRgb(35, 60, 130));

        private static ColorPalette GetGreen(HairShade shade) =>
            Build(shade,
                Color.FromRgb(135, 215, 135),
                Color.FromRgb(60, 145, 60),
                Color.FromRgb(35, 90, 35));

        private static ColorPalette GetWhite(HairShade shade) =>
            Build(shade,
                Color.FromRgb(250, 248, 245),
                Color.FromRgb(225, 223, 220),
                Color.FromRgb(185, 183, 180));

        private static ColorPalette GetSilver(HairShade shade) =>
            Build(shade,
                Color.FromRgb(220, 222, 230),
                Color.FromRgb(170, 172, 185),
                Color.FromRgb(115, 118, 135));

        private static ColorPalette GetOrange(HairShade shade) =>
            Build(shade,
                Color.FromRgb(240, 175, 110),
                Color.FromRgb(215, 120, 50),
                Color.FromRgb(150, 70, 20));

        private static ColorPalette InterpolatePalette(
            HairShade shade,
            ColorPalette light,
            ColorPalette normal,
            ColorPalette dark)
        {
            return shade switch
            {
                HairShade.Light => light,

                HairShade.Lightish => Blend(
                    light,
                    normal),

                HairShade.Normal => normal,

                HairShade.Darkish => Blend(
                    normal,
                    dark),

                HairShade.Dark => dark,

                _ => throw new ArgumentOutOfRangeException(nameof(shade))
            };
        }

        private static ColorPalette Build(
            HairShade shade,
            Color light,
            Color normal,
            Color dark)
        {
            return shade switch
            {
                HairShade.Light => new ColorPalette(
                    Lighten(light, 25),
                    Lighten(light, 12),
                    light,
                    Lighten(light, 40)),

                HairShade.Lightish => new ColorPalette(
                    Midpoint(
                        Darken(normal, 40),
                        Lighten(light, 25)),
                    Midpoint(
                        normal,
                        light),
                    Midpoint(
                        Lighten(normal, 25),
                        Lighten(light, 20)),
                    Midpoint(
                        Lighten(normal, 50),
                        Lighten(light, 40))),

                HairShade.Normal => new ColorPalette(
                    Darken(normal, 40),
                    normal,
                    Lighten(normal, 25),
                    Lighten(normal, 50)),

                HairShade.Darkish => new ColorPalette(
                    Midpoint(
                        Darken(dark, 50),
                        Darken(normal, 40)),
                    Midpoint(
                        Darken(dark, 25),
                        normal),
                    Midpoint(
                        dark,
                        Lighten(normal, 25)),
                    Midpoint(
                        Lighten(dark, 25),
                        Lighten(normal, 50))),

                HairShade.Dark => new ColorPalette(
                    Darken(dark, 50),
                    Darken(dark, 25),
                    dark,
                    Lighten(dark, 25)),

                _ => throw new ArgumentOutOfRangeException(nameof(shade))
            };
        }

        private static ColorPalette Blend(
            ColorPalette a,
            ColorPalette b)
        {
            return new ColorPalette(
                Midpoint(a.Dark, b.Dark),
                Midpoint(a.Mid, b.Mid),
                Midpoint(a.Light, b.Light),
                Midpoint(a.Highlight, b.Highlight));
        }

        private static Color Midpoint(
            Color a,
            Color b)
        {
            return Color.FromRgb(
                (byte)((a.R + b.R) / 2),
                (byte)((a.G + b.G) / 2),
                (byte)((a.B + b.B) / 2));
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