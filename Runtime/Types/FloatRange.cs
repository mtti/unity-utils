using System;
using UnityEngine;

namespace mtti.Funcs
{
    [Serializable]
    public struct FloatRange
    {
        [SerializeField]
        public float Min;

        [SerializeField]
        public float Max;

        public FloatRange(float min, float max)
        {
            if (min > max)
            {
                float tmp = min;
                min = max;
                max = tmp;
            }

            Min = min;
            Max = max;
        }

        /// <summary>
        /// Get a random value in the range (inclusive).
        /// </summary>
        public float GetRandom()
        {
            return UnityEngine.Random.Range(Min, Max);
        }
    }
}
