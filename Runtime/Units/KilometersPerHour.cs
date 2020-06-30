using System;

namespace mtti.Funcs.Units
{
    /// <summary>
    /// Represents speed in kilometers per hour.
    /// </summary>
    [Serializable]
    public struct KilometersPerHour : ISpeed
    {
        /// <summary>
        /// Implicitly convert to float.
        /// </summary>
        public static implicit operator float(KilometersPerHour v)
        {
            return v.Value;
        }

        /// <summary>
        /// Implicitly convert from float.
        /// </summary>
        public static implicit operator KilometersPerHour(float value)
        {
            return new KilometersPerHour(value);
        }

        public static KilometersPerHour operator +(
            KilometersPerHour lhs,
            KilometersPerHour rhs
        )
        {
            return new KilometersPerHour(lhs.Value + rhs.Value);
        }

        public static KilometersPerHour operator -(
            KilometersPerHour lhs,
            KilometersPerHour rhs
        )
        {
            return new KilometersPerHour(lhs.Value - rhs.Value);
        }

        /// <summary>
        /// Convert kilometers per hour to meters per second.
        /// </summary>
        public static float ToMetersPerSecond(float value)
        {
            return value / 3.6f;
        }

        public readonly float Value;

        public float AsMetersPerSecond
        {
            get
            {
                return ToMetersPerSecond(Value);
            }
        }

        public KilometersPerHour(float value)
        {
            Value = value;
        }

        override public string ToString()
        {
            return string.Format("{0} km/h", Value);
        }
    }
}
