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
    /// When added to a GameObject with both a ParticleSystem and PooledObject
    /// components, releases the GameObject back into PooledObject's pool when
    /// the particle system finishes playing.
    /// </summary>
    public class PooledParticleSystem : MonoBehaviour
    {
        private Transform _transform;

        private ParticleSystem _particleSystem;

        public void Initialize(Vector3 position, Vector3 direction)
        {
            _transform.position = position;
            _transform.rotation = Quaternion.LookRotation(direction);
            this.gameObject.SetActive(true);
            _particleSystem.Play();
        }

        private void Release()
        {
            GetComponent<PooledGameObject>().Release();
        }

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (!_particleSystem.IsAlive())
            {
                Release();
            }
        }
    }
}
