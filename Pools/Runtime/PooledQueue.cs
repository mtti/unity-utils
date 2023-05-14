/*
Copyright 2017-2023 Matti Hiltunen (https://www.mattihiltunen.com)

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

namespace mtti.Pools
{
    public class PooledQueue<T>: Queue<T>, IDisposable
    {
        public static PooledQueue<T> Claim()
        {
            return ObjectPool<PooledQueue<T>>.Instance.Claim();
        }

        private byte _version = 0;
        
        public byte Version
        {
            get { return _version; }
        }
        
        public void Dispose()
        {
            unchecked { _version++; }
            Clear();
            ObjectPool<PooledQueue<T>>.Instance.Release(this);
        }
    }
}
