using UnityEngine;

namespace mtti.Funcs
{
    public struct Circle
    {
        public readonly Vector2 Origin;

        public readonly float Radius;

        public Circle(Vector2 origin, float radius)
        {
            Origin = origin;
            Radius = radius;
        }

        public Circle(float radius)
        {
            Origin = Vector2.zero;
            Radius = radius;
        }

        public Vector2 GetPoint(float angle)
        {
            return new Vector2(
                Radius * Mathf.Cos(angle),
                Radius * Mathf.Sin(angle)
            );
        }

        /// <summary>
        /// Get the position of an evenly distributed point around the
        /// circle.
        /// </summary>
        public Vector2 GetPoint(int count, int i)
        {
            float theta = (Mathf.PI * 2.0f) / (float)count;
            return GetPoint(i * theta);
        }
    }
}
