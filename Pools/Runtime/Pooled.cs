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
using UnityEngine;

namespace mtti.Pools
{
    public static class Pooled
    {
        public static void Release(GameObject obj)
        {
            PooledGameObject.Release(obj);
        }

        public static void Release(Component obj)
        {
            PooledGameObject.Release(obj);
        }
    }

    public class Pooled<T> : IDisposable
        where T : class, new()
    {
        public static Pooled<T> Claim()
        {
            return ObjectPool<Pooled<T>>.Instance.Claim();
        }

        public static Pooled<T> Claim(out T value)
        {
            var pooled = Claim();
            value = pooled.Value;
            return pooled;
        }

        protected T _value;

        public T Value { get { return _value; } }

        public Pooled()
        {
            _value = new T();
        }

        public void Dispose()
        {
            OnDispose();
            ObjectPool<Pooled<T>>.Instance.Release(this);
        }

        protected virtual void OnDispose()
        {
            // Do nothing
        }
    }
}
