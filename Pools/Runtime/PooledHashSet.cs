/*
Copyright 2017-2022 Matti Hiltunen (https://www.mattihiltunen.com)

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

namespace mtti.Pools
{
    public class PooledHashSet<T> : HashSet<T>, IDisposable
    {
        public static PooledHashSet<T> Claim()
        {
            return ObjectPool<PooledHashSet<T>>.Instance.Claim();
        }

        private byte _version = 0;

        public byte Version { get { return _version; } }

        public void Dispose()
        {
            unchecked { _version++; }
            Clear();
            ObjectPool<PooledHashSet<T>>.Instance.Release(this);
        }
    }

    public static class PooledHashSet
    {
        public static PooledHashSet<T> From<T>(IList<T> source)
        {
            var result = PooledHashSet<T>.Claim();
            for (int i = 0, count = source.Count; i < count; i++)
            {
                result.Add(source[i]);
            }
            return result;
        }

        public static PooledHashSet<T> From<T>(IEnumerable<T> source)
        {
            var result = PooledHashSet<T>.Claim();
            foreach (var item in source)
            {
                result.Add(item);
            }
            return result;
        }
    }
}
