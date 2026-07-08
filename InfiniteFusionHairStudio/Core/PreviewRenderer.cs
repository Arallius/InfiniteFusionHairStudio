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

        public void SetBack(BitmapSource bitmap)
        {
            _backImage.Source = bitmap;
        }

        public void SetBase(BitmapSource bitmap)
        {
            _baseImage.Source = bitmap;
        }

        public void SetBangs(BitmapSource bitmap)
        {
            _bangsImage.Source = bitmap;
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