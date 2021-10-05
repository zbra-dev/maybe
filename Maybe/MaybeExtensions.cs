using System;

namespace Maybe
{
    /// <summary>
    /// Contains extensions methods for the maybe class.
    /// </summary>
    public static class MaybeExtentions
    {
        #region Defaults Values
        /// <summary>
        /// Returns the encapsulated value or an empty string.
        /// </summary>
        /// <returns>
        /// string.Empty if subject.HasValue is false, otherwise subject.Value
        /// </returns>
        /// <param name="subject"> The subject.</param>
        public static string OrEmpty(this Maybe<string> subject) => subject.Or(string.Empty);

        /// <summary>
        /// Returns the encapsulated value or null.
        /// </summary>
        /// <returns>
        /// null if subject.HasValue is false, otherwise subject.Value
        /// </returns>
        /// <param name="subject"> The subject.</param>
        public static T OrNull<T>(this Maybe<T> subject) where T : class => subject.Or((T)null);

        /// <summary>
        /// Returns the encapsulated value or true.
        /// </summary>
        /// <returns>
        /// true if subject.HasValue is false, otherwise subject.Value
        /// </returns>
        /// <param name="subject"> The subject.</param>
        public static bool OrTrue(this Maybe<bool> subject) => subject.Or(true);

        /// <summary>
        /// Returns the encapsulated value or false.
        /// </summary>
        /// <returns>
        /// false if subject.HasValue is false, otherwise subject.Value
        /// </returns>
        /// <param name="subject"> The subject.</param>
        public static bool OrFalse(this Maybe<bool> subject) => subject.Or(false);

        #endregion

        #region Interaction with Nullable/// <summary>
        /// <summary>
        /// Converts the value to Nullable&lt;<typeparamref name="T"/>&gt;.
        /// </summary>
        /// <returns>
        /// null is value.HasValue is false, otherwise value.Value
        /// </returns>
        /// <param name="value"> The value to be converted.</param>
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
        /// <summary>
        /// Converts the value to Maybe&lt;<typeparamref name="S"/>&gt;.
        /// </summary>
        /// <returns>
        /// Maybe&lt;<typeparamref name="S"/>&gt;.Nothing if value.HasValue is false,
        /// otherwise new Maybe&lt;<typeparamref name="S"/>&gt;(value.Value)
        /// </returns>
        /// <param name="value"> The value to be converted.</param>
        public static Maybe<S> ToMaybe<S>(this S? value) where S : struct
        {
            return !value.HasValue
                ? Maybe<S>.Nothing
                : new Maybe<S>(value.Value);
        }

        /// <summary>
        /// This methods prevents stacking maybes, it just returns value
        /// </summary>
        /// <returns>
        /// value
        /// </returns>
        /// <param name="value"> The value to be converted.</param>
        public static Maybe<T> ToMaybe<T>(this Maybe<T> value)
        {
            return value;
        }

        /// <summary>
        /// Converts the value to Maybe&lt;<typeparamref name="T"/>&gt;.
        /// </summary>
        /// <returns>
        /// Maybe&lt;<typeparamref name="T"/>&gt;.Nothing if value is null,
        /// otherwise new Maybe&lt;<typeparamref name="T"/>&gt;(value)
        /// </returns>
        /// <param name="value"> The value to be converted.</param>
        public static Maybe<T> ToMaybe<T>(this T value)
        {
            return value == null ? Maybe<T>.Nothing : new Maybe<T>(value);
        }

        #endregion

        #region Operations 
        /// <summary>
        /// Projects the value according to the selector.
        /// Analogous to Linq's Select.
        /// </summary>
        /// <returns>
        /// Maybe&lt;<typeparamref name="V"/>&gt;.Nothing if subject.HasValue is false,
        /// otherwise returns an instance of Maybe&lt;<typeparamref name="V"/>&gt; with the projected value
        /// </returns>
        /// <param name="subject"> The subject that will be projected.</param>
        /// <param name="selector"> The selector to be applied.</param>
        public static Maybe<V> Select<T, V>(this Maybe<T> subject, Func<T, V> selector)
        {
            selector = selector ?? throw new ArgumentNullException(nameof(selector));

            return !subject.HasValue ? Maybe<V>.Nothing : selector(subject.Value).ToMaybe();
        }

        /// <summary>
        /// Projects the value according to the selector.
        /// Analogous to Linq's Select.
        /// </summary>
        /// <returns>
        /// Maybe&lt;<typeparamref name="V"/>&gt;.Nothing if subject.HasValue is false,
        /// otherwise returns an instance of Maybe&lt;<typeparamref name="V"/>&gt; with the projected value
        /// </returns>
        /// <param name="subject"> The subject that will be projected.</param>
        /// <param name="selector"> The selector to be applied.</param>
        public static Maybe<V> Select<T, V>(this Maybe<T> subject, Func<T, V?> selector)
            where V : struct
        {
            selector = selector ?? throw new ArgumentNullException(nameof(selector));

            return !subject.HasValue ? Maybe<V>.Nothing : ToMaybe(selector(subject.Value));
        }

        /// <summary>
        /// Projects the value according to the selector and flatten it.
        /// Analogous to Linq's SelectMany.
        /// </summary>
        /// <returns>
        /// Maybe&lt;<typeparamref name="V"/>&gt;.Nothing if subject.HasValue is false,
        /// otherwise returns an instance of Maybe&lt;<typeparamref name="V"/>&gt; with the projected value
        /// </returns>
        /// <param name="subject"> The subject that will be projected.</param>
        /// <param name="selector"> The selector to be applied.</param>
        public static Maybe<V> SelectMany<T, V>(this Maybe<T> subject, Func<T, Maybe<V>> selector)
        {
            selector = selector ?? throw new ArgumentNullException(nameof(selector));

            return !subject.HasValue ? Maybe<V>.Nothing : selector(subject.Value);
        }
        #endregion
    }
}
