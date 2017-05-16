using System;

namespace MonoCross
{
    /// <summary>
    /// Defines an object that is responsible for caching image data within memory.
    /// </summary>
    public interface IImageCache
    {
        /// <summary>
        /// Gets or sets the maximum number of images that are allowed to be cached at any given time.
        /// </summary>
        int Capacity { get; set; }

        /// <summary>
        /// Gets the number of images that are currently cached.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds the specified image data to the cache using the specified file path as a key.
        /// Image data added with this method will never expire.
        /// </summary>
        /// <param name="filePath">The path to the file where the image data is stored.</param>
        /// <param name="imageData">The image data to be added to the cache.</param>
        void Add(string filePath, IImageData imageData);

        /// <summary>
        /// Adds the specified image data to the cache using the specified file path as a key.
        /// </summary>
        /// <param name="filePath">The path to the file where the image data is stored.</param>
        /// <param name="imageData">The image data to be added to the cache.</param>
        /// <param name="expirationDate">An optional point in time at which the image data should be considered invalid and removed from the cache.
        /// A value of <c>null</c> means that the data should never expire.</param>
        void Add(string filePath, IImageData imageData, DateTime? expirationDate);

        /// <summary>
        /// Empties the cache of all images.
        /// </summary>
        void Clear();

        /// <summary>
        /// Returns the image data associated with the specified file path.
        /// </summary>
        /// <param name="filePath">The file path of the image data to return.</param>
        /// <returns>The <see cref="IImageData"/> instance associated with the file path, or <c>null</c> if no instance is found.</returns>
        IImageData Get(string filePath);

        /// <summary>
        /// Removes from the cache the image data associated with the specified file path.
        /// </summary>
        /// <param name="filePath">The file path of the image data to remove.</param>
        void Remove(string filePath);
    }
}
