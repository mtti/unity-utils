using System;
using UnityEngine;

namespace mtti.Funcs.Types
{
    /// <summary>
    /// Representation of 3D vectors and points using 64 bit integers.
    /// </summary>
    [Serializable]
    public struct Vector3Long : IEquatable<Vector3Long>
    {
        #region Operators

        public static Vector3Long operator +(
            Vector3Long lhs,
            Vector3Long rhs
        )
        {
            return new Vector3Long(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
        }

        public static Vector3Long operator -(
            Vector3Long lhs,
            Vector3Long rhs
        )
        {
            return new Vector3Long(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
        }

        public static Vector3Long operator *(
            Vector3Long lhs,
            Vector3Long rhs
        )
        {
            return new Vector3Long(lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z);
        }

        public static bool operator ==(
            Vector3Long lhs,
            Vector3Long rhs
        )
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(
            Vector3Long lhs,
            Vector3Long rhs
        )
        {
            return !lhs.Equals(rhs);
        }

        public static implicit operator Vector3Long(Vector3Int other)
        {
            return new Vector3Long(other.x, other.y, other.z);
        }

        #endregion

        #region Statics

        public static Vector3Long Zero = new Vector3Long(0);

        #endregion

        [SerializeField]
        private long _x;

        [SerializeField]
        private long _y;

        [SerializeField]
        private long _z;

        public long X
        {
            get { return _x; }
        }

        public long Y
        {
            get { return _y; }
        }

        public long Z
        {
            get { return _z; }
        }

        public long SquareMagnitude
        {
            get { return (_x * _x) + (_y * _y) + (_z * _z); }
        }

        public double Magnitude
        {
            get { return Math.Sqrt(SquareMagnitude); }
        }

        public Vector3 Normalized
        {
            get
            {
                var magnitude = Magnitude;
                return new Vector3(
                    (float)((double)_x / magnitude),
                    (float)((double)_y / magnitude),
                    (float)((double)_z / magnitude)
                );
            }
        }

        public Vector3Long(
            long x,
            long y,
            long z
        )
        {
            _x = x;
            _y = y;
            _z = z;
        }

        /// <summary>
        /// Create a new <c>Vector3Long</c> with all axes set to the same value.
        /// </summary>
        public Vector3Long(long value)
        {
            _x = value;
            _y = value;
            _z = value;
        }

        public override bool Equals(object other)
        {
            if (!(other is Vector3Long)) return false;
            return Equals((Vector3Long)other);
        }

        public bool Equals(Vector3Long other)
        {
            return _x == other._x && _y == other._y && _z == other._z;
        }

        public override int GetHashCode()
        {
            return (int)(_x ^ _y ^ _z);
        }

        public override string ToString()
        {
            return $"{_x}, {_y}, {_z}";
        }
    }
}
