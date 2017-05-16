using System.Collections.Generic;

namespace MonoCross.Utilities.ImageComposition
{
    /// <summary>
    /// Defines an image compositor.
    /// </summary>
    public interface ICompositor
    {
        /// <summary>
        /// Generates a composite image from the image files at the specified paths.
        /// </summary>
        /// <param name="paths">The fully-qualified paths of the images, in order of lowest z-index to highest, to layer together using alpha blending.</param>
        /// <param name="saveLocation">The fully-qualified path to save the composited image. If the file already exists, it will not be overwritten.</param>
        /// <remarks>This method assumes that all of the images passed in are the same size. If not, they are resized to fit to the first image.</remarks>
        void CreateCompositeImage(List<string> paths, string saveLocation);

        /// <summary>
        /// Generates a composite image from the image files at the specified paths.
        /// </summary>
        /// <param name="paths">The fully-qualified paths of the images, in order of lowest z-index to highest, to layer together using alpha blending.</param>
        /// <param name="saveLocation">The fully-qualified path to save the composited image.</param>
        /// <param name="overwrite"><c>true</c> to overwrite an existing file; otherwise <c>false</c>.</param>
        /// <remarks>This method assumes that all of the images passed in are the same size. If not, they are resized to fit to the first image.</remarks>
        void CreateCompositeImage(List<string> paths, string saveLocation, bool overwrite);

        //If anyone complains about the above method's resizing behavior, implement this:
        //void CreateCompositeImage(List<string> paths, string saveLocation, ResizeMode mode);
        
        //TODO: These aren't quite ready for prime-time on other platforms.
#if DROID
        /// <summary>
        /// Creates a composited image from a list of source images.
        /// </summary>
        /// <param name="imageBytes">The bytes of the images, in order of lowest z-index to highest, to layer together using alpha blending.</param>
        /// <param name="extension">The file extension of the image that matches the desired encoding.</param>
        /// <returns>A byte array that contains the composited image.</returns>
        ///// <remarks>This method assumes that all of the images passed in are the same size. If not, they are resized to fit to the first image .</remarks>
        byte[] CreateCompositeImage(List<byte[]> imageBytes, string extension);

        /// <summary>
        /// Resizes an image to fit inside a boundary while maintaining aspect ratio.
        /// </summary>
        /// <param name="image">The image as a byte array.</param>
        /// <param name="extension">The file extension of the image that matches the desired encoding.</param>
        /// <param name="maxWidth">The maximum width of the resized image.</param>
        /// <param name="maxHeight">The maximum height of the resized image.</param>
        /// <returns>The resized image as a byte array.</returns>
        byte[] Resize(byte[] image, string extension, int maxWidth, int maxHeight);
#endif
    }

    //public enum ResizeMode
    //{
    //    SizeToFit,
    //    StretchHorizontal,
    //    StretchVertical,
    //    None,
    //}
}
