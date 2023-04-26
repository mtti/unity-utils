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
using UnityEngine;

namespace mtti.Pools
{
    public abstract class BaseGameObjectPool
    {
        public event Action<GameObject> ObjectReturned;

        protected Queue<GameObject> _pool = new Queue<GameObject>();

        private bool _valid = true;

        public bool IsValid
        {
            get
            {
                return _valid;
            }
        }

        /// <summary>
        /// Check if the pool is valid and if it isn't, throw a
        /// <see cref="System.InvalidOperationException"/>.
        /// </summary>
        protected void ExpectValidPool()
        {
            if (!_valid)
            {
                throw new System.InvalidOperationException(
                    "Invalid object pool"
                );
            }
        }

        /// <summary>
        /// Mark the pool as invalid, preventing any further use.
        /// </summary>
        protected void Invalidate()
        {
            ExpectValidPool();
            _valid = false;
        }

        internal void Release(GameObject obj)
        {
            obj.SetActive(false);

            var pooledObject = obj.GetComponent<PooledGameObject>();
            if (pooledObject != null)
            {
                pooledObject.OnReleasedToPool();
            }

            if (ObjectReturned != null) ObjectReturned(obj);

            _pool.Enqueue(obj);
        }
    }
}
