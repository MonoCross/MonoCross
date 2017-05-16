using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
#if !SILVERLIGHT
            IFile file = new BasicFile();
#else
            IFile file = ( MXDevice.Encryption.Required ? new SLFileEncrypted() : new SLFile() );
#endif
            return file;
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
            IFile file = null;

            switch (fileType)
            {
#if !SILVERLIGHT
                case FileType.BasicFile:
                    file = new BasicFile();
                    break;
#else
                case FileType.SLFile:
                    file = new SLFile();
                    //file = ( MXDevice.Encryption.Required ? new SLFileEncrypted() : new SLFile() );
                    break;
#endif
                default:
                    // returns the default - BasicFile implementation                 
                    break;
            }

            return file;
        }
    }

    /// <summary>
    /// Indicates the file type to use
    /// </summary>
    public enum FileType
    {
#if !SILVERLIGHT
        /// <summary>
        /// Basic file type (default).
        /// </summary>
        BasicFile,
#else
        /// <summary>
        /// Silverlight file type.
        /// </summary>
        SLFile,
#endif
    }
}
