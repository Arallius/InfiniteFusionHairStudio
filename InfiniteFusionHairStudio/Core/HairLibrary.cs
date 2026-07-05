using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InfiniteFusionHairStudio.Core
{
    public class HairLibrary
    {
        public List<HairAsset> Hairstyles { get; } = new();

        public HairLibrary(string libraryPath)
        {
            if (!Directory.Exists(libraryPath))
                return;

            var folders = Directory.GetDirectories(libraryPath);

            foreach (var folder in folders.OrderBy(f => f))
            {
                string folderName = Path.GetFileName(folder);

                Hairstyles.Add(new HairAsset
                {
                    Name = folderName,
                    FolderPath = folder,
                    FilePrefix = folderName
                });
            }
        }
    }
}