using System;
using System.Text;

namespace mtti.Pools
{
    public class PooledStringBuilder : IDisposable
    {
        public static PooledStringBuilder Claim(out StringBuilder value)
        {
            var pooled = ObjectPool<PooledStringBuilder>.Instance.Claim();
            value = pooled._value;
            return pooled;
        }

        StringBuilder _value;

        public PooledStringBuilder()
        {
            _value = new StringBuilder();
        }

        public void Dispose()
        {
            _value.Clear();
            ObjectPool<PooledStringBuilder>.Instance.Release(this);
        }
    }
}
