using System;
using System.IO;
using MonoCross.Utilities.Network;

namespace MonoCross.Utilities.Encryption
{
    /// <summary>
    /// <see cref="IEncryption"/> implementation that skips encryption.
    /// </summary>
    public class MockEncryption : IEncryption
    {
        /// <summary>
        /// Gets or sets whether encryption is required.
        /// </summary>
        public bool Required
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the keyword that, combined with the Salt, will generate the encryption key.
        /// </summary>
        public string Key
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the salt that, combined with the Key, will generate the encryption key.
        /// </summary>
        public byte[] Salt
        {
            get;
            set;
        }

        /// <summary>
        /// Encrypts the specified text.
        /// </summary>
        /// <param name="txt">The text to encrypt.</param>
        /// <returns>
        /// The encrypted text.
        /// </returns>
        public string EncryptString(string txt)
        {
            return EncryptString(txt, Key, Salt);
        }

        /// <summary>
        /// Encrypts the specified text.
        /// </summary>
        /// <param name="txt">The text to encrypt.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        /// <returns>
        /// The encrypted text.
        /// </returns>
        public string EncryptString(string txt, string key, byte[] salt)
        {
            if (string.IsNullOrEmpty(txt))
                return string.Empty;

            byte[] plain = NetworkUtils.StrToByteArray(txt);
            byte[] encrypted = EncryptBytes(plain, key, salt);

            return Convert.ToBase64String(encrypted, 0, encrypted.Length);
        }

        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="cipher">The text to decrypt.</param>
        /// <returns>
        /// The decrypted text.
        /// </returns>
        public string DecryptString(string cipher)
        {
            return DecryptString(cipher, Key, Salt);
        }

        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="cipher">The text to decrypt.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        /// <returns>
        /// The decrypted text.
        /// </returns>
        public string DecryptString(string cipher, string key, byte[] salt)
        {
            if (string.IsNullOrEmpty(cipher))
                return string.Empty;

            byte[] cipherBytes = Convert.FromBase64String(cipher);
            byte[] decrypted = DecryptBytes(cipherBytes, key, salt);

            return NetworkUtils.ByteArrayToStr(decrypted);
        }

        /// <summary>
        /// Encrypts the specified byte array.
        /// </summary>
        /// <param name="bytes">The byte array to encrypt.</param>
        /// <returns>
        /// The encrypted byte array.
        /// </returns>
        public byte[] EncryptBytes(byte[] bytes)
        {
            return EncryptBytes(bytes, Key, Salt);
        }

        /// <summary>
        /// Encrypts the specified byte array.
        /// </summary>
        /// <param name="bytes">The byte array to encrypt.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        /// <returns>
        /// The encrypted byte array.
        /// </returns>
        public byte[] EncryptBytes(byte[] bytes, string key, byte[] salt)
        {
            using (MemoryStream msInput = new MemoryStream(bytes))
            using (MemoryStream msOutput = new MemoryStream())
            {
                EncryptStream(msInput, msOutput, key, salt);
                return msOutput.ToArray();
            }
        }

        /// <summary>
        /// Decrypts the specified byte array.
        /// </summary>
        /// <param name="cipher">The byte array to decrypt.</param>
        /// <returns>
        /// The decrypted byte array.
        /// </returns>
        public byte[] DecryptBytes(byte[] cipher)
        {
            return DecryptBytes(cipher, Key, Salt);
        }

        /// <summary>
        /// Decrypts the specified byte array.
        /// </summary>
        /// <param name="cipher">The byte array to decrypt.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        /// <returns>
        /// The decrypted byte array.
        /// </returns>
        public byte[] DecryptBytes(byte[] cipher, string key, byte[] salt)
        {
            using (MemoryStream msInput = new MemoryStream(cipher))
            using (MemoryStream msOutput = new MemoryStream())
            {
                DecryptStream(msInput, msOutput, key, salt);
                return msOutput.ToArray();
            }
        }


        /// <summary>
        /// Reads the contents of the specified input stream and writes an encrypted version of the contents to the specified output stream.
        /// </summary>
        /// <param name="inputStream">The stream that contains the unencrypted data.</param>
        /// <param name="outputStream">The stream to write the encrypted data to.</param>
        public virtual void EncryptStream(Stream inputStream, Stream outputStream)
        {
            EncryptStream(inputStream, outputStream, Key, Salt);
        }

        /// <summary>
        /// Reads the contents of the specified input stream and writes an encrypted version of the contents to the specified output stream.
        /// </summary>
        /// <param name="inputStream">The stream that contains the unencrypted data.</param>
        /// <param name="outputStream">The stream to write the encrypted data to.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        public void EncryptStream(Stream inputStream, Stream outputStream, string key, byte[] salt)
        {
        }

        /// <summary>
        /// Reads the contents of the specified input stream and writes a decrypted version of the contents to the specified output stream.
        /// </summary>
        /// <param name="inputStream">The stream that contains the encrypted data.</param>
        /// <param name="outputStream">The stream to write the decrypted data to.</param>
        public virtual void DecryptStream(Stream inputStream, Stream outputStream)
        {
            DecryptStream(inputStream, outputStream, Key, Salt);
        }

        /// <summary>
        /// Reads the contents of the specified input stream and writes a decrypted version of the contents to the specified output stream.
        /// </summary>
        /// <param name="inputStream">The stream that contains the encrypted data.</param>
        /// <param name="outputStream">The stream to write the decrypted data to.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        public void DecryptStream(Stream inputStream, Stream outputStream, string key, byte[] salt)
        {
        }
    }
}
