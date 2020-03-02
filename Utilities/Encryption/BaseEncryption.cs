using System;
using System.Security.Cryptography;
using System.IO;
using MonoCross.Utilities.Network;

namespace MonoCross.Utilities.Encryption
{
    /// <summary>
    /// Represents a base class for data encryption modules.  This class is abstract.
    /// </summary>
    public abstract class BaseEncryption : IEncryption
    {
        const int BUFFER_SIZE = 8192;

        #region Public Properties

        /// <summary>
        /// Gets or sets whether encryption is required.
        /// </summary>
        public virtual bool Required
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the keyword that, combined with the Salt, will generate the encryption key.
        /// </summary>
        public virtual string Key
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the salt that, combined with the Key, will generate the encryption key.
        /// </summary>
        public virtual byte[] Salt
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Returns an <see cref="AesManaged"/> instance with the specified key and key salt.
        /// </summary>
        /// <param name="aesKey">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        protected virtual AesManaged GetAesManaged(string aesKey, byte[] salt)
        {
            AesManaged aesManaged = EncryptorMap.Get(aesKey);

            if (aesManaged == null)
            {
                aesManaged = CreateAesManaged(aesKey, salt);
                EncryptorMap.Add(aesKey, aesManaged);
            }

            return aesManaged;
        }

        /// <summary>
        /// Creates a new <see cref="AesManaged"/> instance with the specified key and key salt.
        /// </summary>
        /// <param name="aesKey">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="aesKey"/> is <c>null</c> or an empty string, or if <paramref name="salt"/> is <c>null</c>.</exception>
        protected virtual AesManaged CreateAesManaged(string aesKey, byte[] salt)
        {
            if (string.IsNullOrEmpty(aesKey))
                throw new ArgumentNullException("aesKey");
            if (salt == null)
                throw new ArgumentNullException("salt");

            Rfc2898DeriveBytes rdb = new Rfc2898DeriveBytes(aesKey, salt);
            byte[] key = rdb.GetBytes(32);
            byte[] iv = rdb.GetBytes(16);

            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            return new AesManaged()
            {
                Key = key,
                IV = iv
            };
        }

        #region String Encryption/Decryption Methods

        /// <summary>
        /// Encrypts the specified text.
        /// </summary>
        /// <param name="txt">The text to encrypt.</param>
        /// <returns>The encrypted text.</returns>
        public virtual string EncryptString(string txt)
        {
            return EncryptString(txt, Key, Salt);
        }

        /// <summary>
        /// Encrypts the specified text.
        /// </summary>
        /// <param name="txt">The text to encrypt.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        /// <returns>The encrypted text.</returns>
        public virtual string EncryptString(string txt, string key, byte[] salt)
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
        /// <returns>The decrypted text.</returns>
        public virtual string DecryptString(string cipher)
        {
            return DecryptString(cipher, Key, Salt);
        }

        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="cipher">The text to decrypt.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        /// <returns>The decrypted text.</returns>
        public virtual string DecryptString(string cipher, string key, byte[] salt)
        {
            if (string.IsNullOrEmpty(cipher))
                return string.Empty;

            byte[] cipherBytes = Convert.FromBase64String(cipher);
            byte[] decrypted = DecryptBytes(cipherBytes, key, salt);

            return NetworkUtils.ByteArrayToStr(decrypted);
        }

        #endregion

        #region Byte Encryption/Decryption Methods

        /// <summary>
        /// Encrypts the specified byte array.
        /// </summary>
        /// <param name="contents">The byte array to encrypt.</param>
        /// <returns>The encrypted byte array.</returns>
        public virtual byte[] EncryptBytes(byte[] contents)
        {
            return EncryptBytes(contents, Key, Salt);
        }

        /// <summary>
        /// Encrypts the specified byte array.
        /// </summary>
        /// <param name="contents">The byte array to encrypt.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        /// <returns>The encrypted byte array.</returns>
        public virtual byte[] EncryptBytes(byte[] contents, string key, byte[] salt)
        {
            MemoryStream msInput = new MemoryStream(contents);
            MemoryStream msOutput = new MemoryStream();

            EncryptStream(msInput, msOutput, key, salt);

            byte[] retArray = msOutput.ToArray();
            return retArray;
        }

        /// <summary>
        /// Decrypts the specified byte arrat.
        /// </summary>
        /// <param name="cipher">The byte array to decrypt.</param>
        /// <returns>The decrypted byte array.</returns>
        public virtual byte[] DecryptBytes(byte[] cipher)
        {
            return DecryptBytes(cipher, Key, Salt);
        }

        /// <summary>
        /// Decrypts the specified byte array.
        /// </summary>
        /// <param name="cipher">The byte array to decrypt.</param>
        /// <param name="key">The keyword to derive the encryption key from.</param>
        /// <param name="salt">The key salt to derive the encryption key from.</param>
        /// <returns>The decrypted byte array.</returns>
        public virtual byte[] DecryptBytes(byte[] cipher, string key, byte[] salt)
        {
            MemoryStream msInput = new MemoryStream(cipher);
            MemoryStream msOutput = new MemoryStream();

            DecryptStream(msInput, msOutput, key, salt);

            byte[] retArray = msOutput.ToArray();
            return retArray;
        }

        #endregion

        #region Stream Encryption/Decryption Methods

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
        public abstract void EncryptStream(Stream inputStream, Stream outputStream, string key, byte[] salt);

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
        public abstract void DecryptStream(Stream inputStream, Stream outputStream, string key, byte[] salt);

        #endregion

    }
}
