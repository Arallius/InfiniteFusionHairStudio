namespace InfiniteFusionHairStudio.Core
{
    public class ExportSlot
    {
        public HairAsset Hair { get; set; } = new HairAsset();

        // Individual composer selections
        public HairPart? Bangs { get; set; }
        public HairPart? Base { get; set; }
        public HairPart? Back { get; set; }

        // Individual colors
        public HairColor BangsColor { get; set; } = HairColor.Brown;
        public HairColor BaseColor { get; set; } = HairColor.Brown;
        public HairColor BackColor { get; set; } = HairColor.Brown;

        // Individual shades
        public HairShade BangsShade { get; set; } = HairShade.Normal;
        public HairShade BaseShade { get; set; } = HairShade.Normal;
        public HairShade BackShade { get; set; } = HairShade.Normal;

        // Reserved for export compatibility / future use.
        public int PaletteIndex { get; set; }
    }
}