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

using System.Collections.Generic;

namespace mtti.Funcs
{
    /// <summary>
    /// Methods that extend <see cref="System.Collections.Generic.List{T}"/>.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Append the contents of this list to another.
        /// </summary>
        /// <remarks>
        /// Does the same thing as
        /// <see cref="System.Collections.Generic.List{T}.AddRange(IEnumerable{T})"/>
        /// but without allocating an enumerator.
        /// </remarks>
        public static void AppendTo<T>(
            this List<T> self,
            List<T> other
        )
        {
            for (int i = 0, count = self.Count; i < count; i++)
            {
                other.Add(self[i]);
            }
        }
    }
}
