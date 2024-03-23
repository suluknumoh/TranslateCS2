using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace TranslateCS2.Helpers;

public static class ImageHelper {

    public static BitmapImage GetBitmapImage(Assembly assembly,
                                             string resourceName) {
        return GetBitmapImage(assembly.GetManifestResourceStream(resourceName));
    }

    public static BitmapImage GetBitmapImage(Bitmap bitmap) {
        using MemoryStream memoryStream = new MemoryStream();
        bitmap.Save(memoryStream, ImageFormat.Png);
        memoryStream.Position = 0;
        return GetBitmapImage(memoryStream);
    }

    public static BitmapImage GetBitmapImage(byte[] bytes) {
        using MemoryStream memoryStream = new MemoryStream(bytes);
        return GetBitmapImage(memoryStream);
    }

    public static BitmapImage GetBitmapImage(Stream? stream) {
        BitmapImage bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = stream;
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();
        //try {
        //    bitmapImage.Freeze();
        //} catch {

        //    // nix, wenns nicht geht, gehts nicht
        //}
        return bitmapImage;
    }
    public static void CreateSplashScreen(string pathToFolderWithSmallImages,
                                          string pathToLargeOverlayImage,
                                          string pathToSplashScreenDestinationImage) {
        int tileSize = 75;
        int width = 2400;
        int height = 1350;
        Bitmap splashScreen = new Bitmap(width,
                                         height,
                                         PixelFormat.Format32bppArgb);
        Colorize(splashScreen, Color.White);
        using (Graphics graphics = GetGraphics(splashScreen)) {
            DirectoryInfo directoryInfo = new DirectoryInfo(pathToFolderWithSmallImages);
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            IList<int> indizes = [];
            Random random = new Random();
            for (int x = 0; x < width; x += tileSize) {
                for (int y = 0; y < height; y += tileSize) {
                    int index = random.Next(0,
                                            fileInfos.Length);
                    while (indizes.Contains(index)) {
                        index = random.Next(0,
                                            fileInfos.Length);
                    }
                    indizes.Add(index);
                    Bitmap image = new Bitmap(fileInfos[index].FullName);
                    image = ResizeImage(image,
                                        tileSize,
                                        tileSize);
                    graphics.DrawImageUnscaled(image,
                                               x,
                                               y);
                    if (indizes.Count == fileInfos.Length) {
                        indizes.Clear();
                    }
                }
            }
        }
        splashScreen = ResizeImage(splashScreen,
                                   960,
                                   540);
        Bitmap overlay = new Bitmap(pathToLargeOverlayImage);
        overlay = MakeTransparant(overlay,
                                  Color.White);
        overlay = ResizeImage(overlay,
                              500,
                              500);
        using (Graphics graphics = GetGraphics(splashScreen)) {
            graphics.DrawImageUnscaled(overlay,
                                       230,
                                       20);
        }
        splashScreen.Save(pathToSplashScreenDestinationImage);
    }

    private static void Colorize(Bitmap bitmap, Color color) {
        for (int x = 0; x < bitmap.Width; x++) {
            for (int y = 0; y < bitmap.Height; y++) {
                bitmap.SetPixel(x, y, color);
            }
        }
    }

    public static Graphics GetGraphics(Bitmap image) {
        Graphics graphics = Graphics.FromImage(image);
        graphics.CompositingMode = CompositingMode.SourceOver;
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        return graphics;
    }

    public static Bitmap MakeTransparant(Bitmap bitmap,
                                         Color color) {
        Size bitmapSize = bitmap.Size;
        Bitmap ret = bitmap.Clone(new Rectangle(0,
                                                     0,
                                                     bitmapSize.Width,
                                                     bitmapSize.Height),
                                  PixelFormat.Format32bppArgb);
        ret.MakeTransparent(color);
        return ret;
    }
    public static Bitmap ResizeImage(Image image,
                                     int width,
                                     int height) {
        Rectangle destRect = new Rectangle(0,
                                           0,
                                           width,
                                           height);
        Bitmap destImage = new Bitmap(width,
                                      height);
        destImage.SetResolution(image.HorizontalResolution,
                                image.VerticalResolution);
        using Graphics graphics = GetGraphics(destImage);
        using ImageAttributes wrapMode = new ImageAttributes();
        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
        graphics.DrawImage(image,
                           destRect,
                           0,
                           0,
                           image.Width,
                           image.Height,
                           GraphicsUnit.Pixel,
                           wrapMode);

        return destImage;
    }
}