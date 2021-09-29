/*
Copyright 2017-2021 Matti Hiltunen

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
    public class MultiValueDictionary<KeyT, ValueT> :
        IEnumerable<KeyValuePair<KeyT, ValueT>>
    {
        public class Enumerator : IEnumerator<KeyValuePair<KeyT, ValueT>>
        {
            private Dictionary<KeyT, List<ValueT>> _collection;

            private IEnumerator<KeyValuePair<KeyT, List<ValueT>>> _keyEnumerator;

            private KeyT _currentKey;

            private IEnumerator<ValueT> _valueEnumerator;

            public Enumerator(Dictionary<KeyT, List<ValueT>> collection)
            {
                _collection = collection;
                _keyEnumerator = _collection.GetEnumerator();
            }

            KeyValuePair<KeyT, ValueT> IEnumerator<KeyValuePair<KeyT, ValueT>>.Current
            {
                get
                {
                    return new KeyValuePair<KeyT, ValueT>(
                        _keyEnumerator.Current.Key,
                        _valueEnumerator.Current
                    );
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return ((IEnumerator<KeyValuePair<KeyT, ValueT>>)this).Current;
                }
            }

            public bool MoveNext()
            {
                while (true)
                {
                    if (_valueEnumerator == null || !_valueEnumerator.MoveNext())
                    {
                        if (_valueEnumerator != null) _valueEnumerator.Dispose();
                        if (!_keyEnumerator.MoveNext())
                        {
                            _keyEnumerator.Dispose();
                            return false;
                        }

                        _valueEnumerator
                            = _keyEnumerator.Current.Value.GetEnumerator();
                        continue;
                    }

                    return true;
                }

            }

            public void Reset()
            {
                if (_valueEnumerator != null) _valueEnumerator.Dispose();
                _valueEnumerator = null;
                _keyEnumerator.Reset();
            }

            public void Dispose()
            {
                if (_valueEnumerator != null) _valueEnumerator.Dispose();
                if (_keyEnumerator != null) _keyEnumerator.Dispose();
                _valueEnumerator = null;
                _keyEnumerator = null;
            }
        }

        private Dictionary<KeyT, List<ValueT>> _index
            = new Dictionary<KeyT, List<ValueT>>();

        public int Get(KeyT key, ICollection<ValueT> result)
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

        public List<ValueT> Get(KeyT key)
        {
            var result = new List<ValueT>();
            Get(key, result);
            return result;
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

        public int GetAll(ICollection<ValueT> result)
        {
            int resultCount = 0;
            foreach (var item in _index)
            {
                var values = item.Value;
                for (int i = 0, count = values.Count; i < count; i++)
                {
                    result.Add(values[i]);
                    resultCount += 1;
                }
            }
            return resultCount;
        }

        public List<ValueT> GetAll()
        {
            var result = new List<ValueT>();
            GetAll(result);
            return result;
        }

        public void Clear()
        {
            foreach (KeyValuePair<KeyT, List<ValueT>> pair in _index)
            {
                pair.Value.Clear();
            }
            _index.Clear();
        }

        IEnumerator<KeyValuePair<KeyT, ValueT>> IEnumerable<KeyValuePair<KeyT, ValueT>>.GetEnumerator()
        {
            return new Enumerator(_index);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<KeyT, ValueT>>)this).GetEnumerator();
        }
    }
}
