using System;
using System.Collections.Generic;

namespace mtti.Pools
{
    public struct PooledCompositeDisposable : IDisposable
    {
        public static PooledCompositeDisposable Claim()
        {
            var inner = ObjectPool<InnerPooledCompositeDisposable>.Instance.Claim();
            return new PooledCompositeDisposable(inner);
        }

        private int _version;

        private InnerPooledCompositeDisposable _inner;

        public bool IsAllocated
        {
            get { return _inner != null; }
        }

        internal PooledCompositeDisposable(InnerPooledCompositeDisposable inner)
        {
            _inner = inner;
            _version = inner.Version;
        }

        public void Add(IDisposable item)
        {
            ExpectValid();

            _inner.Add(item);
        }

        public void Clear()
        {
            ExpectValid();
            _inner.Clear();
        }

        public void Dispose()
        {
            ExpectValid();

            _inner.Clear();
            _inner.Version++;
            ObjectPool<InnerPooledCompositeDisposable>.Instance.Release(_inner);
            _inner = null;
        }

        private void ExpectValid()
        {
            if (_inner == null)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            if (_inner.Version != _version)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
    }

    internal class InnerPooledCompositeDisposable
    {
        internal int Version;

        private List<IDisposable> _disposables = new List<IDisposable>();

        public void Add(IDisposable item)
        {
            if (_disposables.Contains(item)) return;
            _disposables.Add(item);
        }

        /// <summary>
        /// Add a disposable to the collection and then return it. This is
        /// a helper method to facilitate chaining operators to disposable
        /// observables.
        /// </summary>
        public T Include<T>(T item) where T : IDisposable
        {
            Add(item);
            return item;
        }

        public void Clear()
        {
            for (int i = 0, count = _disposables.Count; i < count; i++)
            {
                _disposables[i].Dispose();
            }

            _disposables.Clear();
        }
    }
}
