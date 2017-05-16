namespace MonoCross.Utilities
{
    /// <summary>
    /// The available modes of data encryption.
    /// </summary>
    public enum EncryptionMode
    {
        /// <summary>
        /// Encryption is determined by the application.
        /// </summary>
        Default,
        /// <summary>
        /// No encryption will be done.
        /// </summary>
        NoEncryption,
        /// <summary>
        /// The data will be encrypted.
        /// </summary>
        Encryption,
    }
}
