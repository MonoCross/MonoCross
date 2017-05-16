using System.IO;
using System;
using System.Security.Cryptography;
using System.IO.Compression;
using Path = System.IO.Path;

namespace MonoCross.Utilities.Storage
{
    /// <summary>
    /// Represents a file system utility with basic functionality that works across multiple platforms.
    /// </summary>
    /// <remarks>
    /// For now we will not be catching any exceptions, and will not be very defensive either.
    /// The business developer will have to deal with that...
    /// </remarks>
    public class BasicFile : BaseFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicFile"/> class.
        /// </summary>
        public BasicFile()
        {
        }

        #region IFile Members

        /// <summary>
        /// Reads the specified file and returns its contents as a byte array.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        public override byte[] Read( string filename, string key, byte[] salt )
        {
            DateTime dtMetric = DateTime.UtcNow;

            if (!Exists(filename))
                filename = Path.Combine(Device.ApplicationPath, filename);

            if ( !Exists( filename ) || Length( filename ) == 0 )
                return null;

            byte[] bytes;

            FileStream fileStream = null;
            MemoryStream decryptStream = null;

            try
            {
                // Create the streams used for decryption.
                fileStream = File.Open( filename, FileMode.OpenOrCreate );
                decryptStream = new MemoryStream();

                Device.Encryption.DecryptStream( fileStream, decryptStream, key, salt );

                Device.Log.Metric( string.Format( "BasicFile.Read(key,salt): File: {0} Time: {1} milliseconds", filename, DateTime.UtcNow.Subtract( dtMetric ).TotalMilliseconds ) );

                bytes = decryptStream.ToArray();

                return bytes;
            }

            catch (System.Security.Cryptography.CryptographicException cexc)
            {
                if (cexc.Message.Contains("Bad PKCS7 padding") || cexc.Message.Contains("Padding is invalid and cannot be removed"))
                {
                    // attempt to deserialize file with no encryption.
                    bytes = this.Read(filename, EncryptionMode.NoEncryption);
                    return bytes;
                }
                else
                {
                    Device.Log.Error("error in file read", cexc);
                    var exc = new Exception("error in file read", cexc);
#if !NETCF
                    exc.Data.Add("filename", filename);
#endif
                    throw exc; 
                }
            }
            catch ( Exception exc )
            {
#if !NETCF
                exc.Data.Add( "filename", filename );
#endif
                Device.Log.Error( exc );
                throw;
            }
            finally
            {
                if ( fileStream != null )
				{
                    fileStream.Close();
					fileStream.Dispose();
				}
				if ( decryptStream != null )
				{
					decryptStream.Close();
					decryptStream.Dispose();
				}
            }
        }

        /// <summary>
        /// Returns the names of the files that are inside of the specified directory.
        /// </summary>
        /// <param name="directoryName">The directory to get the files of.</param>
        public override string[] GetFileNames( string directoryName )
        {
            return Directory.GetFiles( directoryName );
        }

        /// <summary>
        /// Gets the names of subdirectories within the specified directory.
        /// </summary>
        /// <param name="directoryName">The directory to get the subdirectories of.</param>
        public override string[] GetDirectoryNames( string directoryName )
        {
            return Directory.GetDirectories( directoryName );
        }

        #endregion
    }
}
