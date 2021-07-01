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
    public static class Vector3Extensions
    {
        /// <summary>
        /// Copy the Vector3 but replace the <c>x</c> property.
        /// </summary>
        public static Vector3 WithX(this Vector3 self, float x)
        {
            return new Vector3(
                x,
                self.y,
                self.z
            );
        }

        /// <summary>
        /// Copy the Vector3 but replace the <c>y</c> property.
        /// </summary>
        public static Vector3 WithY(this Vector3 self, float y)
        {
            return new Vector3(
                self.x,
                y,
                self.z
            );
        }

        /// <summary>
        /// Copy the Vector3 but replace the <c>z</c> property.
        /// </summary>
        public static Vector3 WithZ(this Vector3 self, float z)
        {
            return new Vector3(
                self.x,
                self.y,
                z
            );
        }
    }
}
