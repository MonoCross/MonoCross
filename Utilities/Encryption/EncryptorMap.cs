using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
//using MonoCross;

namespace MonoCross.Utilities.Encryption
{
    /// <summary>
    /// Represents a static mapping of expiring encryption modules.
    /// </summary>
    public static class EncryptorMap
    {
        private static object staticLock = new object();  // lock object for static method

        private static Dictionary<string, Encryptor> _map = new Dictionary<string, Encryptor>();

        /// <summary>
        /// Adds a new item to the map with the specified key.  If the key already exists in the map, the item using it will be replaced.
        /// </summary>
        /// <param name="key">The key to associate with the encryptor.</param>
        /// <param name="aesManaged">The encryptor to add to the map.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="aesManaged"/> is <c>null</c>.</exception>
        public static void Add(string key, AesManaged aesManaged)
        {
            if (aesManaged == null)
                throw new ArgumentNullException("aesManaged");

            lock (staticLock)
            {
                CleanExpired();

                // if the encryptor key doesn't already exist in the map then add it.
                if (!_map.ContainsKey(key))
                    _map.Add(key, new Encryptor(aesManaged));
                else
                    _map[key] = new Encryptor(aesManaged);
            }
        }

        /// <summary>
        /// Removes from the map any items that have passed their expiration times.
        /// </summary>
        private static void CleanExpired()
        {
            lock (staticLock)
            {
                _map.Where(kvp => kvp.Value.IsExpired)
                    .ToList()
                    .ForEach(kvp => _map.Remove(kvp.Key));
            }
        }

        /// <summary>
        /// Returns the encryptor associated with the specified key.  If no encryptor is found, null is returned.
        /// </summary>
        /// <param name="key">The key of the encryptor to retrieve.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is <c>null</c> or an empty string.</exception>
        public static AesManaged Get(string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "Requested Encryptor key is null or empty");

            lock (staticLock)
            {
                CleanExpired();

                Encryptor encryptor;
                if (!_map.TryGetValue(key, out encryptor))
                    return null;

                encryptor.BumpExpiration();  // extend expiration of enryptor. 
                return encryptor.AesManaged;  // return encryptor or null.
            }
        }

        /// <summary>
        /// Removes any expired items and adds the specified encryptor to the map with the specified key.
        /// If the key is already being used, the encryptor using it will be replaced.
        /// </summary>
        /// <param name="key">The key to associate with the encryptor.</param>
        /// <param name="aesManaged">The encryptor to add to the map.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="aesManaged"/> is <c>null</c>.</exception>
        public static void Update(string key, AesManaged aesManaged)
        {
            if (aesManaged == null)
                throw new ArgumentNullException("aesManaged");

            lock (staticLock)
            {
                CleanExpired();

                // if the key exists in the map then replace it with current parameter..
                if (_map.ContainsKey(key))
                    _map.Remove(key);

                _map.Add(key, new Encryptor(aesManaged));
            }
        }

        /// <summary>
        /// Removes all items from the map.
        /// </summary>
        public static void Clear()
        {
            lock (staticLock)
            {
                _map.Clear();
            }
        }

        /// <summary>
        /// Removes the item using the specified key from the map.
        /// </summary>
        /// <param name="key">The key of the item to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is <c>null</c> or an empty string.</exception>
        public static void Remove(string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "Requested Encryptor key is null or empty");

            lock (staticLock)
            {
                CleanExpired();

                // if the encryptor key exists in the map, then remove it.
                if (_map.ContainsKey(key))
                    _map.Remove(key);
            }
        }

        private class Encryptor
        {
            private int ExpirationPeriod = 30;  // number of minutes before encryptor is considered expired.

            public Encryptor(AesManaged aesManaged)
            {
                AesManaged = aesManaged;
                Expiration = DateTime.UtcNow.AddMinutes(ExpirationPeriod);
            }

            public AesManaged AesManaged { get; set; }
            public DateTime _expiration;

            /// <summary>
            /// cache expiration date in universal time
            /// </summary>
            public DateTime Expiration
            {
                get
                {
                    return _expiration;
                }
                set
                {
                    _expiration = value.ToUniversalTime();
                }
            }

            public void BumpExpiration()
            {
                Expiration = DateTime.UtcNow.AddMinutes(ExpirationPeriod);
            }

            public bool IsExpired
            {
                get
                {
                    return (DateTime.UtcNow >= Expiration);
                }
            }
        }
    }
}
