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

namespace mtti.Funcs
{
    /// <summary>
    /// Value type representing speed. The value is stored internally as
    /// meters per second and methods are provided for the most common
    /// conversions. Equality uses an approximate comparison.
    /// </summary>
    public struct Speed : IEquatable<Speed>, IComparable<Speed>
    {
        public const float Ms2Kmh = 3.6f;

        public const float Ms2Mph = 2.237f;

        public const float Ms2Feet = 3.281f;

        public static Speed operator +(
            Speed lhs,
            Speed rhs
        )
        {
            return new Speed(lhs.AsMetersPerSecond + rhs.AsMetersPerSecond);
        }

        public static Speed operator -(
            Speed lhs,
            Speed rhs
        )
        {
            return new Speed(lhs.AsMetersPerSecond - rhs.AsMetersPerSecond);
        }

        public static bool operator ==(
            Speed lhs,
            Speed rhs
        )
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(
            Speed lhs,
            Speed rhs
        )
        {
            return !lhs.Equals(rhs);
        }

        public static Speed FromKilometersPerHour(float value)
        {
            return new Speed(value / Ms2Kmh);
        }

        public static Speed FromMilesPerHour(float value)
        {
            return new Speed(value / Ms2Mph);
        }

        /// <summary>
        /// Value as meters per second.
        /// </summary>
        public readonly float AsMetersPerSecond;

        /// <summary>
        /// Value as kilometers per hour.
        /// </summary>
        public float AsKilometersPerHour
        {
            get { return AsMetersPerSecond * Ms2Kmh; }
        }

        /// <summary>
        /// Value as miles per hour.
        /// </summary>
        public float AsMilesPerHour
        {
            get { return AsMetersPerSecond * Ms2Mph; }
        }

        /// <summary>
        /// Approximate value as feet per second.
        /// </summary>
        public float AsFeetPerSecond
        {
            get { return AsMetersPerSecond * Ms2Feet; }
        }

        public Speed(float metersPerSecond)
        {
            AsMetersPerSecond = metersPerSecond;
        }

        public int CompareTo(Speed other)
        {
            return AsMetersPerSecond.CompareTo(other.AsMetersPerSecond);
        }

        public override bool Equals(object other)
        {
            if (!(other is Speed)) return false;
            var speed = (Speed)other;
            return Equals(speed);
        }

        public bool Equals(Speed other)
        {
            return AsMetersPerSecond
                .ApproximatelyEquals(other.AsKilometersPerHour);
        }

        public override int GetHashCode()
        {
            return AsMetersPerSecond.GetHashCode();
        }

        public override string ToString()
        {
            return $"{AsMetersPerSecond} m/s";
        }

        public string ToStringKilometersPerHour()
        {
            var rounded = UnityEngine.Mathf.RoundToInt(AsKilometersPerHour);
            return $"{rounded} km/h";
        }

        public string ToStringMilesPerHour()
        {
            var rounded = UnityEngine.Mathf.RoundToInt(AsMilesPerHour);
            return $"{rounded} mph";
        }
    }
}
