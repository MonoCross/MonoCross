using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace MonoCross.Utilities.Encryption
{
    /// <summary>
    /// Represents an encryption module that utilizes the AES algorithm.
    /// </summary>
    public class AesEncryption : BaseEncryption
    {
        const int BUFFER_SIZE = 8192;

        #region Stream Encryption/Decryption Methods

        /// <summary>
        /// Reads the contents of the specified input stream and writes an encrypted version of the contents to the specified output stream.
        /// </summary>
        /// <param name="inputStream">The stream that contains the unencrypted data.</param>
        /// <param name="outputStream">The stream to write the encrypted data to.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        public override void EncryptStream( Stream inputStream, Stream outputStream, string key, byte[] salt )
        {
            DateTime dtMetric = DateTime.UtcNow;

            byte[] bytes = null;
            AesManaged aesAlg = null;

            // Create the streams used for encryption.
            CryptoStream crypto = null;
            BinaryWriter binaryWriter = null;
            BinaryReader binaryReader = null;
            GZipStream gzip = null;
            try
            {
                if (inputStream != null && inputStream.Length > 0)
                {
                    aesAlg = GetAesManaged(key, salt);

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    binaryReader = new BinaryReader(inputStream);

                    crypto = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write);
                    gzip = new GZipStream(crypto, CompressionMode.Compress);

                    binaryWriter = new BinaryWriter(gzip);

                    // process through stream in small chunks to keep peak memory usage down.
                    bytes = binaryReader.ReadBytes(BUFFER_SIZE);
                    while (bytes.Length > 0)
                    {
                        binaryWriter.Write(bytes);
                        bytes = binaryReader.ReadBytes(BUFFER_SIZE);
                    }
                }
            }
            finally
            {
                if ( binaryWriter != null )
                    binaryWriter.Close();
                if ( gzip != null )
                    gzip.Close();
                if ( crypto != null )
                    crypto.Close();

                if ( binaryReader != null )
                    binaryReader.Close();

                //FIXME: http://support.MonoCross.com/discussions/data-stack/47-v28-encryptor-cache-causes-file-locking-problem-in-windows
                //// Clear the RijndaelManaged object.
                //if ( aesAlg != null )
                //    aesAlg.Clear();
            }
            Device.Log.Metric(string.Format("AesEncryption.EncryptStream(stream, key, salt): Time: {0} milliseconds", DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
        }

        /// <summary>
        /// Reads the contents of the specified input stream and writes a decrypted version of the contents to the specified output stream.
        /// </summary>
        /// <param name="inputStream">The stream that contains the encrypted data.</param>
        /// <param name="outputStream">The stream to write the decrypted data to.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        public override void DecryptStream( Stream inputStream, Stream outputStream, string key, byte[] salt )
        {
            DateTime dtMetric = DateTime.UtcNow;

            byte[] bytes;

            AesManaged aesAlg = null;
            CryptoStream crypto = null;
            GZipStream gzip = null;
            BinaryWriter binaryWriter = null;
            BinaryReader binaryReader = null;

            try
            {
                if (inputStream != null && inputStream.Length > 0)
                {
                    aesAlg = GetAesManaged(key, salt);

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for decryption.
                    crypto = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read);
                    gzip = new GZipStream(crypto, CompressionMode.Decompress);
                    binaryReader = new BinaryReader(gzip);
                    binaryWriter = new BinaryWriter(outputStream);

                    // process through stream in small chunks to keep peak memory usage down.
                    bytes = binaryReader.ReadBytes(BUFFER_SIZE);
                    while (bytes.Length > 0)
                    {
                        binaryWriter.Write(bytes);
                        bytes = binaryReader.ReadBytes(BUFFER_SIZE);
                    }
                }
            }
            finally
            {
                if ( binaryWriter != null )
                    binaryWriter.Close();
                if ( gzip != null )
                    gzip.Close();
                if ( crypto != null )
                    crypto.Close();

                if ( binaryReader != null )
                    binaryReader.Close();

                //FIXME: http://support.MonoCross.com/discussions/data-stack/47-v28-encryptor-cache-causes-file-locking-problem-in-windows
                //// Clear the RijndaelManaged object.
                //if ( aesAlg != null )
                //    aesAlg.Clear();
            }
            Device.Log.Metric( string.Format( "AesEncryption.DecryptStream(stream, key, salt): Time: {0} milliseconds", DateTime.UtcNow.Subtract( dtMetric ).TotalMilliseconds ) );
        }

        #endregion
    }
}
