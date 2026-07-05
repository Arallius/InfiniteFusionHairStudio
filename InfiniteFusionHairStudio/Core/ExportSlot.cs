namespace InfiniteFusionHairStudio.Core
{
    public class ExportSlot
    {
        public HairAsset Hair { get; set; } = new HairAsset();

        // This will be used for recoloring later.
        public int PaletteIndex { get; set; }
    }
}