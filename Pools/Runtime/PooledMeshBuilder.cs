using mtti.Funcs;
using System;

namespace mtti.Pools
{
    public class PooledMeshBuilder : MeshBuilder, IDisposable
    {
        public void Dispose()
        {
            Clear();
            ObjectPool<PooledMeshBuilder>.Instance.Release(this);
        }

        public static PooledMeshBuilder Claim()
        {
            return ObjectPool<PooledMeshBuilder>.Instance.Claim();
        }
    }
}
