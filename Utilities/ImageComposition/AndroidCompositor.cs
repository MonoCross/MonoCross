using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace MonoCross.Utilities.ImageComposition
{
    /// <summary>
    /// An ICompositor for platforms with access to Android.Graphics.
    /// </summary>
    public class AndroidCompositor : ICompositor
    {
        public void CreateCompositeImage(List<string> paths, string saveLocation, bool overwrite)
        {
            if (!overwrite && Device.File.Exists(saveLocation) || paths.Count == 0)
                return;

            var bytes = paths.Select(path =>
            {
                var b = Device.File.Read(path, EncryptionMode.NoEncryption);
                if (b == null)
                {
                    throw new FileNotFoundException("Image not found for composition", path) { Data = { { "FilePath", path }, }, };
                }
                return b;
            }).ToList();
            var image = CreateCompositeImage(bytes, "png");
            if (image == null) return;
            Device.File.EnsureDirectoryExists(saveLocation);
            Device.File.Save(saveLocation, image, EncryptionMode.NoEncryption);
        }

        public void CreateCompositeImage(List<string> paths, string saveLocation)
        {
            CreateCompositeImage(paths, saveLocation, false);
        }

        public byte[] CreateCompositeImage(List<byte[]> imageBytes, string extension)
        {
            var metric = DateTime.UtcNow;
            imageBytes = imageBytes.Where(bytes => bytes.Length != 0).ToList();
            var images = new List<BitmapDrawable>(imageBytes.Select(bytes => new BitmapDrawable(BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length))).Where(image => image != null));

            if (images.Count == 0) return null;

            var b = images[0].Bitmap.Copy(images[0].Bitmap.GetConfig(), true);
            images[0].Dispose();
            var canvas = new Canvas(b);

            var width = b.Width;
            var height = b.Height;

            for (var i = 1; i < images.Count; i++)
            {
                var image = images[i];
                canvas.DrawBitmap(image.Bitmap, null, new RectF(0, 0, width, height), image.Paint);
                image.Dispose();
            }

            Device.Log.Metric("ImageEngine icon creation", DateTime.UtcNow.Subtract(metric).TotalMilliseconds);
            return EncodeBitmap(b, DecodeFormat(extension));
        }

        public byte[] Resize(byte[] image, string extension, int maxWidth, int maxHeight)
        {
            //Decode image size
            var options = new BitmapFactory.Options { InJustDecodeBounds = true, };
            BitmapFactory.DecodeByteArray(image, 0, image.Length, options);

            //Find the correct scale value. It should be a power of 2 for efficiency.
            var scale = 1;
            while (options.OutWidth / scale / 2 >= maxWidth && options.OutHeight / scale / 2 >= maxHeight)
                scale *= 2;

            return EncodeBitmap(BitmapFactory.DecodeByteArray(image, 0, image.Length, new BitmapFactory.Options { InSampleSize = scale, }), DecodeFormat(extension));
        }

        private byte[] EncodeBitmap(Bitmap b, ImageFileFormat format)
        {
            if (b == null) return null;
            var save = new MemoryStream();
            switch (format)
            {
                case ImageFileFormat.JPEG:
                    b.Compress(Bitmap.CompressFormat.Jpeg, 100, save);
                    break;
                default:
                    b.Compress(Bitmap.CompressFormat.Png, 100, save);
                    break;
            }
            save.Position = 0;
            return save.GetBuffer();
        }

        private ImageFileFormat DecodeFormat(string extension)
        {
            extension = extension.ToLowerInvariant();
            if (extension.EndsWith("jpg") || extension.EndsWith("jpeg"))
            {
                return ImageFileFormat.JPEG;
            }
            return ImageFileFormat.PNG;
        }
    }
}