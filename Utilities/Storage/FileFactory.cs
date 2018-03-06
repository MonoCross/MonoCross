using MonoCross.Navigation;

namespace MonoCross.Utilities.Storage
{
    /// <summary>
    /// Factory for the creation of a file access strategy.
    /// </summary>
    internal static class FileFactory
    {
        /// <summary>
        /// Creates an IFile instance.
        /// </summary>
        /// <returns></returns>
        internal static IFile Create()
        {
            return MXContainer.Resolve<IFile>();
        }

        // If we ever want to make an implementation of IFile that we want in core,
        // like BasicFile for example then include it here...
        /// <summary>
        /// Creates an IFile instance of the specified file type.
        /// </summary>
        /// <param name="fileType">Type of the file.</param>
        /// <returns></returns>
        internal static IFile Create(FileType fileType)
        {
            return MXContainer.Resolve<IFile>(fileType.ToString());
        }
    }

    /// <summary>
    /// Indicates the file type to use
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// Basic file type (default).
        /// </summary>
        BasicFile,
    }
}
