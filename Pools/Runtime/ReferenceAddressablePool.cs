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

namespace mtti.Pools
{
    /// <summary>
    /// An addressable pool which references the source asset with an asset
    /// reference.
    /// </summary>
    /// <remarks>
    /// You shouldn't need to create these manually. Use
    /// <see cref="mtti.Pools.AddressablePool.Create(AssetReference)" />
    /// instead.
    /// </remarks>
    public class ReferenceAddressablePool : AddressablePool
    {
        private AssetReference _reference;

        public ReferenceAddressablePool(AssetReference reference)
        {
            _reference = reference;
        }

        protected override AsyncOperationHandle<GameObject> Instantiate()
        {
            return _reference.InstantiateAsync();
        }

        protected override void DestroyInstance(GameObject instance)
        {
            _reference.ReleaseInstance(instance);
        }
    }
}
#endif
