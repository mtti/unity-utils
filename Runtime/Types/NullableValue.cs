/*
Copyright 2022 Matti Hiltunen (https://www.mattihiltunen.com)

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
    /// <summary>
    /// Utilty class for creating <see cref="mtti.Funcs.NullableValue{T}" />s.
    /// </summary>
    public static class NullableValue
    {
        public static NullableValue<T> From<T>(T value) where T : struct
        {
            return new NullableValue<T>(value);
        }
    }

    /// <summary>
    /// A nullable value type which works with Unity's native containers
    /// and inspector, unlike <see cref="System.Nullable{T}" />.
    /// </summary>
    [Serializable]
    public struct NullableValue<T> : IEquatable<NullableValue<T>>
        where T : struct
    {
        public static bool operator ==(
            NullableValue<T> first,
            NullableValue<T> second
        )
        {
            return first.Equals(second);
        }

        public static bool operator !=(
            NullableValue<T> first,
            NullableValue<T> second
        )
        {
            return !first.Equals(second);
        }

        public static implicit operator NullableValue<T>(T value)
        {
            return new NullableValue<T>(value);
        }

        [SerializeField]
        private bool _hasValue;

        [SerializeField]
        private T _value;

        public bool HasValue { get { return _hasValue; } }

        public T Value
        {
            get
            {
                if (!_hasValue)
                {
                    throw new InvalidOperationException("Value not found");
                }
                return _value;
            }
        }

        public NullableValue(T value)
        {
            _hasValue = true;
            _value = value;
        }

        public bool TryGetValue(out T value)
        {
            if (_hasValue)
            {
                value = _value;
                return true;
            }

            value = default(T);
            return false;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is NullableValue<T>))
            {
                return false;
            }

            var other = (NullableValue<T>)obj;
            return ((IEquatable<NullableValue<T>>)this).Equals(other);
        }

        public override int GetHashCode()
        {
            if (!_hasValue) return 0;
            return _value.GetHashCode();
        }

        bool IEquatable<NullableValue<T>>.Equals(NullableValue<T> other)
        {
            if (!_hasValue && !other._hasValue) return true;
            if (_hasValue != other._hasValue) return false;
            return _value.Equals(other._value);
        }
    }
}
