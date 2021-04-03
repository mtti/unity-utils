/*
Copyright 2017-2021 Matti Hiltunen

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

namespace mtti.Funcs
{
    public static class ComponentExtensions
    {
        public static T EnsureComponent<T>(
            this Component self
        ) where T : Component
        {
            return self.gameObject.EnsureComponent<T>();
        }

        public static T RequireComponent<T>(
            this Component self
        ) where T : Component
        {
            return self.gameObject.RequireComponent<T>();
        }

        public static T RequireComponentInChildren<T>(
            this Component self
        ) where T : Component
        {
            return self.gameObject.RequireComponentInChildren<T>();
        }

        /// <summary>
        /// Recursively set the layer of a GameObject and all it's children.
        /// </summary>
        public static void SetLayerRecursively(this Component self, int layer)
        {
            self.gameObject.SetLayerRecursively(layer);
        }
    }
}
