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

using System.Collections.Generic;

namespace mtti.Pools
{
    public delegate T ObjectPoolDelegate<T>();

    public class ObjectPool<T> where T : class, new()
    {
        private static ObjectPool<T> s_instance = new ObjectPool<T>();

        public static ObjectPool<T> Instance
        {
            get
            {
                return s_instance;
            }
        }

        /// <summary>
        /// Release all objects in a list and clear the list afterwards. A convenience method for
        /// releasing buffers of pooled objects.
        /// </summary>
        /// <param name="objects">The list of objects to release.</param>
        public static void ReleaseAndClear(List<T> objects)
        {
            for (int i = 0, count = objects.Count; i < count; i++)
            {
                Instance.Release(objects[i]);
                objects[i] = null;
            }
            objects.Clear();
        }

        /// <summary>
        /// Release all objects in an array and set the array's references to null. A convenience
        /// method of releasing buffers of pooled objects.
        /// </summary>
        /// <param name="objects">The array of objects to clear.</param>
        public static void ReleaseAndClear(T[] objects)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                Instance.Release(objects[i]);
                objects[i] = null;
            }
        }

        private object _lock = new object();

        private Queue<T> _pool = new Queue<T>();

        private ObjectPoolDelegate<T> _factory = null;

        public int Count
        {
            get
            {
                lock (this._lock)
                {
                    return _pool.Count;
                }
            }
        }

        public ObjectPool() { }

        public ObjectPool(ObjectPoolDelegate<T> factory)
        {
            _factory = factory;
        }

        public T Claim()
        {
            lock (_lock)
            {
                if (_pool.Count > 0)
                {
                    return _pool.Dequeue();
                }
                else
                {
                    if (_factory == null)
                    {
                        return new T();
                    }
                    else
                    {
                        return _factory();
                    }
                }
            }
        }

        public void Release(T obj)
        {
            lock (_lock)
            {
                _pool.Enqueue(obj);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _pool.Clear();
            }
        }
    }
}
