using System.IO;
using System.Security.Cryptography;
using System;
using MonoCross.Utilities.Network;
using System.IO.Compression;

namespace MonoCross.Utilities.Storage
{
    internal class BasicFileEncrypted : BasicFile, IFile
    {
        internal BasicFileEncrypted()
        {
        }

        private static AesManaged GetAesManaged()
        {
            if (string.IsNullOrEmpty(Device.Encryption.Key))
                throw new ArgumentNullException( "Application.Encryption.Key" );
            if (Device.Encryption.Salt == null)
                throw new ArgumentNullException( "Application.Encryption.Salt" );

            Rfc2898DeriveBytes rdb = new Rfc2898DeriveBytes(Device.Encryption.Key, Device.Encryption.Salt);
            byte[] key = rdb.GetBytes( 32 );
            byte[] iv = rdb.GetBytes( 16 );

            if ( key == null || key.Length <= 0 )
                throw new ArgumentNullException( "key" );
            if ( iv == null || iv.Length <= 0 )
                throw new ArgumentNullException( "iv" );

            return new AesManaged()
            {
                Key = key,
                IV = iv
            };
        }

        #region IFile Members

        /// <summary>
        /// Saves a stream into a file with encryption
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="contents">Stream</param>
        public override void Save( string filename, Stream contents )
        {
            EnsureDirectoryExists( filename );

            DateTime dtMetric = DateTime.UtcNow;

            byte[] bytes = null;
            AesManaged aesAlg = null;

            // Create the streams used for encryption.
            FileStream fileStream = null;
            CryptoStream csEncrypt = null;
            BinaryWriter binaryWriter = null;
            BinaryReader binaryReader = null;
            GZipStream gzip = null;
            try
            {
                aesAlg = GetAesManaged();

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor( aesAlg.Key, aesAlg.IV );

                binaryReader = new BinaryReader( contents );
                fileStream = new FileStream( filename, FileMode.Create );
                csEncrypt = new CryptoStream( fileStream, encryptor, CryptoStreamMode.Write );
                gzip = new GZipStream( csEncrypt, CompressionMode.Compress );
                binaryWriter = new BinaryWriter( gzip );

                // process through stream in small chunks to keep peak memory usage down.
                bytes = binaryReader.ReadBytes( 6144 );
                while ( bytes.Length > 0 )
                {
                    binaryWriter.Write( bytes );
                    bytes = binaryReader.ReadBytes( 6144 );
                }
            }
            finally
            {
                if ( binaryWriter != null )
                    binaryWriter.Close();
                if ( gzip != null )
                    gzip.Close();
                if ( csEncrypt != null )
                    csEncrypt.Close();
                if ( fileStream != null )
                    fileStream.Close();
                if ( binaryReader != null )
                    binaryReader.Close();

                // Clear the RijndaelManaged object.
                if ( aesAlg != null )
                    aesAlg.Clear();
            }
            Device.Log.Debug( string.Format( "BasicFileEncrypted.Save(stream): File: {0} Time: {1} milliseconds", filename, DateTime.UtcNow.Subtract( dtMetric ).TotalMilliseconds ) );

        }

        public override void Save( string filename, string contents )
        {
            byte[] bytes = NetworkUtils.StrToByteArray( contents );
            Save( filename, bytes );
        }

        public override void Save( string filename, byte[] contents )
        {
            EnsureDirectoryExists( filename );

            DateTime dtMetric = DateTime.UtcNow;
            AesManaged aesAlg = null;

            FileStream fileStream = null;
            CryptoStream csEncrypt = null;
            GZipStream gzip = null;

            try
            {
                aesAlg = GetAesManaged();

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor( aesAlg.Key, aesAlg.IV );

                // Create the streams used for encryption.
                fileStream = new FileStream( filename, FileMode.Create );
                csEncrypt = new CryptoStream( fileStream, encryptor, CryptoStreamMode.Write );
//                csEncrypt.Write( contents, 0, contents.Length );

                gzip = new GZipStream( csEncrypt, CompressionMode.Compress );
                gzip.Write( contents, 0, contents.Length );

            }
            finally
            {
                if ( gzip != null )
                    gzip.Close();
                if ( csEncrypt != null )
                    csEncrypt.Close();
                if ( fileStream != null )
                    fileStream.Close();

                // Clear the RijndaelManaged object.
                if ( aesAlg != null )
                    aesAlg.Clear();
            }
            Device.Log.Debug( string.Format( "BasicFileEncrypted.Save(byte[]): File: {0} Time: {1} milliseconds", filename, DateTime.UtcNow.Subtract( dtMetric ).TotalMilliseconds ) );

        }

        public override byte[] Read( string filename )
        {
            DateTime dtMetric = DateTime.UtcNow;

            if ( !Exists( filename ) )
                return null;

            byte[] bytes;
            AesManaged aesAlg = null;

            FileStream fileStream = null;
            CryptoStream csDecrypt = null;
            GZipStream gzip = null;

            try
            {
                aesAlg = GetAesManaged();

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor( aesAlg.Key, aesAlg.IV );

                // Create the streams used for decryption.
                fileStream = File.Open( filename, FileMode.OpenOrCreate );
                csDecrypt = new CryptoStream( fileStream, decryptor, CryptoStreamMode.Read );

                //sReader = new StreamReader( csDecrypt );
                gzip = new GZipStream( csDecrypt, CompressionMode.Decompress );
                //sReader = new StreamReader( gzip );

                int buffersize = 100;
                byte[] buffer = new byte[buffersize];
                int offset = 0, read = 0, size = 0;
                do
                {
                    // If the buffer doesn't offer enough space left create a new array
                    // with the double size. Copy the current buffer content to that array
                    // and use that as new buffer.
                    if ( buffer.Length < size + buffersize )
                    {
                        byte[] tmp = new byte[buffer.Length * 2];
                        Array.Copy( buffer, tmp, buffer.Length );
                        buffer = tmp;
                    }

                    // Read the net chunk of data.
                    // read = csDecrypt.Read( buffer, offset, buffersize );
                    read = gzip.Read( buffer, offset, buffersize );

                    // Increment offset and read size.
                    offset += buffersize;
                    size += read;
                } while ( read == buffersize ); // Terminate if we read less then the buffer size.

                // Copy only that amount of data to the result that has actually been read!
                bytes = new byte[size];
                Array.Copy( buffer, bytes, size );

                Device.Log.Debug( string.Format( "BasicFileEncrypted.Read: File: {0} Time: {1} milliseconds", filename, DateTime.UtcNow.Subtract( dtMetric ).TotalMilliseconds ) );

                if ( bytes == null )
                    return base.Read( filename );
                return bytes;
            }
            finally
            {
                if ( gzip != null )
                    gzip.Close();
                if ( csDecrypt != null )
                    csDecrypt.Close();
                if ( fileStream != null )
                    fileStream.Close();

                // Clear the RijndaelManaged object.
                if ( aesAlg != null )
                    aesAlg.Clear();
            }
        }

        public override string ReadString( string filename )
        {
            DateTime dtMetric = DateTime.UtcNow;

            if ( !Exists( filename ) )
                return null;

            string retval = null;
            AesManaged aesAlg = null;

            FileStream fileStream = null;
            CryptoStream csDecrypt = null;
            GZipStream gzip = null;
            StreamReader sReader = null;
            try
            {
                aesAlg = GetAesManaged();

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor( aesAlg.Key, aesAlg.IV );

                // Create the streams used for decryption.
                fileStream = File.Open( filename, FileMode.OpenOrCreate );
                csDecrypt = new CryptoStream( fileStream, decryptor, CryptoStreamMode.Read );
                //sReader = new StreamReader( csDecrypt );
                gzip = new GZipStream( csDecrypt, CompressionMode.Decompress );
                sReader = new StreamReader( gzip );

                retval = sReader.ReadToEnd();
            }
            catch ( CryptographicException cexc )
            {
                // if we have cryptographic failure, then try the base class before giving up.
                Device.Log.Error( cexc );
            }
#if !MONO
            catch ( InvalidDataException iexc )
            {
                // if we have cryptographic failure, then try the base class before giving up.
                Device.Log.Error( iexc );
            }
#endif
            catch ( Exception exc )
            {
                Device.Log.Error( exc );
            }
            finally
            {
                //if ( sReader != null )
                //    sReader.Close();
                //if ( csDecrypt != null )
                //    csDecrypt.Close();
                //if ( gzip != null )
                //    gzip.Close();
                if ( fileStream != null )
                    fileStream.Close();

                // Clear the RijndaelManaged object.
                if ( aesAlg != null )
                    aesAlg.Clear();
            }
            
            Device.Log.Debug( string.Format( "BasicFileEncrypted.ReadString: File: {0} Time: {1} milliseconds", filename, DateTime.UtcNow.Subtract( dtMetric ).TotalMilliseconds ) );

            if ( string.IsNullOrEmpty( retval ) )
                return base.ReadString( filename );
            return retval;
        }

        #endregion
    }
}
