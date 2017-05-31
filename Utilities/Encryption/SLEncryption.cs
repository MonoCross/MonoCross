using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using MonoCross.Utilities.Networking;

namespace MonoCross.Utilities.Encryption
{
    public class SLEncryption : BaseEncryption, IEncryption
    {
        const int BUFFER_SIZE = 8192;

        #region Stream Encryption/Decryption Methods

        public override void EncryptStream( Stream inputStream, Stream outputStream, string key, byte[] salt )
        {
            DateTime dtMetric = DateTime.UtcNow;

            byte[] bytes = null;
            AesManaged aesAlg = null;

            // Create the streams used for encryption.
            CryptoStream crypto = null;
            BinaryWriter binaryWriter = null;
            BinaryReader binaryReader = null;
            try
            {
                aesAlg = GetAesManaged( key, salt );

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor( aesAlg.Key, aesAlg.IV );

                binaryReader = new BinaryReader( inputStream );

                crypto = new CryptoStream( outputStream, encryptor, CryptoStreamMode.Write );

                binaryWriter = new BinaryWriter( crypto );

                // process through stream in small chunks to keep peak memory usage down.
                bytes = binaryReader.ReadBytes( BUFFER_SIZE );
                while ( bytes.Length > 0 )
                {
                    binaryWriter.Write( bytes );
                    bytes = binaryReader.ReadBytes( BUFFER_SIZE );
                }
            }
            finally
            {
                if ( binaryWriter != null )
                    binaryWriter.Close();
                if ( crypto != null )
                    crypto.Close();
                if ( binaryReader != null )
                    binaryReader.Close();

                // Clear the RijndaelManaged object.
                if ( aesAlg != null )
                    aesAlg.Clear();
            }
            Device.Log.Debug(string.Format("AesEncryption.EncryptStream(stream, key, salt): Time: {0} milliseconds", DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
        }

        public override void DecryptStream( Stream inputStream, Stream outputStream, string key, byte[] salt )
        {
            DateTime dtMetric = DateTime.UtcNow;

            byte[] bytes;

            AesManaged aesAlg = null;
            CryptoStream crypto = null;
            BinaryWriter binaryWriter = null;
            BinaryReader binaryReader = null;

            try
            {
                aesAlg = GetAesManaged( key, salt );

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor( aesAlg.Key, aesAlg.IV );

                // Create the streams used for decryption.
                crypto = new CryptoStream( inputStream, decryptor, CryptoStreamMode.Read );
                binaryReader = new BinaryReader( crypto );
                binaryWriter = new BinaryWriter( outputStream );

                // process through stream in small chunks to keep peak memory usage down.
                bytes = binaryReader.ReadBytes( BUFFER_SIZE );
                while ( bytes.Length > 0 )
                {
                    binaryWriter.Write( bytes );
                    bytes = binaryReader.ReadBytes( BUFFER_SIZE );
                }
            }
            finally
            {
                if ( binaryWriter != null )
                    binaryWriter.Close();
                if ( crypto != null )
                    crypto.Close();
                if ( binaryReader != null )
                    binaryReader.Close();

                // Clear the RijndaelManaged object.
                if ( aesAlg != null )
                    aesAlg.Clear();
            }
            Device.Log.Debug(string.Format("AesEncryption.DecryptStream(stream, key, salt): Time: {0} milliseconds", DateTime.UtcNow.Subtract(dtMetric).TotalMilliseconds));
        }

        #endregion

    }
}
