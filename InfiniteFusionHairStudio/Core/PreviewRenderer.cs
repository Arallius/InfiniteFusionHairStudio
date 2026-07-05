using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace InfiniteFusionHairStudio.Core
{
    public class PreviewRenderer
    {
        private readonly Image _bodyImage;
        private readonly Image _hairImage;

        public PreviewRenderer(Image bodyImage, Image hairImage)
        {
            _bodyImage = bodyImage;
            _hairImage = hairImage;
        }

        public void SetBody(string imagePath)
        {
            if (!File.Exists(imagePath))
                return;

            _bodyImage.Source = LoadBitmap(imagePath);
        }

        public void SetHair(string imagePath)
        {
            if (!File.Exists(imagePath))
                return;

            _hairImage.Source = LoadBitmap(imagePath);
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