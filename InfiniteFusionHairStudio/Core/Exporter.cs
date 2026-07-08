using System.IO;
using System.IO.Compression;
using System.Windows.Media.Imaging;

namespace InfiniteFusionHairStudio.Core
{
    public static class Exporter
    {
        public static void Export(
            string exportsFolder,
            HairAsset replaceHair,
            ExportSlot[] slots)
        {
            Directory.CreateDirectory(exportsFolder);

            string zipPath = Path.Combine(
                exportsFolder,
                $"{replaceHair.Name} Hair Pack.zip");

            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }

            SpriteComposer composer = new();

            using ZipArchive archive =
                ZipFile.Open(zipPath, ZipArchiveMode.Create);

            for (int i = 0; i < slots.Length; i++)
            {
                ExportSlot(
                    archive,
                    composer,
                    replaceHair,
                    slots[i],
                    i + 1);
            }
        }

        private static void ExportSlot(
            ZipArchive archive,
            SpriteComposer composer,
            HairAsset replaceHair,
            ExportSlot slot,
            int slotNumber)
        {
            BitmapSource overworld =
                composer.ComposeOverworld(slot);

            BitmapSource trainer =
                composer.ComposeTrainer(slot);

            AddBitmap(
                archive,
                overworld,
                $"hair_{slotNumber}_{replaceHair.FilePrefix}.png");

            AddBitmap(
                archive,
                trainer,
                $"hair_trainer_{slotNumber}_{replaceHair.FilePrefix}.png");
        }

        private static void AddBitmap(
            ZipArchive archive,
            BitmapSource bitmap,
            string zipFileName)
        {
            var entry = archive.CreateEntry(
                zipFileName,
                CompressionLevel.Optimal);

            PngBitmapEncoder encoder = new();

            encoder.Frames.Add(
                BitmapFrame.Create(bitmap));

            using MemoryStream memory = new();

            encoder.Save(memory);

            memory.Position = 0;

            using Stream output =
                entry.Open();

            memory.CopyTo(output);
        }
    }
}