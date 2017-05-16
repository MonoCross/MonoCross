using System.IO;

namespace MonoCross.Utilities.Encryption
{
    /// <summary>
    /// Defines the MonoCross abstract encryption utility.
    /// </summary>
    public interface IEncryption
    {
        /// <summary>
        /// Gets or sets whether encryption is required.
        /// </summary>
        bool Required
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the keyword that, combined with the Salt, will generate the encryption key.
        /// </summary>
        string Key
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the salt that, combined with the Key, will generate the encryption key.
        /// </summary>
        byte[] Salt
        {
            get;
            set;
        }

        /// <summary>
        /// Encrypts the specified text.
        /// </summary>
        /// <param name="txt">The text to encrypt.</param>
        /// <returns>The encrypted text.</returns>
        string EncryptString( string txt );

        /// <summary>
        /// Encrypts the specified text.
        /// </summary>
        /// <param name="txt">The text to encrypt.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        /// <returns>The encrypted text.</returns>
        string EncryptString( string txt, string key, byte[] salt );

        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="cipher">The text to decrypt.</param>
        /// <returns>The decrypted text.</returns>
        string DecryptString( string cipher );

        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="cipher">The text to decrypt.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        /// <returns>The decrypted text.</returns>
        string DecryptString( string cipher, string key, byte[] salt );

        /// <summary>
        /// Encrypts the specified byte array.
        /// </summary>
        /// <param name="bytes">The byte array to encrypt.</param>
        /// <returns>The encrypted byte array.</returns>
        byte[] EncryptBytes( byte[] bytes );

        /// <summary>
        /// Encrypts the specified byte array.
        /// </summary>
        /// <param name="bytes">The byte array to encrypt.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        /// <returns>The encrypted byte array.</returns>
        byte[] EncryptBytes( byte[] bytes, string key, byte[] salt );

        /// <summary>
        /// Decrypts the specified byte array.
        /// </summary>
        /// <param name="cipher">The byte array to decrypt.</param>
        /// <returns>The decrypted byte array.</returns>
        byte[] DecryptBytes( byte[] cipher );

        /// <summary>
        /// Decrypts the specified byte array.
        /// </summary>
        /// <param name="cipher">The byte array to decrypt.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        /// <returns>The decrypted byte array.</returns>
        byte[] DecryptBytes( byte[] cipher, string key, byte[] salt );

        /// <summary>
        /// Reads the contents of the specified input stream and writes an encrypted version of the contents to the specified output stream.
        /// </summary>
        /// <param name="inputStream">The stream that contains the unencrypted data.</param>
        /// <param name="outputStream">The stream to write the encrypted data to.</param>
        void EncryptStream( Stream inputStream, Stream outputStream );

        /// <summary>
        /// Reads the contents of the specified input stream and writes an encrypted version of the contents to the specified output stream.
        /// </summary>
        /// <param name="inputStream">The stream that contains the unencrypted data.</param>
        /// <param name="outputStream">The stream to write the encrypted data to.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        void EncryptStream( Stream inputStream, Stream outputStream, string key, byte[] salt );

        /// <summary>
        /// Reads the contents of the specified input stream and writes a decrypted version of the contents to the specified output stream.
        /// </summary>
        /// <param name="inputStream">The stream that contains the encrypted data.</param>
        /// <param name="outputStream">The stream to write the decrypted data to.</param>
        void DecryptStream( Stream inputStream, Stream outputStream );

        /// <summary>
        /// Reads the contents of the specified input stream and writes a decrypted version of the contents to the specified output stream.
        /// </summary>
        /// <param name="inputStream">The stream that contains the encrypted data.</param>
        /// <param name="outputStream">The stream to write the decrypted data to.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        void DecryptStream( Stream inputStream, Stream outputStream, string key, byte[] salt );
    }
}
