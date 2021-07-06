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

using System.Collections.Generic;

namespace mtti.Funcs.Collections
{
    public class SparseList<T> : IEnumerable<KeyValuePair<ulong, T>>
    {
        private ulong _nextKey;

        private Dictionary<ulong, T> _items = new Dictionary<ulong, T>();

        public T this[uint key]
        {
            get
            {
                return _items[key];
            }

            set
            {
                if (key >= _nextKey) _nextKey = key + 1;
                _items[key] = value;
            }
        }

        public ulong Add(T item)
        {
            var key = _nextKey++;
            _items[key] = item;
            return key;
        }

        public void Remove(ulong key)
        {
            _items.Remove(key);
        }

        public bool ContainsKey(ulong key)
        {
            return _items.ContainsKey(key);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator<KeyValuePair<ulong, T>> IEnumerable<KeyValuePair<ulong, T>>.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
