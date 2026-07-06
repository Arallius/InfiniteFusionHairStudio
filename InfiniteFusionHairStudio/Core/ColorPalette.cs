using System.Windows.Media;

namespace InfiniteFusionHairStudio.Core
{
    public class ColorPalette
    {
        public Color Dark { get; }

        public Color Mid { get; }

        public Color Light { get; }

        public Color Highlight { get; }

        public ColorPalette(
            Color dark,
            Color mid,
            Color light,
            Color highlight)
        {
            Dark = dark;
            Mid = mid;
            Light = light;
            Highlight = highlight;
        }
    }
}