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

#if USE_ADDRESSABLES
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

#if USE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace mtti.Pools
{
    /// <summary>
    /// A GameObject pool backed by the Addressable Asset System.
    /// </summary>
    public abstract class AddressablePool : BaseGameObjectPool
    {
        public static AddressablePool Create(string address)
        {
            return new StringAddressablePool(address);
        }

        public static AddressablePool Create(IResourceLocation location)
        {
            return new LocationAddressablePool(location);
        }

        public static AddressablePool Create(AssetReference reference)
        {
            return new ReferenceAddressablePool(reference);
        }

#if USE_UNITASK
        public async UniTask<T> Claim<T>() where T : Component
#else
        public async Task<T> Claim<T>() where T : Component
#endif
        {
            ExpectValidPool();

            var obj = await Claim();
            if (obj == null) return null;
            return obj.GetComponent<T>();
        }

#if USE_UNITASK
        public async UniTask<GameObject> Claim()
#else
        public async Task<GameObject> Claim()
#endif
        {
            ExpectValidPool();

            var obj = await GetInstance();

            var pooledObject = PooledGameObject.AddTo(obj);
            pooledObject.Pool = this;
            pooledObject.OnClaimedFromPool();

            return obj;
        }

        protected abstract AsyncOperationHandle<GameObject> Instantiate();

        protected abstract void DestroyInstance(GameObject instance);

        /// <summary>
        /// Pre-allocate the pool so it has at least <c>minCount</c> instances.
        /// </summary>
#if USE_UNITASK
        public async UniTask<int> Allocate(int minCount)
#else
        public async Task<int> Allocate(int minCount)
#endif
        {
            ExpectValidPool();

            int createdCount = 0;

            while (_pool.Count < minCount)
            {
                var obj = await CreateNew();
                _pool.Enqueue(obj);
                createdCount += 1;
            }

            return createdCount;
        }

        /// <summary>
        /// Destroy all objects currently in the pool.
        /// </summary>
        public void Clear()
        {
            Prune(0);
        }

        public int Prune(int maxCount)
        {
            ExpectValidPool();

            if (maxCount < 0) maxCount = 0;

            int prunedCount = 0;
            while (_pool.Count > maxCount)
            {
                var obj = _pool.Dequeue();
                if (obj == null) continue;

                DestroyInstance(obj);

                prunedCount += 1;
            }

            return prunedCount;
        }

        /// <summary>
        /// Destroy all pooled objects and mark the pool as invalid.
        /// Claimed objects originally created by this pool are not destroyed,
        /// but will instead be destroyed when they're released.
        /// </summary>
        public void Dispose()
        {
            if (!IsValid) return;
            Prune(0);
            Invalidate();
        }

#if USE_UNITASK
        private async UniTask<GameObject> GetInstance()
#else
        private async Task<GameObject> GetInstance()
#endif
        {
            GameObject obj = null;
            while (_pool.Count > 0 && obj == null)
            {
                obj = _pool.Dequeue();
            }

            if (obj == null)
            {
                return await CreateNew();
            }

            return obj;
        }

#if USE_UNITASK
        private async UniTask<GameObject> CreateNew()
#else
        private async Task<GameObject> CreateNew()
#endif
        {
            await UniTask.NextFrame();

            var result = Instantiate();
            await result.Task;
            if (result.Status != AsyncOperationStatus.Succeeded)
            {
                return null;
            }

            var obj = result.Result;
            obj.SetActive(false);
            var pooledObject = PooledGameObject.AddTo(obj);
            pooledObject.Pool = this;

            return obj;
        }
    }
}
#endif // USE_ADDRESSABLES
