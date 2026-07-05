namespace InfiniteFusionHairStudio.Core
{
    public class HairAsset
    {
        public string Name { get; set; } = "";

        public string FolderPath { get; set; } = "";

        // Used when building filenames like:
        // hair_1_bugsy.png
        public string FilePrefix { get; set; } = "";
    }
}