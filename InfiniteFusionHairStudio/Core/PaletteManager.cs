using System;
using System.Windows.Media.Imaging;

namespace InfiniteFusionHairStudio.Core
{
    public static class PaletteManager
    {
        public static BitmapSource Recolor(
            BitmapSource source,
            HairColor color,
            HairShade shade)
        {
            ArgumentNullException.ThrowIfNull(source);

            // Recoloring temporarily disabled while the
            // new palette engine is rebuilt.

            return source;
        }
    }
}