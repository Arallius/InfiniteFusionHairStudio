using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InfiniteFusionHairStudio.Core
{
    public class SpriteComposer
    {
        public BitmapSource? ComposeBack(
            ExportSlot slot,
            bool useTrainerAssets = true)
        {
            return ComposeSingleLayer(
                slot.Back,
                slot.BackColor,
                slot.BackShade,
                useTrainerAssets);
        }

        public BitmapSource? ComposeBase(
            ExportSlot slot,
            bool useTrainerAssets = true)
        {
            return ComposeSingleLayer(
                slot.Base,
                slot.BaseColor,
                slot.BaseShade,
                useTrainerAssets);
        }

        public BitmapSource? ComposeBangs(
            ExportSlot slot,
            bool useTrainerAssets = true)
        {
            return ComposeSingleLayer(
                slot.Bangs,
                slot.BangsColor,
                slot.BangsShade,
                useTrainerAssets);
        }

        public BitmapSource ComposeTrainer(ExportSlot slot)
        {
            return Compose(
                slot,
                true);
        }

        public BitmapSource ComposeOverworld(ExportSlot slot)
        {
            BitmapSource? referenceImage =
                ComposeBack(slot, false)
                ?? ComposeBase(slot, false)
                ?? ComposeBangs(slot, false);

            int width = referenceImage?.PixelWidth ?? 160;
            int height = referenceImage?.PixelHeight ?? 160;

            int splitY = height / 2;

            DrawingVisual visual = new();
            DrawingContext context = visual.RenderOpen();

            // Top half
            DrawLayerSection(
                context,
                slot.Back,
                slot.BackColor,
                slot.BackShade,
                false,
                width,
                height,
                0,
                splitY);

            DrawLayerSection(
                context,
                slot.Base,
                slot.BaseColor,
                slot.BaseShade,
                false,
                width,
                height,
                0,
                splitY);

            DrawLayerSection(
                context,
                slot.Bangs,
                slot.BangsColor,
                slot.BangsShade,
                false,
                width,
                height,
                0,
                splitY);

            // Bottom half
            DrawLayerSection(
                context,
                slot.Bangs,
                slot.BangsColor,
                slot.BangsShade,
                false,
                width,
                height,
                splitY,
                splitY);

            DrawLayerSection(
                context,
                slot.Base,
                slot.BaseColor,
                slot.BaseShade,
                false,
                width,
                height,
                splitY,
                splitY);

            DrawLayerSection(
                context,
                slot.Back,
                slot.BackColor,
                slot.BackShade,
                false,
                width,
                height,
                splitY,
                splitY);

            context.Close();

            RenderTargetBitmap bitmap = new(
                width,
                height,
                96,
                96,
                PixelFormats.Pbgra32);

            bitmap.Render(visual);
            bitmap.Freeze();

            return bitmap;
        }

        private BitmapSource Compose(
            ExportSlot slot,
            bool useTrainerAssets)
        {
            BitmapSource? referenceImage =
                ComposeBack(slot, useTrainerAssets)
                ?? ComposeBase(slot, useTrainerAssets)
                ?? ComposeBangs(slot, useTrainerAssets);

            int width = referenceImage?.PixelWidth ?? 160;
            int height = referenceImage?.PixelHeight ?? 160;

            DrawingVisual visual = new();
            DrawingContext context = visual.RenderOpen();

            DrawLayer(
                context,
                slot.Back,
                slot.BackColor,
                slot.BackShade,
                useTrainerAssets,
                width,
                height);

            DrawLayer(
                context,
                slot.Base,
                slot.BaseColor,
                slot.BaseShade,
                useTrainerAssets,
                width,
                height);

            DrawLayer(
                context,
                slot.Bangs,
                slot.BangsColor,
                slot.BangsShade,
                useTrainerAssets,
                width,
                height);

            context.Close();

            RenderTargetBitmap bitmap = new(
                width,
                height,
                96,
                96,
                PixelFormats.Pbgra32);

            bitmap.Render(visual);
            bitmap.Freeze();

            return bitmap;
        }

        private BitmapSource? ComposeSingleLayer(
            HairPart? part,
            HairColor color,
            HairShade shade,
            bool useTrainerAssets)
        {
            if (part == null)
                return null;

            string imagePath = useTrainerAssets
                ? part.TrainerImagePath
                : part.OverworldImagePath;

            if (!File.Exists(imagePath))
                return null;

            BitmapImage image = LoadBitmap(imagePath);

            return PaletteManager.Recolor(
                image,
                color,
                shade);
        }

        private static void DrawLayer(
            DrawingContext context,
            HairPart? part,
            HairColor color,
            HairShade shade,
            bool useTrainerAssets,
            int canvasWidth,
            int canvasHeight)
        {
            BitmapSource? bitmap = ComposeLayer(
                part,
                color,
                shade,
                useTrainerAssets);

            if (bitmap == null)
                return;

            context.DrawImage(
                bitmap,
                new Rect(
                    0,
                    0,
                    canvasWidth,
                    canvasHeight));
        }

        private static void DrawLayerSection(
            DrawingContext context,
            HairPart? part,
            HairColor color,
            HairShade shade,
            bool useTrainerAssets,
            int canvasWidth,
            int canvasHeight,
            int sourceY,
            int sectionHeight)
        {
            BitmapSource? bitmap = ComposeLayer(
                part,
                color,
                shade,
                useTrainerAssets);

            if (bitmap == null)
                return;

            CroppedBitmap cropped = new(
                bitmap,
                new Int32Rect(
                    0,
                    sourceY,
                    bitmap.PixelWidth,
                    sectionHeight));

            context.DrawImage(
                cropped,
                new Rect(
                    0,
                    sourceY,
                    canvasWidth,
                    sectionHeight));
        }

        private static BitmapSource? ComposeLayer(
            HairPart? part,
            HairColor color,
            HairShade shade,
            bool useTrainerAssets)
        {
            if (part == null)
                return null;

            string imagePath = useTrainerAssets
                ? part.TrainerImagePath
                : part.OverworldImagePath;

            if (!File.Exists(imagePath))
                return null;

            BitmapImage image = LoadBitmap(imagePath);

            return PaletteManager.Recolor(
                image,
                color,
                shade);
        }

        private static BitmapImage LoadBitmap(string path)
        {
            BitmapImage bitmap = new();

            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(path);
            bitmap.EndInit();

            bitmap.Freeze();

            return bitmap;
        }
    }
}