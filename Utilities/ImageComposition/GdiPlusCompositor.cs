using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace MonoCross.Utilities.ImageComposition
{
    /// <summary>
    /// Represents an image compositor for platforms with access to System.Drawing.
    /// </summary>
    public class GdiPlusCompositor : ICompositor
    {
        /// <summary>
        /// Generates a composite image from the image files at the specified paths.
        /// </summary>
        /// <param name="paths">The fully-qualified paths of the images, in order of lowest z-index to highest, to layer together using alpha blending.</param>
        /// <param name="saveLocation">The fully-qualified path to save the composited image.</param>
        /// <remarks>This method assumes that all of the images passed in are the same size. If not, they are resized to fit to the first image.</remarks>
        /// <exception cref="FileNotFoundException">Thrown if any file in <paramref name="paths"/> could not be read.</exception>
        public void CreateCompositeImage(List<string> paths, string saveLocation)
        {
            CreateCompositeImage(paths, saveLocation, false);
        }

        /// <summary>
        /// Generates a composite image from the image files at the specified paths.
        /// </summary>
        /// <param name="paths">The fully-qualified paths of the images, in order of lowest z-index to highest, to layer together using alpha blending.</param>
        /// <param name="saveLocation">The fully-qualified path to save the composited image.</param>
        /// <param name="overwrite"><c>true</c> to overwrite an existing file; otherwise <c>false</c>.</param>
        /// <remarks>This method assumes that all of the images passed in are the same size. If not, they are resized to fit to the first image.</remarks>
        /// <exception cref="FileNotFoundException">Thrown if any file in <paramref name="paths"/> could not be read.</exception>
        public void CreateCompositeImage(List<string> paths, string saveLocation, bool overwrite)
        {
            if (!overwrite && Device.File.Exists(saveLocation))
                return;
            paths = paths.Where(path => path != null && Device.File.Exists(path)).ToList();
            if (paths.Count == 0) return;

            var metric = DateTime.UtcNow;
            Device.File.EnsureDirectoryExists(saveLocation);

            var images = new List<Bitmap>(paths.Count);

            foreach (var path in paths)
            {
                var bytes = Device.File.Read(path, EncryptionMode.NoEncryption);
                if (bytes == null)
                {
                    throw new FileNotFoundException(path);
                }

                var stream = new MemoryStream(bytes);
                images.Add(new Bitmap(stream));
                stream.Close();
                stream.Dispose();
            }

            if (images.Count == 0) return;

            using (var g = Graphics.FromImage(images[0]))
            {
                var destRect = new Rectangle(0, 0, images[0].Width, images[0].Height);
                for (int i = 1; i < images.Count; i++)
                {
                    g.DrawImage(images[i], destRect, new Rectangle(0, 0, images[i].Width, images[i].Height), GraphicsUnit.Pixel);
                    images[i].Dispose();
                }

                //Convert the icon into a stream so that it can be saved safely with the MonoCross file interface
                var save = new MemoryStream();
                images[0].Save(save, ImageFormat.Png);
                save.Position = 0;
                Device.File.Save(saveLocation, save, EncryptionMode.NoEncryption);

                save.Close();
                save.Dispose();
                images[0].Dispose();
            }

            Device.Log.Metric("ImageEngine icon creation", DateTime.UtcNow.Subtract(metric).TotalMilliseconds);
        }
    }
}