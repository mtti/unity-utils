/*
Copyright 2017-2020 Matti Hiltunen

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
using System.Collections.Generic;
using UnityEngine;

namespace mtti.Funcs
{
    public static class MathUtils
    {
        private static RaycastHit s_hit;

        private static List<Vector3> s_points = new List<Vector3>();

        /// <summary>
        /// Compare two floating point numbers to see if they're nearly equal.
        /// </summary>
        /// <remarks>
        /// Ported from <a href="http://floating-point-gui.de/errors/comparison/">
        /// The Floating Point Guide</a>, under CC-BY.
        /// </remarks>
        public static bool NearlyEqual(float a, float b, float epsilon)
        {
            float difference = Math.Abs(a - b);

            if (a == b)
            {
                return true;
            }
            else if (a == 0 || b == 0 || difference < Single.MinValue)
            {
                return difference < (epsilon * Single.MinValue);
            }
            else
            {
                return difference / (Math.Abs(a) + Math.Abs(b)) < epsilon;
            }
        }

        public static float ReverseLerp(float value, float a, float b)
        {
            return (value - a) / (b - a);
        }

        public static float Lerp(float a, float b, float t)
        {
            return ((1.0f - t) * a) + (b * t);
        }

        public static float Remap(
            float value,
            float a1,
            float b1,
            float a2,
            float b2
        )
        {
            return Lerp(
                a2,
                b2,
                ReverseLerp(value, a1, b1)
            );
        }

        /// <summary>
        /// Convert a flat array index to a 3D array position.
        /// </summary>
        public static Vector3Int FromIndex(int index, int width, int height)
        {
            int z = index / (width * height);
            index = index - (width * height * z);
            int x = index % width;
            int y = index / width;
            return new Vector3Int(x, y, z);
        }

        /// <summary>
        /// Convert a flat array index to a 2D array position.
        /// </summary>
        public static Vector2Int FromIndex(int index, int width)
        {
            int x = index % width;
            int y = index / width;
            return new Vector2Int(x, y);
        }

        public static int ToIndex(int x, int y, int z, int width, int height)
        {
            return (width * height * z) + (x + (y * width));
        }

        public static int ToIndex(int x, int y, int width)
        {
            return x + (y * width);
        }

        public static bool GetClosestRaycast(
            Vector3 position,
            Vector3[] directions,
            float distance,
            out Vector3 result)
        {
            s_points.Clear();
            bool hitSomething;

            for (int i = 0; i < directions.Length; i++)
            {
                hitSomething = Physics.Raycast(
                    position,
                    directions[i],
                    out s_hit,
                    distance
                );
                if (hitSomething)
                {
                    s_points.Add(s_hit.point);
                }
            }

            if (s_points.Count == 0)
            {
                result = Vector3.zero;
                return false;
            }

            result = GetClosest(position, s_points);
            return true;
        }

        public static Vector3 GetClosest(Vector3 position, List<Vector3> points)
        {
            Vector3 result = points[0];
            Vector3 offset = result - position;
            float closestDistance = offset.sqrMagnitude;

            float distance;
            for (int i = 1, count = points.Count; i < count; i++)
            {
                offset = points[i] - position;
                distance = offset.sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    result = points[i];
                }
            }

            return result;
        }

        public static Bounds GetBounds(Vector3[] points)
        {
            Bounds result = new Bounds();
            for (int i = 0; i < points.Length; i++)
            {
                result.Encapsulate(points[i]);
            }
            return result;
        }

        /// <summary>
        /// Get the middle of a set of points.
        /// </summary>
        public static Vector2 GetCenter(Vector2[] points)
        {
            Vector2 min = new Vector2(Single.MaxValue, Single.MaxValue);
            Vector2 max = new Vector2(Single.MinValue, Single.MinValue);

            for (int i = 0; i < points.Length; i++)
            {
                Vector2 point = points[i];
                if (point.x < min.x)
                {
                    min.x = point.x;
                }
                if (point.y < min.y)
                {
                    min.y = point.y;
                }
                if (point.x > max.x)
                {
                    max.x = point.x;
                }
                if (point.y > max.y)
                {
                    max.y = point.y;
                }
            }

            return Vector2.Lerp(min, max, 0.5f);
        }

        /// <summary>
        /// Get the middle of a set of points.
        /// </summary>
        public static Vector2 GetCenter(List<Vector2> points)
        {
            Vector2 min = new Vector2(Single.MaxValue, Single.MaxValue);
            Vector2 max = new Vector2(Single.MinValue, Single.MinValue);

            for (int i = 0, count = points.Count; i < count; i++)
            {
                Vector2 point = points[i];
                if (point.x < min.x)
                {
                    min.x = point.x;
                }
                if (point.y < min.y)
                {
                    min.y = point.y;
                }
                if (point.x > max.x)
                {
                    max.x = point.x;
                }
                if (point.y > max.y)
                {
                    max.y = point.y;
                }
            }

            return Vector2.Lerp(min, max, 0.5f);
        }

        /// <summary>
        /// Get the middle of a set of points.
        /// </summary>
        public static Vector3 GetCenter(Vector3[] points)
        {
            Vector3 min = new Vector3(Single.MaxValue, Single.MaxValue);
            Vector3 max = new Vector3(Single.MinValue, Single.MinValue);

            for (int i = 0; i < points.Length; i++)
            {
                Vector3 point = points[i];
                if (point.x < min.x)
                {
                    min.x = point.x;
                }
                if (point.y < min.y)
                {
                    min.y = point.y;
                }
                if (point.x > max.x)
                {
                    max.x = point.x;
                }
                if (point.y > max.y)
                {
                    max.y = point.y;
                }
            }

            return Vector3.Lerp(min, max, 0.5f);
        }

        /// <summary>
        /// Get the middle of a set of points.
        /// </summary>
        public static Vector3 GetCenter(List<Vector3> points)
        {
            Vector3 min = new Vector3(Single.MaxValue, Single.MaxValue);
            Vector3 max = new Vector3(Single.MinValue, Single.MinValue);

            for (int i = 0, count = points.Count; i < count; i++)
            {
                Vector3 point = points[i];

                if (point.x < min.x)
                {
                    min.x = point.x;
                }
                if (point.y < min.y)
                {
                    min.y = point.y;
                }
                if (point.z < min.z)
                {
                    min.z = point.z;
                }

                if (point.x > max.x)
                {
                    max.x = point.x;
                }
                if (point.y > max.y)
                {
                    max.y = point.y;
                }
                if (point.z > max.z)
                {
                    max.z = point.z;
                }
            }

            return Vector3.Lerp(min, max, 0.5f);
        }

        public static Vector3 GetCentroid(Vector3 a, Vector3 b, Vector3 c)
        {
            return (a + b + c) / 3.0f;
        }
    }
}
