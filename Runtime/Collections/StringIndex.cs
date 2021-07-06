/*
Copyright 2021 Matti Hiltunen

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections.Generic;

namespace mtti.Funcs.Collections
{
    /// <summary>
    /// Two-way mapping of <c>ulong</c>s and strings.
    /// </summary>
    public class StringIndex : IEnumerable<KeyValuePair<ulong, string>>
    {
        private ulong _nextKey = 1;

        private Dictionary<ulong, string> _itemsByKey
            = new Dictionary<ulong, string>();

        private Dictionary<string, ulong> _itemsByValue
            = new Dictionary<string, ulong>();

        /// <summary>
        /// Get or a set a value by key.
        /// </summary>
        public string this[ulong key]
        {
            get
            {
                return _itemsByKey[key];
            }

            set
            {
                if (_itemsByValue.ContainsKey(value))
                {
                    throw new ArgumentException(
                        $"Index already contains the value {value}"
                    );
                }

                Remove(key);
                if (key >= _nextKey) _nextKey = key + 1;

                _itemsByKey[key] = value;
                _itemsByValue[value] = key;
            }
        }

        /// <summary>
        /// Get the key corresponding to a value.
        /// </summary>
        public ulong this[string val]
        {
            get
            {
                return _itemsByValue[val];
            }
        }

        public int Count
        {
            get { return _itemsByKey.Count; }
        }

        /// <summary>
        /// Add a new value to the index and return the key it was assigned.
        /// </summary>
        public ulong Add(string value)
        {
            if (_itemsByValue.ContainsKey(value)) return _itemsByValue[value];

            var key = _nextKey++;
            _itemsByKey[key] = value;
            _itemsByValue[value] = key;
            return key;
        }

        /// <summary>
        /// Remove an item by key.
        /// </summary>
        public bool Remove(ulong key)
        {
            if (!_itemsByKey.ContainsKey(key)) return false;

            var value = _itemsByKey[key];
            _itemsByKey.Remove(key);
            _itemsByValue.Remove(value);

            return true;
        }

        /// <summary>
        /// Remove an item by value.
        /// </summary>
        public bool Remove(string value)
        {
            if (!_itemsByValue.ContainsKey(value)) return false;
            var key = _itemsByValue[value];
            _itemsByKey.Remove(key);
            _itemsByValue.Remove(value);

            return true;
        }

        public bool ContainsKey(ulong key)
        {
            return _itemsByKey.ContainsKey(key);
        }

        public bool ContainsValue(string value)
        {
            return _itemsByValue.ContainsKey(value);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _itemsByKey.GetEnumerator();
        }

        IEnumerator<KeyValuePair<ulong, string>> IEnumerable<KeyValuePair<ulong, string>>.GetEnumerator()
        {
            return _itemsByKey.GetEnumerator();
        }
    }
}
