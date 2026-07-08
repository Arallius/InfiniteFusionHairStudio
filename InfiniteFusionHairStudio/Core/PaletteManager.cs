using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InfiniteFusionHairStudio.Core
{
    public static class PaletteManager
    {
        private static readonly Color SourceDark =
            Color.FromRgb(143, 92, 63);

        private static readonly Color SourceMid =
            Color.FromRgb(214, 167, 118);

        private static readonly Color SourceLight =
            Color.FromRgb(230, 199, 147);

        private static readonly Color SourceHighlight =
            Color.FromRgb(245, 226, 142);

        public static BitmapSource Recolor(
            BitmapSource source,
            HairColor color,
            HairShade shade)
        {
            ArgumentNullException.ThrowIfNull(source);

            ColorPalette target =
                PaletteDatabase.GetPalette(
                    color,
                    shade);

            BitmapSource normalized =
                new FormatConvertedBitmap(
                    source,
                    PixelFormats.Bgra32,
                    null,
                    0);
            System.Diagnostics.Debug.WriteLine(
    $"Source: {source.PixelWidth}x{source.PixelHeight}");

            System.Diagnostics.Debug.WriteLine(
                $"Normalized: {normalized.PixelWidth}x{normalized.PixelHeight}");

            WriteableBitmap bitmap =
                new WriteableBitmap(normalized);

            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            int stride = width * 4;

            byte[] pixels =
                new byte[height * stride];

            bitmap.CopyPixels(
                pixels,
                stride,
                0);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                byte blue = pixels[i];
                byte green = pixels[i + 1];
                byte red = pixels[i + 2];
                byte alpha = pixels[i + 3];

                if (alpha == 0)
                    continue;

                Color replacement;

                if (red == SourceDark.R &&
                    green == SourceDark.G &&
                    blue == SourceDark.B)
                {
                    replacement = target.Dark;
                }
                else if (red == SourceMid.R &&
                         green == SourceMid.G &&
                         blue == SourceMid.B)
                {
                    replacement = target.Mid;
                }
                else if (red == SourceLight.R &&
                         green == SourceLight.G &&
                         blue == SourceLight.B)
                {
                    replacement = target.Light;
                }
                else if (red == SourceHighlight.R &&
                         green == SourceHighlight.G &&
                         blue == SourceHighlight.B)
                {
                    replacement = target.Highlight;
                }
                else
                {
                    continue;
                }

                pixels[i] = replacement.B;
                pixels[i + 1] = replacement.G;
                pixels[i + 2] = replacement.R;
                pixels[i + 3] = alpha;
            }

            WriteableBitmap result =
                new WriteableBitmap(
                    width,
                    height,
                    normalized.DpiX,
                    normalized.DpiY,
                    PixelFormats.Bgra32,
                    null);

            result.WritePixels(
                new System.Windows.Int32Rect(
                    0,
                    0,
                    width,
                    height),
                pixels,
                stride,
                0);

            result.Freeze();

            return result;
        }
    }
}