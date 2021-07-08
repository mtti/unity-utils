/*
Copyright 2017-2020 Matti Hiltunen

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
    /// <summary>
    /// Like <see cref="System.Collections.Generic.Dictionary{TKey, TValue}" />
    /// but stores a collection of values under each key.
    /// </summary>
    public class MultiValueDictionary<KeyT, ValueT>
    {
        private Dictionary<KeyT, List<ValueT>> _index
            = new Dictionary<KeyT, List<ValueT>>();

        public int Get(KeyT key, List<ValueT> result)
        {
            if (!_index.ContainsKey(key))
            {
                return 0;
            }

            List<ValueT> items = _index[key];
            int itemCount = items.Count;

            for (int i = 0, count = itemCount; i < count; i++)
            {
                result.Add(items[i]);
            }
            return itemCount;
        }

        public ValueT GetFirst(KeyT key)
        {
            ValueT result;
            GetFirst(key, out result);
            return result;
        }

        public bool GetFirst(KeyT key, out ValueT result)
        {
            if (!_index.ContainsKey(key))
            {
                result = default(ValueT);
                return false;
            }

            var items = _index[key];
            if (items.Count == 0)
            {
                result = default(ValueT);
                return false;
            }

            result = items[0];
            return true;
        }

        public int Count(KeyT key)
        {
            if (!_index.ContainsKey(key))
            {
                return 0;
            }
            return _index[key].Count;
        }

        public void Add(KeyT key, ValueT value)
        {
            List<ValueT> list;

            if (_index.ContainsKey(key))
            {
                list = _index[key];
            }
            else
            {
                list = new List<ValueT>();
                _index[key] = list;
            }

            if (list.Contains(value))
            {
                return;
            }

            list.Add(value);
        }

        public void Remove(KeyT key, ValueT value)
        {
            if (!_index.ContainsKey(key))
            {
                return;
            }
            _index[key].Remove(value);
        }

        public void Remove(KeyT key)
        {
            if (!_index.ContainsKey(key)) return;
            _index[key].Clear();
        }

        public void Clear()
        {
            foreach (KeyValuePair<KeyT, List<ValueT>> pair in _index)
            {
                pair.Value.Clear();
            }
            _index.Clear();
        }
    }
}
