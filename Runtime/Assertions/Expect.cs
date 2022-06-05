using System;

namespace mtti.Funcs.Assertions
{
    public static class Expect
    {
        public static T NonNull<T>(T value) where T : class
        {
            if (value == null) throw new ArgumentNullException(
                "value",
                $"Expected a non-null {typeof(T).Name}, got null"
            );
            return value;
        }
    }
}
