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
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace mtti.Funcs
{
    public static class Utils
    {
        public static string FormatArray(object[] subject)
        {
            var sb = new StringBuilder();

            sb.Append("[");
            for (var i = 0; i < subject.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(", ");
                }
                sb.Append(subject[i]);
            }
            sb.Append("]");

            return sb.ToString();
        }

        public static string FormatList<T>(List<T> subject)
        {
            var sb = new StringBuilder();

            sb.Append("[");
            for (int i = 0, count = subject.Count; i < count; i++)
            {
                if (i > 0)
                {
                    sb.Append(", ");
                }
                sb.Append(subject[i]);
            }
            sb.Append("]");

            return sb.ToString();
        }

        public static string FormatFileSize(double bytes)
        {
            if (bytes < 1000)
            {
                return string.Format("{0} bytes", bytes);
            }
            else if (bytes < 1000000)
            {
                return string.Format("{0} kilobytes", bytes / 1000);
            }
            else if (bytes < 1000000000)
            {
                return string.Format("{0} megabytes", bytes / 1000000);
            }
            else
            {
                return string.Format("{0} gigabytes", bytes / 1000000000);
            }
        }

        public static string[] SplitPath(string path)
        {
            return SplitPath(path, Path.DirectorySeparatorChar);
        }

        public static string[] SplitPath(string path, char separator)
        {
            return path.Split(separator);
        }

        public static string GetLastPathElement(string path, char separator)
        {
            var parts = SplitPath(path, separator);
            return parts[parts.Length - 1];
        }

        public static string GetRelativePath(string fromPath, string toPath)
        {
            return GetRelativePath(fromPath, toPath, Path.DirectorySeparatorChar);
        }

        public static string GetRelativePath(
            string fromPath,
            string toPath,
            char separator
        )
        {
            var fromParts = SplitPath(fromPath, separator);
            var toParts = SplitPath(toPath, separator);
            var newParts = new List<string>();

            int lastMatchingIndex = -1;
            for (
                int i = 0, count = Math.Min(fromParts.Length, toParts.Length);
                i < count;
                i++
            )
            {
                if (fromParts[i] == toParts[i])
                {
                    lastMatchingIndex = i;
                }
                else
                {
                    break;
                }
            }

            if (lastMatchingIndex == -1)
            {
                return fromPath;
            }

            int backtrackLevels = fromParts.Length - (lastMatchingIndex + 1);

            for (var i = 0; i < backtrackLevels; i++)
            {
                newParts.Add("..");
            }
            for (var i = lastMatchingIndex + 1; i < toParts.Length; i++)
            {
                newParts.Add(toParts[i]);
            }

            var sb = new StringBuilder();
            for (int i = 0, count = newParts.Count; i < count; i++)
            {
                if (i > 0)
                {
                    sb.Append(separator);
                }
                sb.Append(newParts[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Finds all types in the current application domain which have
        /// a specific attribute.
        /// </summary>
        public static void FindAllTypesWithAttribute(
            Type attributeType,
            List<Type> result
        )
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                var types = assemblies[i].GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    var attribute = Attribute.GetCustomAttribute(
                        types[j],
                        attributeType,
                        false
                    );
                    if (attribute != null)
                    {
                        result.Add(types[j]);
                    }
                }
            }
        }

        /// <summary>
        /// Finds all types in the current application domain which are
        /// assignable to a type. This can be used to find all classes
        /// implementing an interface, for example.
        /// </summary>
        public static void FindAllTypesAssignableTo(
            Type targetType,
            List<Type> result
        )
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                var types = assemblies[i].GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    if (targetType.IsAssignableFrom(types[j]))
                    {
                        if (targetType == types[j]) continue;
                        result.Add(types[j]);
                    }
                }
            }
        }

        /// <summary>
        /// Get all methods in a type which have the specified attribute.
        /// </summary>
        /// <param name="targetType">Target type.</param>
        /// <param name="attributeType">Attribute type.</param>
        /// <param name="result">Result.</param>
        public static void GetMethodsWithAttribute(
            Type targetType,
            Type attributeType,
            List<MethodInfo> result)
        {
            var methods = targetType.GetMethods(
                BindingFlags.Instance | BindingFlags.Public
                    | BindingFlags.NonPublic
            );
            for (int i = 0; i < methods.Length; i++)
            {
                var attribute = Attribute.GetCustomAttribute(
                    methods[i],
                    attributeType,
                    true
                );
                if (attribute != null)
                {
                    result.Add(methods[i]);
                }
            }
        }

        /// <summary>
        /// Find all static methods in the current application domain with an
        /// attribute.
        /// </summary>
        /// <param name="attributeType">Attribute type.</param>
        /// <param name="result">Result.</param>
        public static void GetStaticMethodsWithAttribute(
            Type attributeType,
            List<MethodInfo> result
        )
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                var types = assemblies[i].GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    GetStaticMethodsWithAttribute(
                        types[j],
                        attributeType,
                        result
                    );
                }
            }
        }

        /// <summary>
        /// Get all methods in a type which have the specified attribute.
        /// </summary>
        /// <param name="targetType">Target type.</param>
        /// <param name="attributeType">Attribute type.</param>
        /// <param name="result">Result.</param>
        public static void GetStaticMethodsWithAttribute(
            Type targetType,
            Type attributeType,
            List<MethodInfo> result
        )
        {
            var methods = targetType.GetMethods(
                BindingFlags.Static | BindingFlags.Public
                    | BindingFlags.NonPublic
            );
            for (int i = 0; i < methods.Length; i++)
            {
                var attribute = Attribute.GetCustomAttribute(
                    methods[i],
                    attributeType,
                    true
                );
                if (attribute != null)
                {
                    result.Add(methods[i]);
                }
            }
        }

        /// <summary>
        /// Compare two floating point numbers to see if they're nearly equal.
        /// </summary>
        /// <remarks>
        /// Ported from <a href="http://floating-point-gui.de/errors/comparison/">
        /// The Floating Point Guide</a>, under CC-BY.
        /// </remarks>
        public static bool NearlyEqual(float a, float b, float epsilon)
        {
            float difference = Math.Abs(a - b);

            if (a == b)
            {
                return true;
            }
            else if (a == 0 || b == 0 || difference < Single.MinValue)
            {
                return difference < (epsilon * Single.MinValue);
            }
            else
            {
                return difference / (Math.Abs(a) + Math.Abs(b)) < epsilon;
            }
        }

        public static float ReverseLerp(float value, float a, float b)
        {
            return (value - a) / (b - a);
        }

        public static float Lerp(float a, float b, float t)
        {
            return ((1.0f - t) * a) + (b * t);
        }

        public static float Remap(
            float value,
            float a1,
            float b1,
            float a2,
            float b2
        )
        {
            return Lerp(
                a2,
                b2,
                ReverseLerp(value, a1, b1)
            );
        }
    }
}
