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
using System.Reflection;

namespace mtti.Funcs
{
    public static class ReflectionUtils
    {
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
    }
}
