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

using UnityEngine;

namespace mtti.Pools
{
    /// <summary>
    /// A specialized <see cref="mtti.Pools.GameObjectPool" /> for particle
    /// systems. Every created GameObject instance is given a
    /// PooledParticleSystem component which automatically releases the
    /// GameObject back into the pool after the particle system has finished
    /// playing.
    /// </summary>
    public class ParticleSystemPool : GameObjectPool
    {
        public ParticleSystemPool(
            ParticleSystem prefab
        ) : base(prefab.gameObject) { }

        protected override GameObject Create()
        {
            GameObject obj = GameObject.Instantiate(_prefab.gameObject);
            obj.AddComponent<PooledParticleSystem>();
            return obj;
        }
    }
}
