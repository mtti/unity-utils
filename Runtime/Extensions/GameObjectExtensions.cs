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
    }
}
