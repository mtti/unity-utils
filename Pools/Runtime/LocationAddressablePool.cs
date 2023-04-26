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

namespace mtti.Pools
{
    /// <summary>
    /// An addressable pool which references the source asset by an
    /// <see cref="UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation" />.
    /// </summary>
    /// <remarks>
    /// You shouldn't need to create these manually. Use
    /// <see cref="mtti.Pools.AddressablePool.Create(string)" />
    /// instead.
    /// </remarks>
    public class LocationAddressablePool : AddressablePool
    {
        private IResourceLocation _location;

        public LocationAddressablePool(IResourceLocation location)
        {
            _location = location;
        }

        protected override AsyncOperationHandle<GameObject> Instantiate()
        {
            return Addressables.InstantiateAsync(_location);
        }

        protected override void DestroyInstance(GameObject instance)
        {
            if (!Addressables.ReleaseInstance(instance))
            {
                UnityEngine.Object.Destroy(instance);
            }
        }
    }
}
#endif // USE_ADDRESSABLES
