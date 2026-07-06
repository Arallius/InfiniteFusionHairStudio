using System.IO;

namespace InfiniteFusionHairStudio.Core
{
    public class HairPart
    {
        public string Name { get; set; } = "";

        public HairPartType Type { get; set; }

        public string FolderPath { get; set; } = "";

        public string TrainerImagePath =>
            Path.Combine(FolderPath, "trainer.png");

        public string OverworldImagePath =>
            Path.Combine(FolderPath, "overworld.png");
    }
}