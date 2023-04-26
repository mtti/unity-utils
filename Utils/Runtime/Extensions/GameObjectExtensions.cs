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

using System;
using UnityEngine;

namespace mtti.Funcs
{
    public static class GameObjectExtensions
    {
        public static T EnsureComponent<T>(
            this GameObject gameObject
        ) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// Find a child GameObject by name.
        /// </summary>
        public static GameObject FindChildByName(
            this GameObject self,
            string name
        )
        {
            if (self.name == name) return self;

            var transform = self.GetComponent<Transform>();
            for (int i = 0, count = transform.childCount; i < count; i++)
            {
                var result = transform.GetChild(i).FindChildByName(name);
                if (result != null) return result.gameObject;
            }

            return null;
        }

        /// <summary>
        /// Find a component of a child by the child GameObject's name.
        /// </summary>
        public static T FindChildByName<T>(
            this GameObject self,
            string name
        ) where T : Component
        {
            var obj = self.FindChildByName(name);
            if (obj == null) return null;
            return obj.GetComponent<T>();
        }

        /// <summary>
        /// Find a component of a child by the child GameObject's name, or
        /// throw an exception if not found.
        /// </summary>
        public static T RequireChildByName<T>(
            this GameObject self,
            string name
        ) where T : Component
        {
            var result = self.FindChildByName<T>(name);
            if (result == null)
            {
                throw new InvalidOperationException($"GameObject {self.name} has no child called {name} with the type {typeof(T).Name}");
            }
            return result;
        }

        /// <summary>
        /// Get a component and throw an exception if the GameObject doesn't
        /// have one.
        /// </summary>
        public static T RequireComponent<T>(
            this GameObject self
        ) where T : Component
        {
            T component = self.GetComponent<T>();
            if (component == null)
            {
                throw new InvalidOperationException($"GameObject {self.name} has no {typeof(T).Name} component");
            }
            return component;
        }

        public static T RequireComponentInChildren<T>(
            this GameObject self
        ) where T : Component
        {
            var result = self.GetComponentInChildren<T>(true);
            if (result == null)
            {
                throw new InvalidOperationException($"No child of {self.name} has the component {typeof(T).Name}");
            }
            return result;
        }

        /// <summary>
        /// Recursively set the layer of a GameObject and all it's children.
        /// </summary>
        public static void SetLayerRecursively(this GameObject self, int layer)
        {
            self.layer = layer;
            for (int i = 0, count = self.transform.childCount; i < count; i++)
            {
                SetLayerRecursively(self.transform.GetChild(i).gameObject, layer);
            }
        }
    }
}
