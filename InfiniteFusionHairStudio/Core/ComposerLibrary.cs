using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace InfiniteFusionHairStudio.Core
{
    public class ComposerLibrary
    {
        public List<HairPart> Bangs { get; } = new();

        public List<HairPart> Base { get; } = new();

        public List<HairPart> Back { get; } = new();

        public ComposerLibrary(string composerFolder)
        {
            LoadCategory(
                Path.Combine(composerFolder, "Bangs"),
                HairPartType.Bangs,
                Bangs);

            LoadCategory(
                Path.Combine(composerFolder, "Base"),
                HairPartType.Base,
                Base);

            LoadCategory(
                Path.Combine(composerFolder, "Back"),
                HairPartType.Back,
                Back);

        }

        private static void LoadCategory(
            string categoryFolder,
            HairPartType type,
            List<HairPart> destination)
        {
            if (!Directory.Exists(categoryFolder))
                return;

            foreach (string folder in Directory.GetDirectories(categoryFolder).OrderBy(f => f))
            {
                destination.Add(new HairPart
                {
                    Name = Path.GetFileName(folder),
                    FolderPath = folder,
                    Type = type
                });
            }
        }
    }
}