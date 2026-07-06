using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace InfiniteFusionHairStudio.Core
{
    public class PreviewRenderer
    {
        private readonly Image _backImage;
        private readonly Image _bodyImage;
        private readonly Image _baseImage;
        private readonly Image _bangsImage;

        public PreviewRenderer(
            Image backImage,
            Image bodyImage,
            Image baseImage,
            Image bangsImage)
        {
            _backImage = backImage;
            _bodyImage = bodyImage;
            _baseImage = baseImage;
            _bangsImage = bangsImage;
        }

        public void SetBody(string imagePath)
        {
            if (!File.Exists(imagePath))
                return;

            _bodyImage.Source = LoadBitmap(imagePath);
        }

        public void SetBack(
            string imagePath,
            HairColor color,
            HairShade shade)
        {
            if (!File.Exists(imagePath))
                return;

            _backImage.Source = PaletteManager.Recolor(
                LoadBitmap(imagePath),
                color,
                shade);
        }

        public void SetBase(
            string imagePath,
            HairColor color,
            HairShade shade)
        {
            if (!File.Exists(imagePath))
                return;

            _baseImage.Source = PaletteManager.Recolor(
                LoadBitmap(imagePath),
                color,
                shade);
        }

        public void SetBangs(
            string imagePath,
            HairColor color,
            HairShade shade)
        {
            if (!File.Exists(imagePath))
                return;

            _bangsImage.Source = PaletteManager.Recolor(
                LoadBitmap(imagePath),
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