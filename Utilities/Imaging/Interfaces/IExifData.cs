using System;
using System.Collections.Generic;

namespace MonoCross
{
    /// <summary>
    /// Defines an object that contains Exif data for an image.
    /// </summary>
    public interface IExifData
    {
        /// <summary>
        /// Gets the aperture value of the lens when the image was taken.
        /// </summary>
        double Aperture { get; }

        /// <summary>
        /// Gets a value indicating the color space of the image.
        /// </summary>
        int ColorSpace { get; }

        /// <summary>
        /// Gets the date and time when the image was last changed.
        /// </summary>
        DateTime DateTime { get; }

        /// <summary>
        /// Gets the date and time when the image was stored as digital data.
        /// </summary>
        DateTime DateTimeDigitized { get; }

        /// <summary>
        /// Gets the date and time when the image was originally generated.
        /// </summary>
        DateTime DateTimeOriginal { get; }

        /// <summary>
        /// Gets the number of dots per inch on the Y axis.
        /// </summary>
        double DPIHeight { get; }

        /// <summary>
        /// Gets the number of dots per inch on the X axis.
        /// </summary>
        double DPIWidth { get; }

        /// <summary>
        /// Gets the exposure program that the camera used when the image was taken.
        /// </summary>
        string ExposureProgram { get; }

        /// <summary>
        /// Gets the exposure time, in seconds.
        /// </summary>
        double ExposureTime { get; }

        /// <summary>
        /// Gets a flag value indicating the type of flash that was used.
        /// </summary>
        int Flash { get; }

        /// <summary>
        /// Gets the F-number of the lens when the image was taken.
        /// </summary>
        double FNumber { get; }

        /// <summary>
        /// Gets the focal length of the lens when the image was taken, in millimeters.
        /// </summary>
        double FocalLength { get; }

        /// <summary>
        /// Gets the manufacturer of the camera that captured the image.
        /// </summary>
        string Manufacturer { get; }

        /// <summary>
        /// Gets the model of the camera that captured the image.
        /// </summary>
        string Model { get; }

        /// <summary>
        /// Gets a value indicating the orientation of the camera relative to the captured scene.
        /// </summary>
        int Orientation { get; }

        /// <summary>
        /// Gets the number of pixels on the Y axis.
        /// </summary>
        double PixelHeight { get; }

        /// <summary>
        /// Gets the number of pixels on the X axis.
        /// </summary>
        double PixelWidth { get; }

        /// <summary>
        /// Gets the shutter speed of the camera when the image was taken.
        /// </summary>
        double ShutterSpeed { get; }

        /// <summary>
        /// Gets the resolution of the image on the X axis.
        /// </summary>
        double XResolution { get; }

        /// <summary>
        /// Gets the resolution of the image on the Y axis.
        /// </summary>
        double YResolution { get; }

        /// <summary>
        /// Returns a collection containing all of the available Exif data in an unformatted and unparsed form.
        /// </summary>
        IDictionary<string, object> GetRawData();
    }
}

