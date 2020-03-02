using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MonoCross
{
    /// <summary>
    /// Represents an object that is responsible for caching image data within memory.
    /// </summary>
    public class ImageCache : IImageCache
    {
        /// <summary>
        /// Gets or sets the maximum number of images that are allowed to be cached at any given time.
        /// </summary>
        public int Capacity
        {
            get { return capacity; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Value cannot be less than zero.");
                }

                if (value < cache.Count)
                {
                    for (int i = value; i < cache.Count; i++)
                    {
                        lock (cacheLock)
                        {
                            cache.Remove(cache.Keys.FirstOrDefault());
                        }
                    }
                }

                capacity = value;
            }
        }
        private int capacity;

        /// <summary>
        /// Gets the number of images that are currently cached.
        /// </summary>
        public int Count
        {
            get { return cache.Count; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<string, CacheEntry> cache;
        private readonly object cacheLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoCross.ImageCache"/> class.
        /// </summary>
        public ImageCache()
        {
            cache = new Dictionary<string, CacheEntry>();
            capacity = 50;
        }

        /// <summary>
        /// Adds the specified image data to the cache using the specified file path as a key.
        /// Image data added with this method will never expire.
        /// </summary>
        /// <param name="filePath">The path to the file where the image data is stored.</param>
        /// <param name="imageData">The image data to be added to the cache.</param>
        public void Add(string filePath, IImageData imageData)
        {
            Add(filePath, imageData, null);
        }

        /// <summary>
        /// Adds the specified image data to the cache using the specified file path as a key.
        /// </summary>
        /// <param name="filePath">The path to the file where the image data is stored.</param>
        /// <param name="imageData">The image data to be added to the cache.</param>
        /// <param name="expirationDate">An optional point in time at which the image data should be considered invalid and removed from the cache.
        /// A value of <c>null</c> means that the data should never expire.</param>
        public void Add(string filePath, IImageData imageData, DateTime? expirationDate)
        {
            if (filePath != null && capacity > 0)
            {
                lock (cacheLock)
                {
                    cache[filePath] = new CacheEntry(imageData, expirationDate);
                    if (cache.Count > capacity)
                    {
                        cache.Remove(cache.Keys.FirstOrDefault(k => !k.Equals(filePath, StringComparison.OrdinalIgnoreCase)) ?? cache.Keys.FirstOrDefault());
                    }
                }
            }
        }

        /// <summary>
        /// Empties the cache of all images.
        /// </summary>
        public void Clear()
        {
            lock (cacheLock)
            {
                cache.Clear();
            }
        }

        /// <summary>
        /// Returns the image data associated with the specified file path.
        /// </summary>
        /// <param name="filePath">The file path of the image data to return.</param>
        /// <returns>The <see cref="IImageData"/> instance associated with the file path, or <c>null</c> if no instance is found.</returns>
        public IImageData Get(string filePath)
        {
            lock (cacheLock)
            {
                var entry = cache.GetValueOrDefault(filePath);
                if (entry.ExpirationDate != null && (DateTime.Now > entry.ExpirationDate.Value))
                {
                    cache.Remove(filePath);
                    return null;
                }
                return entry.Data;
            }
        }

        /// <summary>
        /// Removes from the cache the image data associated with the specified file path.
        /// </summary>
        /// <param name="filePath">The file path of the image data to remove.</param>
        public void Remove(string filePath)
        {
            lock (cacheLock)
            {
                cache.Remove(filePath);
            }
        }

        private struct CacheEntry
        {
            public IImageData Data;
            public DateTime? ExpirationDate;

            public CacheEntry(IImageData data, DateTime? expirationDate)
            {
                Data = data;
                ExpirationDate = expirationDate;
            }
        }
    }
}
