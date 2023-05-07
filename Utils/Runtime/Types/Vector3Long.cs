using System;
using UnityEngine;

namespace mtti.Funcs.Types
{
    /// <summary>
    /// Representation of 3D vectors and points using 64 bit integers.
    /// </summary>
    [Serializable]
    public struct Vector3Long: IEquatable<Vector3Long>
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
            return lhs._x == rhs._x && lhs._y == rhs._y && lhs._z == rhs._z;
        }
        
        public static bool operator !=(
            Vector3Long lhs,
            Vector3Long rhs
        )
        {
            return lhs._x != rhs._x || lhs._y != rhs._y || lhs._z != rhs._z;
        }

        public static implicit operator Vector3Long(Vector3Int other)
        {
            return new Vector3Long(other.x, other.y, other.z);
        }
        
        #endregion
        
        #region Statics

        public static Vector3Long Zero = new Vector3Long(0, 0, 0); 
        
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
        
        public Vector3Long(long x, long y, long z)
        {
            _x = x;
            _y = y;
            _z = z;
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
