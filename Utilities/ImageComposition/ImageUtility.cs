using System;

namespace MonoCross.Utilities
{
    /// <summary>
    /// Represents a utility class with methods for image stream manipulation.
    /// </summary>
    public static class ImageUtility
    {
        /// <summary>
        /// Converts the specified Base64 string to a byte array containing an image.
        /// </summary>
        /// <param name="imageData">The string to convert back into an image.</param>
        public static byte[] DecodeImage(string imageData)
        {
            return string.IsNullOrEmpty(imageData) ? null : Convert.FromBase64String(imageData);
        }

        /// <summary>
        /// Converts the specified byte array representing an image into a data URI.
        /// </summary>
        /// <param name="image">The bytes of the image to convert.</param>
        /// <param name="extension">The extension of the image file.</param>
        /// <returns>A data URI representing the image.</returns>
        public static string EncodeImageToDataUri(byte[] image, string extension)
        {
            return image == null || image.Length == 0 ? null : string.Format("data:image/{0};base64,{1}", extension.Trim(new[] { '.' }), Convert.ToBase64String(image));
        }

        /// <summary>
        /// Converts the specified data URI into a byte array representing an image.
        /// </summary>
        /// <param name="imageData">The data URI to convert.</param>
        /// <param name="extension">The extension of the resulting image.</param>
        /// <returns>A byte array representing the image.</returns>
        public static byte[] DecodeImageFromDataUri(string imageData, out string extension)
        {
            extension = null;
            if (string.IsNullOrEmpty(imageData)) return null;

            const string location = ";base64,";
            extension = imageData.Substring(imageData.IndexOf('/') + 1);
            int i = extension.IndexOf(location, StringComparison.Ordinal);
            extension = extension.Remove(i, extension.Length - i);

            return DecodeImage(imageData.Remove(0, imageData.IndexOf(',') + 1));
        }

        /// <summary>
        /// Converts the specified Base64 string to a byte array.
        /// </summary>
        /// <param name="s">The Base64 string to convert.</param>
        /// <returns>The decoded byte array.</returns>
        public static byte[] FromBase64String(string s)
        {
            return Convert.FromBase64String(s);
        }

        /// <summary>
        /// Converts the specified byte array representing an image into a Base64 and UrlEncoded string.
        /// </summary>
        /// <param name="image">The bytes of the image to convert.</param>
        /// <returns>The encoded string.</returns>
        public static string EncodeImage(byte[] image)
        {
            return Convert.ToBase64String(image);
        }

    }
}
