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
    /// <summary>
    /// Object pool for Unity GameObjects.
    /// </summary>
    public class GameObjectPool : BaseGameObjectPool
    {
        protected GameObject _prefab = null;

        private Func<GameObject> _factoryDelegate;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="mtti.Pools.GameObjectPool"/> class with
        /// a factory method which will be called every time the pool needs to
        /// create a new object.
        /// </summary>
        /// <param name="factory">
        /// Callback to create new GameObjects when the pool is empty.
        /// </param>
        public GameObjectPool(Func<GameObject> factory)
        {
            _factoryDelegate = factory;
        }

        public GameObjectPool(Component component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }

            _prefab = component.gameObject;
            _factoryDelegate = InstantiatePrefab;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="mtti.Pools.GameObjectPool"/> class with
        /// a prefab, which will be cloned every time the pool needs to create
        /// a new object.
        /// </summary>
        /// <param name="prefab">Prefab.</param>
        public GameObjectPool(GameObject prefab)
        {
            if (prefab == null)
            {
                throw new ArgumentNullException("prefab");
            }

            _prefab = prefab;
            _factoryDelegate = InstantiatePrefab;
        }

        /// <summary>
        /// Retrieve a GameObject from the pool, or create a new one using
        /// the factory delegate if the pool is empty.
        /// </summary>
        public GameObject Claim()
        {
            GameObject obj = null;

            // Destroyed GameObjects will equal to null, so keep trying until we
            // get a non-null object or the pool runs out.
            while (_pool.Count > 0 && obj == null)
            {
                obj = _pool.Dequeue();
            }

            PooledGameObject pooledObject;

            // If we don't have a valid GameObject by now, create one.
            if (obj == null)
            {
                obj = Create();

                pooledObject = obj.GetComponent<PooledGameObject>();
                if (pooledObject == null)
                {
                    pooledObject = obj.AddComponent<PooledGameObject>();
                }
                pooledObject.Pool = this;
            }

            obj.SetActive(false);

            pooledObject = obj.GetComponent<PooledGameObject>();
            if (pooledObject != null)
            {
                pooledObject.OnClaimedFromPool();
            }

            return obj;
        }

        public T Claim<T>() where T : Component
        {
            GameObject obj = Claim();
            if (obj == null)
            {
                return null;
            }

            return obj.GetComponent<T>();
        }

        /// <summary>
        /// Destroy all GameObjects currently in the pool.
        /// </summary>
        public void Clear()
        {
            Prune(0);
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
                var obj = _pool.Dequeue();
                if (obj == null) continue;
                UnityEngine.Object.Destroy(obj);
                prunedCount += 1;
            }

            return prunedCount;
        }

        protected virtual GameObject Create()
        {
            return _factoryDelegate();
        }

        /// <summary>
        /// Used in place of a user-provided factory method when a prefab is
        /// provided instead.
        /// </summary>
        /// <returns>A copy of the prefab.</returns>
        private GameObject InstantiatePrefab()
        {
            return GameObject.Instantiate(_prefab);
        }
    }
}
