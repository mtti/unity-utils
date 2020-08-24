using System;
using UnityEngine;

namespace mtti.Funcs.Units
{
    /// <summary>
    /// Represents a distance in meters.
    /// </summary>
    [Serializable]
    public struct Meters
    {
        /// <summary>
        /// Implicitly convert to float.
        /// </summary>
        public static implicit operator float(Meters v)
        {
            return v.Value;
        }

        /// <summary>
        /// Implicitly convert from float.
        /// </summary>
        public static implicit operator Meters(float value)
        {
            return new Meters(value);
        }

        public readonly float Value;

        public Meters(float value)
        {
            Value = value;
        }

        override public string ToString()
        {
            return string.Format("{0} m", Mathf.RoundToInt(Value));
        }
    }
}
