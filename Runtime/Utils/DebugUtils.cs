using mtti.Funcs.Collections;
using System;
using System.Text;
using System.Diagnostics;

namespace mtti.Funcs
{
    public struct SampledValue
    {
        public static SampledValue CreateRepeated(SampledValue original)
        {
            return new SampledValue(original.Value, original.Repeats + 1);
        }

        public readonly long Time;

        public readonly int Frame;

        public readonly object Value;

        public readonly int Repeats;

        public SampledValue(object value, int repeats = 0)
        {
            Time = DateTime.UtcNow.Ticks;
            Frame = UnityEngine.Time.frameCount;
            Value = value;
            Repeats = repeats;
        }
    }

    public static class DebugUtils
    {
        private static MultiValueDictionary<string, SampledValue> s_values
            = new MultiValueDictionary<string, SampledValue>();

        private static StringBuilder s_sb = new StringBuilder();

        private static long _sampleTime = long.MinValue;

        public static MultiValueDictionary<string, SampledValue> SampledValues
        {
            get { return s_values; }
        }

        /// <summary>
        /// The last time in ticks when a value was last sampled.
        /// </summary>
        public static long SampleTime { get { return _sampleTime; } }

        public static void Sample(object value)
        {
            Sample(GetDefaultId(), value);
        }

        public static void Sample(string id, object value)
        {
            // Check for repeats
            var existingCount = s_values.Count(id);
            if (existingCount > 0)
            {
                var lastIndex = existingCount - 1;
                var last = s_values.GetLast(id);

                var bothNull = value == null && last.Value == null;
                var areEqual = value != null && value.Equals(last.Value);

                if (bothNull || areEqual)
                {
                    s_values.Set(
                        id,
                        lastIndex,
                        SampledValue.CreateRepeated(last)
                    );
                    return;
                }
            }

            s_values.Add(id, new SampledValue(value));
            if (s_values.Count(id) > 256) s_values.RemoveAt(id, 0);

            _sampleTime = DateTime.UtcNow.Ticks;
        }

        private static string GetDefaultId()
        {
            var st = new StackTrace(2, true);
            if (st.FrameCount == 0) throw new Exception("Unexpected empty stack trace");

            var frame = st.GetFrame(0);

            return s_sb
                .Clear()
                .Append(frame.GetFileName())
                .Append(':')
                .Append(frame.GetFileLineNumber())
                .Append(':')
                .Append(frame.GetFileColumnNumber())
                .ToString();
        }
    }
}
