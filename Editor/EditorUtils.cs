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
using System.Reflection;
using System.Text;
using System.IO;
using UnityEngine;

namespace mtti.Funcs.Editor
{
    /// <summary>
    /// Utilities for use in the Unity editor.
    /// </summary>
    public static class EditorUtils
    {
        /// <summary>
        /// Get the value of a field. Intended to be used for unit testing.
        /// </summary>
        public static object GetFieldValue(object target, string fieldName)
        {
            Type targetType = target.GetType();
            FieldInfo fieldInfo = targetType.GetField(
                fieldName,
                BindingFlags.Instance | BindingFlags.Public
                    | BindingFlags.NonPublic
            );
            return fieldInfo.GetValue(target);
        }

        /// <summary>
        /// Set the value of field. Intended to be used for unit testing.
        /// </summary>
        public static void SetFieldValue(
            object target,
            string fieldName,
            object value
        )
        {
            Type targetType = target.GetType();
            FieldInfo fieldInfo = targetType.GetField(
                fieldName,
                BindingFlags.Instance | BindingFlags.Public
                    | BindingFlags.NonPublic
            );
            fieldInfo.SetValue(target, value);
        }

        /// <summary>
        /// Set the value of a field, but throw an exception if it doesn't have
        /// the <see cref="UnityEngine.SerializeField" /> attribute.
        /// </summary>
        public static void SetSerializedField(
            object target,
            string fieldName,
            object value
        )
        {
            Type targetType = target.GetType();
            FieldInfo fieldInfo = targetType.GetField(
                fieldName,
                BindingFlags.Instance | BindingFlags.Public
                    | BindingFlags.NonPublic
            );

            object[] attributes = fieldInfo.GetCustomAttributes(
                typeof(UnityEngine.SerializeField),
                true
            );
            if (attributes.Length == 0)
            {
                throw new ArgumentException("Field does not have the [SerializeField] attribute");
            }

            fieldInfo.SetValue(target, value);
        }

        /// <summary>
        /// Get an absolute filesystem path to a location under the assets
        /// directory using the platform-dependent path separator character.
        /// </summary>
        public static string GetAbsoluteAssetPath(string assetPath)
        {
            return GetAbsoluteAssetPath(assetPath, Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Get an absolute filesystem path to a location under the assets directory.
        /// </summary>
        public static string GetAbsoluteAssetPath(
            string assetPath,
            char resultSeparator
        )
        {
            string[] dataPathParts = Application.dataPath.Split('/');
            string[] assetPathParts = assetPath.Split('/');

            string[] resultParts = new string[
                (dataPathParts.Length + assetPathParts.Length) - 1
            ];
            for (int i = 0; i < dataPathParts.Length - 1; i++)
            {
                resultParts[i] = dataPathParts[i];
            }

            for (int i = 0; i < assetPathParts.Length; i++)
            {
                resultParts[i + (dataPathParts.Length - 1)] = assetPathParts[i];
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < resultParts.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(resultSeparator);
                }
                sb.Append(resultParts[i]);
            }
            return sb.ToString();
        }
    }
}
