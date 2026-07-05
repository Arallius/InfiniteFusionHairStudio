using System.IO;
using System.IO.Compression;

namespace InfiniteFusionHairStudio.Core
{
    public static class Exporter
    {
        public static void Export(
            string exportsFolder,
            HairAsset replaceHair,
            ExportSlot slot1,
            ExportSlot slot2,
            ExportSlot slot3,
            ExportSlot slot4)
        {
            Directory.CreateDirectory(exportsFolder);

            string zipPath = Path.Combine(
                exportsFolder,
                $"{replaceHair.Name} Hair Pack.zip");

            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }

            using ZipArchive archive =
                ZipFile.Open(zipPath, ZipArchiveMode.Create);

            ExportSlot(archive, replaceHair, slot1, 1);
            ExportSlot(archive, replaceHair, slot2, 2);
            ExportSlot(archive, replaceHair, slot3, 3);
            ExportSlot(archive, replaceHair, slot4, 4);
        }

        private static void ExportSlot(
            ZipArchive archive,
            HairAsset replaceHair,
            ExportSlot slot,
            int slotNumber)
        {
            AddFile(
                archive,

                Path.Combine(
                    slot.Hair.FolderPath,
                    $"hair_{slotNumber}_{slot.Hair.FilePrefix}.png"),

                $"hair_{slotNumber}_{replaceHair.FilePrefix}.png");

            AddFile(
                archive,

                Path.Combine(
                    slot.Hair.FolderPath,
                    $"hair_trainer_{slotNumber}_{slot.Hair.FilePrefix}.png"),

                $"hair_trainer_{slotNumber}_{replaceHair.FilePrefix}.png");
        }

        private static void AddFile(
            ZipArchive archive,
            string sourceFile,
            string zipFileName)
        {
            var entry = archive.CreateEntry(
                zipFileName,
                CompressionLevel.Optimal);

            using var input =
                File.OpenRead(sourceFile);

            using var output =
                entry.Open();

            input.CopyTo(output);
        }
    }
}