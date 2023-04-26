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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace mtti.Funcs
{
    public static class SceneExtensions
    {
        private static List<GameObject> s_gameObjects;

        /// <summary>
        /// Find an object of a specific type in the root of a scene.
        /// </summary>
        public static T FindRootObject<T>(
            this Scene scene
        ) where T : Component
        {
            try
            {
                if (s_gameObjects == null)
                {
                    s_gameObjects = new List<GameObject>();
                }

                scene.GetRootGameObjects(s_gameObjects);

                for (int i = 0, count = s_gameObjects.Count; i < count; i++)
                {
                    T result = s_gameObjects[i].GetComponent<T>();
                    if (result != null)
                    {
                        return result;
                    }
                }
                return null;
            }
            finally
            {
                s_gameObjects.Clear();
            }
        }

        /// <summary>
        /// Same as <see cref="mtti.Funcs.UnityUtils.FindRootObject{T}(Scene)" />
        /// but throw a <see cref="System.InvalidOperationException" /> if
        /// no object is found.
        /// </summary>
        public static T RequireRootObject<T>(
            this Scene scene
        ) where T : Component
        {
            T result = scene.FindRootObject<T>();
            if (result == null)
            {
                throw new InvalidOperationException(
                    $"Could not find a root object of type {typeof(T).Name} in scene {scene.name}"
                );
            }
            return result;
        }
    }
}
