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
    /// A simple object pool which calls a delegate function to create new
    /// instances. Not thread safe.
    /// </summary>
    public class DelegateObjectPool<T> where T : class
    {
        private Queue<T> _pool = new Queue<T>();

        private Func<T> _factory = null;

        public int Count
        {
            get
            {
                return _pool.Count;
            }
        }

        public DelegateObjectPool(Func<T> factory)
        {
            _factory = factory;
        }

        public T Claim()
        {
            if (_pool.Count > 0)
            {
                return _pool.Dequeue();
            }
            else
            {
                return _factory();
            }
        }

        public void Release(T obj)
        {
            _pool.Enqueue(obj);
        }

        /// <summary>
        /// If the pool has more than <c>targetCount</c> objects waiting,
        /// destroy objects until the target count is reached.
        /// </summary>
        public int Prune(int targetCount)
        {
            if (targetCount < 0) targetCount = 0;

            int prunedCount = 0;
            while (_pool.Count > targetCount)
            {
                _pool.Dequeue();
                prunedCount += 1;
            }

            return prunedCount;
        }

        public void Clear()
        {
            _pool.Clear();
        }
    }
}
