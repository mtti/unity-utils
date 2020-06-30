namespace mtti.Funcs.Units
{
    /// <summary>
    /// Represents speed in miles per hour.
    /// </summary>
    public struct MilesPerHour : ISpeed
    {
        /// <summary>
        /// Implicitly convert to float.
        /// </summary>
        public static implicit operator float(MilesPerHour v)
        {
            return v.Value;
        }

        /// <summary>
        /// Implicitly convert from float.
        /// </summary>
        public static implicit operator MilesPerHour(float value)
        {
            return new MilesPerHour(value);
        }

        /// <summary>
        /// Convert miles per hour to meters per second.
        /// </summary>
        public static float ToMetersPerSecond(float value)
        {
            return value / 2.237f;
        }

        public readonly float Value;

        public float AsMetersPerSecond
        {
            get
            {
                return ToMetersPerSecond(Value);
            }
        }

        public MilesPerHour(float value)
        {
            Value = value;
        }

        override public string ToString()
        {
            return string.Format("{0} mph", Value);
        }
    }
}
