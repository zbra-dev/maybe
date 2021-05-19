using System;

namespace Maybe
{
    public struct Maybe<T>
    {
        public readonly static Maybe<T> Nothing = new Maybe<T>(default, false);

        private readonly T obj;
#pragma warning disable IDE0032 // Use auto property
        private readonly bool hasValue;
#pragma warning restore IDE0032 // Use auto property

        private Maybe(T obj, bool hasValue)
        {
            this.hasValue = hasValue;
            this.obj = obj;
        }

        internal Maybe(T obj)
        {
            this.obj = obj;
            hasValue = true;
        }

        public bool HasValue => hasValue;

        public T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new Exception("No Value is present");
                }

                return obj;
            }
        }

        public T Or(T defaultValue) => hasValue ? obj : defaultValue;


        public T OrGet(Func<T> defaultSupplier) => hasValue ? obj : defaultSupplier();


        public T OrThrow(Func<Exception> errorSupplier)
        {
            if (hasValue)
            {
                return obj;
            }
            throw errorSupplier();
        }

        public void Consume(Action<T> consumer)
        {
            if (hasValue)
            {
                consumer(obj);
            }
        }

        public Maybe<R> Zip<U, R>(Maybe<U> other, Func<T, U, R> transformer)
        {
            if (hasValue && other.hasValue)
            {
                return transformer(Value, other.Value).ToMaybe();
            }

            return Maybe<R>.Nothing;
        }

        public Maybe<R> Zip<U, R>(Maybe<U> other, Func<T, U, Maybe<R>> transformer)
        {
            if (HasValue && other.HasValue)
            {
                return transformer(Value, other.Value);
            }

            return Maybe<R>.Nothing;
        }

        public void ZipAndConsume<U>(Maybe<U> other, Action<T, U> consumer)
        {
            if (HasValue && other.HasValue)
            {
                consumer(obj, other.Value);
            }
        }

        public bool Equals(Maybe<T> other)
        {
            if (HasValue)
            {
                return other.HasValue && Value.Equals(other.Value);
            }
            else
            {
                return !other.HasValue;
            }
        }

        public override bool Equals(object obj) => (obj is Maybe<T> other) && Equals(other);

        public override int GetHashCode() => HasValue ? Value.GetHashCode() : 0;

        public override string ToString() => HasValue ? Value.ToString() : string.Empty;

        public bool Is(T other) => HasValue && Value.Equals(other);

        public bool Is(Func<T, bool> predicate) => HasValue && predicate(Value);

        public Maybe<T> Where(Func<T, bool> predicate) => !Is(predicate) ? Nothing : this;

        public Maybe<TTarget> MaybeCast<TTarget>()
        {
            try
            {
                //return !HasValue
                //    ? Maybe<TTarget>.Nothing
                //    : ((TTarget)Convert.ChangeType(Value, typeof(TTarget))).ToMaybe();
                return !HasValue ? Maybe<TTarget>.Nothing : ((TTarget)(object)Value).ToMaybe();
            }
            catch (InvalidCastException)
            {
                return Maybe<TTarget>.Nothing;
            }
            //catch (FormatException)
            //{
            //    return Maybe<TTarget>.Nothing;
            //}
        }

    }
}
