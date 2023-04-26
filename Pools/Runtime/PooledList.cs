/*
Copyright 2017-2021 Matti Hiltunen (https://www.mattihiltunen.com)

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
    /// <summary>
    /// A generic list fetched from an object pool and returned back to it
    /// when disposed.
    /// </summary>
    /// <remarks>
    /// Not thread safe. Can't be instantiated directly, you must use the
    /// <see cref="mtti.Pools.PooledList{T}.Claim(int)"/> static method.
    /// </remarks>
    public class PooledList<T> : List<T>, IDisposable
    {
        private static DelegateObjectPool<PooledList<T>> s_pool;

        public static PooledList<T> From(IList<T> source)
        {
            var result = Claim(source.Count);
            for (int i = 0, count = source.Count; i < count; i++)
            {
                result.Add(source[i]);
            }
            return result;
        }

        /// <summary>
        /// Claim a pooled list with an optional minimum capacity.
        /// </summary>
        public static PooledList<T> Claim(int capacity = 0)
        {
            if (s_pool == null)
            {
                s_pool = new DelegateObjectPool<PooledList<T>>(CreateNew);
            }

            var list = s_pool.Claim();
            if (capacity > 0 && list.Capacity < capacity)
            {
                list.Capacity = capacity;
            }
            return list;
        }

        /// <summary>
        /// Discard pooled objects until there are no more than
        /// <c>targetCount</c> remaining.
        /// </summary>
        public static int PrunePool(int targetCount)
        {
            if (s_pool == null) return 0;
            return s_pool.Prune(targetCount);
        }

        private static PooledList<T> CreateNew()
        {
            return new PooledList<T>();
        }

        private byte _version = 0;

        /// <summary>
        /// Pooled object version incremented each time the object is released
        /// back into its pool (with overflow back to 0). Can be used to check
        /// that a reference to a pooled object is still valid.
        /// </summary>
        public byte Version { get { return _version; } }

        /// <summary>
        /// Clears the list, increments its version number and returns it to
        /// the object pool.
        /// </summary>
        public void Dispose()
        {
            unchecked { _version++; }
            Clear();
            s_pool.Release(this);
        }

        private PooledList() { }
    }

    public static class PooledList
    {
        /// <summary>
        /// Claim a pooled list, filling it with values from an existing list.
        /// </summary>
        public static PooledList<T> From<T>(IList<T> source)
        {
            return PooledList<T>.From(source);
        }

        /// <summary>
        /// Claim a pooled list populated with the keys of a dictionary.
        /// </summary>
        public static PooledList<T> FromKeys<T, TValue>(
            Dictionary<T, TValue> source
        )
        {
            var list = PooledList<T>.Claim(source.Count);
            foreach (var item in source)
            {
                list.Add(item.Key);
            }
            return list;
        }
    }
}
