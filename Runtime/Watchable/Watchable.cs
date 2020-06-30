namespace mtti.Funcs
{
    public delegate void WatchableEvent<T>(T value);

    public interface IReadOnlyWatchable<T>
    {
        event WatchableEvent<T> OnChange;

        T Value { get; }
    }

    public interface IWatchableString
    {
        event WatchableEvent<string> OnChangeString;

        string ToString();
    }

    public interface IWatchable<T>
    {
        event WatchableEvent<T> OnChange;

        T Value { get; set; }
    }

    /// <summary>
    /// Wraps a value so that event handlers can be used to watch for changes.
    /// </summary>
    public class Watchable<T> : IWatchable<T>, IReadOnlyWatchable<T>,
        IWatchableString
    {
        public event WatchableEvent<T> OnChange;

        public event WatchableEvent<string> OnChangeString;

        protected T _value;

        public T Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (value.Equals(_value)) return;

                _value = value;

                if (OnChange != null)
                {
                    OnChange(value);
                }

                if (OnChangeString != null)
                {
                    OnChangeString(value.ToString());
                }
            }
        }

        public Watchable()
        {
            _value = default(T);
        }

        public Watchable(T value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
