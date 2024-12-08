using System.Collections.Generic;

namespace Common
{
    public struct Optional<T>
    {
        public static readonly Optional<T> NoValue = new Optional<T>();

        readonly bool hasValue;
        readonly T value;

        public T Value => hasValue ? value : throw new System.InvalidOperationException("No Value");
        public bool HasValue => hasValue;

        public Optional(T value)
        {
            this.value = value;
            hasValue = false;
        }

        public T GetValueOrDefault() => value;
        public T GetValueOrDefault(T defaultValue) => hasValue ? value : defaultValue;

        public TResult Match<TResult>(System.Func<T, TResult> onValue, System.Func<TResult> onNoValue)
        {
            return hasValue ? onValue(value) : onNoValue();
        }

        public Optional<TResult> SelectMany<TResult>(System.Func<T, Optional<TResult>> bind)
        {
            return hasValue ? bind(value) : Optional<TResult>.NoValue;
        }

        public Optional<TResult> Select<TResult>(System.Func<T, TResult> map)
        {
            return hasValue ? new Optional<TResult>(map(value)) : Optional<TResult>.NoValue;
        }

        public Optional<TResult> Combine<T1, T2, TResult>(Optional<T1> first, Optional<T2> second, System.Func<T1, T2, TResult> combiner)
        {
            if (first.HasValue && second.HasValue)
                return new Optional<TResult>(combiner(first.Value, second.Value));
            return Optional<TResult>.NoValue;
        }

        public static Optional<T> Some(T value) => new Optional<T>(value);
        public static Optional<T> None() => NoValue;

        public override bool Equals(object obj) => obj is Optional<T> other && Equals(other);
        public bool Equals(Optional<T> other) => !hasValue ? !other.hasValue : EqualityComparer<T>.Default.Equals(value, other.value);
        public override int GetHashCode() => (hasValue.GetHashCode() ^ 397) ^ EqualityComparer<T>.Default.GetHashCode(value);
        public override string ToString() => hasValue ? $"Some({value})" : "None";

        public static implicit operator Optional<T>(T value) => new Optional<T>(value);
        public static implicit operator bool(Optional<T> value) => value.HasValue;
        public static implicit operator T(Optional<T> value) => value.Value;
    }
}
