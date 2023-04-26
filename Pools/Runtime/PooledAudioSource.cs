/*
Copyright 2017-2022 Matti Hiltunen (https://www.mattihiltunen.com)

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
    public class PooledAudioSource : MonoBehaviour
    {
        private Transform _transform;

        private AudioSource _audioSource;

        private AudioSource Source
        {
            get
            {
                if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
                return _audioSource;
            }
        }

        public void Play(Vector3 position, AudioClip clip)
        {
            Source.clip = clip;
            Play(position);
        }

        public void Play(Vector3 position)
        {
            this.transform.position = position;
            this.gameObject.SetActive(true);
            Source.Play();
        }

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!Source.isPlaying) PooledGameObject.Release(this);
        }
    }
}
