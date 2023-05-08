using System;
using UnityEngine;

namespace mtti.Funcs.Types
{
    /// <summary>
    /// Represents an axis-aligned bounding box with all values as 64-bit
    /// integers.
    /// </summary>
    [Serializable]
    public struct BoundsLong : IEquatable<BoundsLong>
    {
        [SerializeField]
        private Vector3Long _center;

        [SerializeField]
        private Vector3Long _size;

        public Vector3Long Center
        {
            get { return _center; }
        }

        public Vector3Long Size
        {
            get { return _size; }
        }

        public Vector3Long Min
        {
            get
            {
                var halfX = _size.X / 2;
                var halfY = _size.Y / 2;
                var halfZ = _size.Z / 2;

                return new Vector3Long(
                    _center.X - halfX,
                    _center.Y - halfY,
                    _center.Z - halfZ
                );
            }
        }

        public Vector3Long Max
        {
            get
            {
                var halfX = _size.X / 2;
                var halfY = _size.Y / 2;
                var halfZ = _size.Z / 2;

                return new Vector3Long(
                    _center.X + halfX,
                    _center.Y + halfY,
                    _center.Z + halfZ
                );
            }
        }

        public BoundsLong(
            Vector3Long center,
            Vector3Long size
        )
        {
            _center = center;
            _size = size;
        }

        /// <summary>
        /// Check if a point is contained in the bounding box.
        /// </summary>
        public bool Contains(Vector3Long point)
        {
            var min = Min;
            var max = Max;

            return point.X >= min.X && point.X < max.X
                && point.Y >= min.Y && point.Y < max.Y
                && point.Z >= min.Z && point.Z < max.Z;
        }

        /// <summary>
        /// Check if another bounding box is container entirely within this one.
        /// </summary>
        public bool Contains(BoundsLong other)
        {
            return Contains(other.Min) && Contains(other.Max);
        }

        public bool Equals(BoundsLong other)
        {
            return _center.Equals(other._center) && _size.Equals(other._size);
        }

        public override int GetHashCode()
        {
            return _center.GetHashCode() ^ _size.GetHashCode();
        }
    }
}
