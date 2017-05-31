using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using CoreGraphics;
using Foundation;
using UIKit;

namespace MonoCross.Utilities.ImageComposition
{
    public class TouchCompositor : ICompositor
    {
        private static readonly object padlock = new object();

        /// <summary>
        /// Creates a composited image from a list of source images
        /// </summary>
        /// <param name="paths">The paths of the images, in order of lowest z-index to highest, to composite together</param>
        /// <param name="saveLocation">Where to save the composited image</param>
        public void CreateCompositeImage(List<string> paths, string saveLocation)
        {
            CreateCompositeImage(paths, saveLocation, false);
        }

        /// <summary>
        /// Creates a composited image from a list of source images
        /// </summary>
        /// <param name="paths">The paths of the images, in order of lowest z-index to highest, to composite together</param>
        /// <param name="saveLocation">Where to save the composited image</param>
        /// <param name="overwrite"><c>true</c> to overwrite an existing file; otherwise <c>false</c>.</param>
        public void CreateCompositeImage(List<string> paths, string saveLocation, bool overwrite)
        {
            if (!overwrite && Device.File.Exists(saveLocation))
                return;
            
            paths = paths.Where(path => path != null && Device.File.Exists(path)).ToList();
            if (paths.Count == 0)
                return;

            try
            {
                var metric = System.DateTime.UtcNow;
                Device.File.EnsureDirectoryExists(saveLocation);
                using (new NSAutoreleasePool())
                {
                    var images = new List<CGImage>(paths.Count);
                    
                    images.AddRange(paths.Select<string, CGImage>(path => path.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase) ?
                    	CGImage.FromPNG(CGDataProvider.FromFile(path), null, false, CGColorRenderingIntent.Default) :
                    	CGImage.FromJPEG(CGDataProvider.FromFile(path), null, false, CGColorRenderingIntent.Default)));
                    	
                    nint width = images[0].Width;
                    nint height = images[0].Height;

                    var bounds = new RectangleF(0, 0, width, height);
                    CGBitmapContext g = new CGBitmapContext(
                        System.IntPtr.Zero,
                        width,
                        height,
                        images[0].BitsPerComponent,
                        images[0].Width * 4,
                        CGColorSpace.CreateDeviceRGB(),
                        CGImageAlphaInfo.PremultipliedLast
                    );

                    foreach (var cgImage in images)
                    {
                        g.DrawImage(bounds, cgImage);
                    }

                    lock (padlock)
                    {
                        // UIImage.AsPNG() should be safe to run on a background thread, but MT 6.2.6.6 says otherwise.
                        // Xamarin confirmed that this was unintentional and that MT 6.2.7 will remove the UI check.
                        UIApplication.CheckForIllegalCrossThreadCalls = false;
                        NSError err = null;
                        UIImage.FromImage(g.ToImage()).AsPNG().Save(saveLocation, true, out err);
                        UIApplication.CheckForIllegalCrossThreadCalls = true;
                    }
                }
                Device.Log.Metric("ImageEngine icon creation", System.DateTime.UtcNow.Subtract(metric).TotalMilliseconds);
            }
            catch (Exception e)
            {
                Device.Log.Error("An error occurred while compositing the image", e);
            }
        }

        public UIImage CreateCompositeImage(params UIImage[] uiImages)
        {
            if (uiImages == null || uiImages.Count() < 1)
            {
                return null;
            }

            try
            {
                using (new NSAutoreleasePool())
                {
                    var cgImages = uiImages.Where(uiImage => null != uiImage).Select(uiImage => uiImage.CGImage).ToList();

                    if (cgImages.Count() < 1)
                    {
                        return null;
                    }

                    var firstImage = cgImages[0];
                
                    // note that bytes per row should be based on width, not height.
                    nint bytesPerRow = firstImage.Width * 4;
                    byte[] ctxBuffer = new byte[bytesPerRow * firstImage.Height];

                    var bounds = new RectangleF(0, 0, firstImage.Width, firstImage.Height);
                    CGBitmapContext g = new CGBitmapContext(
                        ctxBuffer,
                        firstImage.Width,
                        firstImage.Height,
                        firstImage.BitsPerComponent,
                        bytesPerRow,
                        CGColorSpace.CreateDeviceRGB(),
                        CGImageAlphaInfo.PremultipliedLast
                    );

                    foreach (var cgImage in cgImages)
                    {
                        g.DrawImage(bounds, cgImage);
                    }

                    return UIImage.FromImage(g.ToImage());
                }
            }
            catch (Exception e)
            {
                Device.Log.Error("An error occurred while compositing the image", e);
                return null;
            }
        }
    }
}