using System;
using System.Collections.Generic;
using System.Web;
using MonoCross.Navigation;
using System.Collections;

namespace MonoCross.Web
{
    public class WebSessionDictionary : SessionDictionary
    {
        /// <summary>
        /// Abandons this instance.
        /// </summary>
        public override void Abandon()
        {
            CheckSession();
            HttpContext.Current.Session.Abandon();
        }

        public override void Add(KeyValuePair<string, object> item)
        {
            CheckSession();
            HttpContext.Current.Session.Add(item.Key, item.Value);
        }

        public override void Clear()
        {
            CheckSession();
            var safeEntries = new Dictionary<string, object>();
            foreach (var key in SafeKeys)
            {
                object entry;
                TryGetValue(key, out entry);
                if (entry != null) safeEntries.Add(key, entry);
            }

            HttpContext.Current.Session.Clear();
            foreach (var pair in safeEntries)
            {
                Add(pair);
            }
        }

        public override bool Contains(KeyValuePair<string, object> item)
        {
            CheckSession();
            return ContainsKey(item.Key);
        }

        public override void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            CheckSession();
            HttpContext.Current.Session.CopyTo(array, arrayIndex);
        }

        public override bool Remove(KeyValuePair<string, object> item)
        {
            CheckSession();
            bool answer = false;
            if (Contains(item))
            {
                HttpContext.Current.Session.Remove(item.Key);
                answer = true;
            }
            return answer;
        }

        public override void CopyTo(Array array, int index)
        {
            CheckSession();
            HttpContext.Current.Session.CopyTo(array, index);
        }

        public override int Count
        {
            get
            {
                CheckSession();
                return HttpContext.Current.Session.Count;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                CheckSession();
                return HttpContext.Current.Session.IsReadOnly;
            }
        }

        public override void Add(string key, object value)
        {
            CheckSession();
            HttpContext.Current.Session.Add(key, value);
        }

        public override bool ContainsKey(string key)
        {
            CheckSession();
            return HttpContext.Current.Session.Keys.Any(myKey => myKey.ToString() == key);
        }

        public override bool Remove(string key)
        {
            CheckSession();
            bool answer = false;
            if (ContainsKey(key))
            {
                HttpContext.Current.Session.Remove(key);
                answer = true;
            }
            return answer;
        }

        public override object this[string key]
        {
            get
            {
                CheckSession();
                return HttpContext.Current.Session[key];
            }
            set
            {
                CheckSession();
                HttpContext.Current.Session[key] = value;
            }
        }

        public override ICollection<string> Keys
        {
            get
            {
                CheckSession();
                var finalKeys = new List<string>();
                foreach (string myKey in HttpContext.Current.Session.Keys)
                {
                    finalKeys.Add(myKey);
                }
                return finalKeys;
            }
        }

        public override ICollection<object> Values
        {
            get
            {
                CheckSession();
                var finalVals = new List<object>();
                foreach (string myKey in HttpContext.Current.Session.Keys)
                {
                    finalVals.Add(HttpContext.Current.Session[myKey]);
                }
                return finalVals;
            }
        }

        public override void Add(object key, object value)
        {
            CheckSession();
            HttpContext.Current.Session.Add(key.ToString(), value);
        }

        public override bool Contains(object key)
        {
            return ContainsKey(key.ToString());
        }

        public override void Remove(object key)
        {
            Remove(key.ToString());
        }

        public override bool TryGetValue(string key, out object value)
        {
            CheckSession();
            var containsKey = ContainsKey(key);
            value = containsKey ? HttpContext.Current.Session[key] : null;
            return containsKey;
        }

        private void CheckSession()
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                throw new NullReferenceException("Session not yet initialized.");
            }
        }
    }
}