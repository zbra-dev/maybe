using System;

namespace Maybe
{
    public static class MaybeExtentions
    {
        #region Defaults Values
        public static string OrEmpty(this Maybe<string> value) => value.Or(string.Empty);

        public static T OrNull<T>(this Maybe<T> a) where T : class => a.Or(null);

        public static bool OrTrue(this Maybe<bool> a) => a.Or(true);

        public static bool OrFalse(this Maybe<bool> a) => a.Or(false);

        #endregion

        #region Interaction with Nullable
        public static T? ToNullable<T>(this Maybe<T> value)
            where T : struct
        {
            if (!value.HasValue)
            {
                return null;
            }
            else
            {
                return value.Value;
            }
        }

        #endregion

        #region Monad bind
        public static Maybe<S> ToMaybe<S>(this S? value) where S : struct
            => !value.HasValue ? Maybe<S>.Nothing
            : new Maybe<S>(value.Value);

        public static Maybe<string> ToMaybe(this string value)
            => string.IsNullOrEmpty(value) ? Maybe<string>.Nothing
                                           : new Maybe<string>(value);

        public static Maybe<T> ToMaybe<T>(this T value)
        {
            if (value == null)
            {
                return Maybe<T>.Nothing;
            }

            var valueType = value.GetType();

            if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Maybe<>))
            {
                bool hasValue = (bool)valueType.GetProperty("HasValue").GetGetMethod().Invoke(value, null);

                if (hasValue)
                {
                    var val = valueType.GetProperty("Value").GetGetMethod().Invoke(value, null);
                    if (val is T)
                    {
                        return new Maybe<T>((T)val);
                    }
                }
                return Maybe<T>.Nothing;
            }
            return new Maybe<T>(value);
        }

        #endregion

        #region Operations 
        public static Maybe<V> Select<T, V>(this Maybe<T> m, Func<T, V> k)
            => !m.HasValue ? Maybe<V>.Nothing
                           : k(m.Value).ToMaybe();

        public static Maybe<V> Select<T, V>(this Maybe<T> m, Func<T, V?> k)
            where V : struct
            => !m.HasValue ? Maybe<V>.Nothing
                           : ToMaybe(k(m.Value));

        public static Maybe<V> SelectMany<T, V>(this Maybe<T> m, Func<T, Maybe<V>> k)
             => !m.HasValue ? Maybe<V>.Nothing
                            : k(m.Value);

        public static Maybe<T> OrGetAlternative<T>(this Maybe<T> m, Func<Maybe<T>> alternative)
            => (!m.HasValue) ? alternative()
                             : m;

        public static Maybe<T> OrGetAlternative<T>(this Maybe<T> m, Func<T> alternative)
            => (!m.HasValue) ? ToMaybe(alternative())
                             : m;
        #endregion
    }
}
