using mtti.Funcs.Collections;
using System;
using System.Text;
using System.Diagnostics;

namespace mtti.Funcs
{
    public struct SampledValue
    {
        public readonly long Time;

        public readonly int Frame;

        public readonly object Value;

        public SampledValue(object value)
        {
            Time = DateTime.UtcNow.Ticks;
            Frame = UnityEngine.Time.frameCount;
            Value = value;
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
