/*
Copyright 2021 Matti Hiltunen (https://www.mattihiltunen.com)

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
    [Serializable]
    public struct Layer
    {
        /// <summary>
        /// Conversion operator to allow setting Layer fields by string name
        /// of the layer. Will throw an exception if the layer does not exist.
        /// </summary>
        public static implicit operator Layer(string name)
        {
            return new Layer(name);
        }

        /// <summary>
        /// Conversion operator to allow setting Layer fields directly from
        /// an <c>int</c>. Will throw an exception if the index is out of
        /// range.
        /// </summary>
        public static implicit operator Layer(int value)
        {
            return new Layer(value);
        }

        private static void ExpectValidIndex(int index)
        {
            if (index < 0 || index > 31)
            {
                throw new ArgumentOutOfRangeException(
                    "index",
                    "Layer index must be from 0 to 31"
                );
            }
        }

        [SerializeField]
        private int _index;

        /// <summary>
        /// Get the index of this layer.
        /// </summary>
        public int Index
        {
            get { return _index; }
        }

        /// <summary>
        /// Get a layer mask with only this layer.
        /// </summary>
        public LayerMask Mask
        {
            get { return 1 << _index; }
        }

        public Layer(int index)
        {
            ExpectValidIndex(index);
            _index = index;
        }

        /// <summary>
        /// Create a new Layer object from the string name of the layer.
        /// This will throw an exception if the layer doesn't exist. In the
        /// editor, you can use <see cref="mtti.Funcs.Editor.EditorUtils.CreateLayer()"/>
        /// to create a new layer.
        /// </summary>
        public Layer(string name)
        {
            var index = LayerMask.NameToLayer(name);
            if (index == -1) throw new ArgumentException(
                $"No layer by the name {name} found"
            );
            ExpectValidIndex(index);
            _index = index;
        }

        public override string ToString()
        {
            return LayerMask.LayerToName(_index);
        }
    }
}
