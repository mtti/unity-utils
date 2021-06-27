/*
Copyright 2021 Matti Hiltunen (https://www.mattihiltunen.com)

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

using UnityEngine;

namespace mtti.Funcs.Types
{
    /// <summary>
    /// A quadratic bezier curve.
    /// </summary>
    public struct QuadraticBezier
    {
        public static Vector3 GetPoint(
            Vector3 p0,
            Vector3 p1,
            Vector3 p2,
            float t
        )
        {
            Vector3 q0 = Vector3.Lerp(p0, p1, t);
            Vector3 q1 = Vector3.Lerp(p1, p2, t);
            return Vector3.Lerp(q0, q1, t);
        }

        [SerializeField]
        private Vector3 _p0;

        [SerializeField]
        private Vector3 _p1;

        [SerializeField]
        private Vector3 _p2;

        public QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            _p0 = p0;
            _p1 = p1;
            _p2 = p2;
        }

        /// <summary>
        /// Get a point along the curve.
        /// </summary>
        public Vector3 GetPoint(float t)
        {
            return QuadraticBezier.GetPoint(_p0, _p1, _p2, t);
        }

        /// <summary>
        /// Get multiple points along the curve.
        /// </summary>
        public void GetPoints(Vector3[] result)
        {
            float stepSize = 1.0f / (float)result.Length;
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = GetPoint(stepSize * (float)i);
            }
        }

        public float EstimateLength(int resolution)
        {
            float length = 0.0f;
            float stepSize = 1.0f / (float)resolution;
            Vector3 previousPoint = _p0;
            Vector3 currentPoint;

            for (int i = 0; i < resolution; i++)
            {
                currentPoint = GetPoint(stepSize * (float)i);
                length += Vector3.Distance(previousPoint, currentPoint);

                previousPoint = currentPoint;
            }
            length += Vector3.Distance(previousPoint, _p2);

            return length;
        }

        /// <summary>
        /// Split the curve into two at a <c>t</c> value.
        /// </summary>
        public void Split(float t, QuadraticBezier[] result)
        {
            Vector3 q0 = Vector3.Lerp(_p0, _p1, t);
            Vector3 q1 = Vector3.Lerp(_p1, _p2, t);
            Vector3 b = Vector3.Lerp(q0, q1, t);
            result[0] = new QuadraticBezier(_p0, q0, b);
            result[1] = new QuadraticBezier(b, q1, _p2);
        }
    }
}
