/*
Copyright 2017-2020 Matti Hiltunen

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
using IEnumerator = System.Collections.IEnumerator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace mtti.Funcs
{
    /// <summary>
    /// Unity-specific utility functions.
    /// </summary>
    public static class UnityUtils
    {
        private static List<GameObject> s_gameObjects = new List<GameObject>();

        /// <summary>
        /// Check if project is currently in the Unity Editor and not playing. Also callable during
        /// runtime, when it will always return <c>false</c>.
        /// </summary>
        public static bool IsInEditMode
        {
            get
            {
#if UNITY_EDITOR
                return !UnityEditor.EditorApplication.isPlaying;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Instantiates a prefab. At runtime, this just calls <c>GameObject.Instantiate</c>,
        /// but uses <c>PrefabUtility</c> in edit mode to preserve the prefab connection.
        /// </summary>
        public static GameObject InstantiatePrefab(GameObject prefab)
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                return (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefab);
            }
#endif
            return GameObject.Instantiate(prefab);
        }

        /// <summary>
        /// Instantiates a prefab also maintaining the prefab connection in edit mode. This generic
        /// version receives and returns a component of the prefab and instance respectively.
        /// </summary>
        public static T InstantiatePrefab<T>(T prefab) where T : Component
        {
            GameObject obj = InstantiatePrefab(prefab.gameObject);
            return obj.GetComponent<T>();
        }

        public static T EnsureComponent<T>(GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        public static T EnsureComponent<T>(Component parent) where T : Component
        {
            return EnsureComponent<T>(parent.gameObject);
        }

        /// <summary>
        /// Find an object of a specific type in the root of a scene.
        /// </summary>
        public static T FindRootObject<T>(Scene scene) where T : Component
        {
            try
            {
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
        /// Find a root object of a specific type in any loaded scene.
        /// </summary>
        public static T FindRootObject<T>() where T : Component
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                T obj = FindRootObject<T>(scene);
                if (obj != null)
                {
                    return obj;
                }
            }
            return null;
        }

        /// <summary>
        /// Find all scene root objects of a specific type.
        /// </summary>
        public static int FindRootObjects<T>(List<T> result) where T : Component
        {
            int count = 0;
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                T obj = FindRootObject<T>(scene);
                if (obj == null)
                {
                    continue;
                }

                count++;
                if (result != null)
                {
                    result.Add(obj);
                }
            }

            return count;
        }

        public static bool IsSceneLoaded(string path)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.path == path)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool GetLoadedScene(string path, out Scene result)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.path == path)
                {
                    result = scene;
                    return true;
                }
            }
            result = new Scene();
            return false;
        }

        /// <summary>
        /// Load a scene additively if it's not already loaded.
        /// </summary>
        public static IEnumerator LoadScene(string scenePath)
        {
            if (UnityUtils.IsSceneLoaded(scenePath))
            {
                Debug.LogFormat("Already loaded: {0}", scenePath);
            }
            else
            {
                yield return SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
            }
        }
    }
}
