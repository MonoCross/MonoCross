using System;

namespace MonoCross
{
    /// <summary>
    /// Defines an object that contains image data.
    /// </summary>
    public interface IImageData
    {
        /// <summary>
        /// Returns an array of bytes that represent the individual pixel values of the image.
        /// </summary>
        byte[] GetBytes();

        /// <summary>
        /// Returns an <see cref="IExifData"/> object containing any available Exif data for the image.
        /// </summary>
        IExifData GetExifData();

        /// <summary>
        /// Saves the image data to the specified path on disk using the specified file format.
        /// </summary>
        /// <param name="filePath">The path at which to save the image data.</param>
        /// <param name="format">The file format in which to save the image data.</param>
        void Save(string filePath, ImageFileFormat format);
    }

    /// <summary>
    /// Describes the available file formats for image data.
    /// </summary>
    public enum ImageFileFormat : byte
    {
        /// <summary>
        /// The Joint Photographics Experts Group file format.
        /// </summary>
        JPEG,
        /// <summary>
        /// The Portable Network Graphics file format.
        /// </summary>
        PNG
    }
}
