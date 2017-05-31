using System.Collections.Generic;

namespace MonoCross.Utilities.ImageComposition
{
    /// <summary>
    /// Represents an image compositor with no implementation.  This is compatible with all platforms and targets, and it is useful
    /// for when a concrete class is required but no implementation is necessary.
    /// </summary>
    public class NullCompositor : ICompositor
    {
        /// <summary>
        /// Generates a composite image from the image files at the specified paths.
        /// Note that this does not actually do anything in a <see cref="NullCompositor"/> instance.
        /// </summary>
        /// <param name="paths">The fully-qualified paths of the images, in order of lowest z-index to highest, to layer together using alpha blending.</param>
        /// <param name="saveLocation">The fully-qualified path to save the composited image.</param>
        public void CreateCompositeImage(List<string> paths, string saveLocation)
        {
            //Do nothing. 
        }

        /// <summary>
        /// Generates a composite image from the image files at the specified paths.
        /// Note that this does not actually do anything in a <see cref="NullCompositor"/> instance.
        /// </summary>
        /// <param name="paths">The fully-qualified paths of the images, in order of lowest z-index to highest, to layer together using alpha blending.</param>
        /// <param name="saveLocation">The fully-qualified path to save the composited image.</param>
        /// <param name="overwrite"><c>true</c> to overwrite an existing file; otherwise <c>false</c>.</param>
        public void CreateCompositeImage(List<string> paths, string saveLocation, bool overwrite)
        {
            //Do nothing. 
        }
    }
}
