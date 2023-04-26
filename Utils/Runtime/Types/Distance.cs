/*
Copyright 2017-2023 Matti Hiltunen

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
    /// Value type representing distance. The value is stored internally as
    /// meters and methods are provided for the most common conversions.
    /// Equality uses an approximate comparison.
    /// </summary>
    public struct Distance : IEquatable<Distance>, IComparable<Distance>
    {
        public const float KilometersToMeters = 1000.0f;

        public const float MilesToMeters = 1609.0f;

        public const float InchesToMeters = 0.0254f;

        public static Distance operator +(
            Distance lhs,
            Distance rhs
        )
        {
            return new Distance(lhs.AsMeters + rhs.AsMeters);
        }

        public static Distance operator -(
            Distance lhs,
            Distance rhs
        )
        {
            return new Distance(lhs.AsMeters - rhs.AsMeters);
        }

        public static bool operator ==(
            Distance lhs,
            Distance rhs
        )
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(
            Distance lhs,
            Distance rhs
        )
        {
            return !lhs.Equals(rhs);
        }

        public static Distance FromKilometers(float value)
        {
            return new Distance(value * KilometersToMeters);
        }

        public static Distance FromMiles(float value)
        {
            return new Distance(value * MilesToMeters);
        }

        public static Distance FromInches(float value)
        {
            return new Distance(value * InchesToMeters);
        }

        /// <summary>
        /// Value in meters.
        /// </summary>
        public readonly float AsMeters;

        /// <summary>
        /// Value in kilometers.
        /// </summary>
        public float AsKilometers
        {
            get { return AsMeters / KilometersToMeters; }
        }

        /// <summary>
        /// Approximate value in miles.
        /// </summary>
        public float AsMiles
        {
            get { return AsMeters / MilesToMeters; }
        }

        /// <summary>
        /// Approximate value in feet.
        /// </summary>
        public float AsFeet
        {
            get { return AsInches / 12.0f; }
        }

        /// <summary>
        /// Approximate value in inches.
        /// </summary>
        public float AsInches
        {
            get { return AsMeters / InchesToMeters; }
        }

        public Distance(float metersPerSecond)
        {
            AsMeters = metersPerSecond;
        }

        public int CompareTo(Distance other)
        {
            return AsMeters.CompareTo(other.AsMeters);
        }

        public override bool Equals(object other)
        {
            if (!(other is Distance))
            {
                return false;
            }
            Distance distance = (Distance)other;
            return Equals(distance);
        }

        public bool Equals(Distance other)
        {
            return AsMeters
                .ApproximatelyEquals(other.AsMeters);
        }

        public override int GetHashCode()
        {
            return AsMeters.GetHashCode();
        }

        public override string ToString()
        {
            return $"{AsMeters} m";
        }
    }
}
