using System;
using System.Collections.Generic;
using System.Linq;

namespace Maybe
{
    public static class MaybeEnumerableExtentions
    {
        #region Empties 
        public static IEnumerable<T> OrEmpty<T>(this Maybe<IEnumerable<T>> maybe)
            => maybe.OrGet(() => Enumerable.Empty<T>());


        public static ICollection<T> OrEmpty<T>(this Maybe<ICollection<T>> maybe)
            => maybe.OrGet(() => new HashSet<T>());


        public static IReadOnlyCollection<T> OrEmpty<T>(this Maybe<IReadOnlyCollection<T>> maybe)
            => maybe.OrGet(() => new HashSet<T>());

        public static IList<T> OrEmpty<T>(this Maybe<List<T>> maybe)
        {
            if (maybe.HasValue)
            {
                return maybe.Value;
            }
            else
            {
                return new T[0];
            }
        }

        public static IList<T> OrEmpty<T>(this Maybe<IList<T>> maybe)
            => maybe.OrGet(() => new T[0]);


        public static ISet<T> OrEmpty<T>(this Maybe<ISet<T>> maybe)
            => maybe.OrGet(() => new HashSet<T>());

        #endregion

        #region Conditional Get

        public static Maybe<T> MaybeGet<T>(this IList<T> list, int position)
        {
            if (list == null || list.Count == 0)
            {
                return Maybe<T>.Nothing;
            }

            if (position >= 0 && list.Count > position)
            {
                return list[position].ToMaybe();
            }
            return Maybe<T>.Nothing;
        }

        public static Maybe<T> MaybeSingle<T>(this IEnumerable<T?> enumerable) where T : struct
        {
            if (enumerable == null)
            {
                return Maybe<T>.Nothing;
            }

            using (var enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var result = enumerator.Current;

                    if (enumerator.MoveNext())
                    {
                        throw new InvalidOperationException("Sequence contains more than one element");
                    }

                    return result.ToMaybe();
                }

                return Maybe<T>.Nothing;
            }
        }

        public static Maybe<T> MaybeSingle<T>(this IEnumerable<T?> enumerable, Func<T?, bool> predicate) where T : struct
        {
            if (enumerable == null)
            {
                return Maybe<T>.Nothing;
            }

            predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));

            using (var enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var result = enumerator.Current;

                    if (predicate(result))
                    {
                        while (enumerator.MoveNext())
                        {
                            if (predicate(enumerator.Current))
                            {
                                throw new InvalidOperationException("Sequence contains more than one element");
                            }
                        }

                        return result.ToMaybe();
                    }
                }

                return Maybe<T>.Nothing;
            }
        }

        public static Maybe<T> MaybeSingle<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return Maybe<T>.Nothing;
            }

            using (var enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var result = enumerator.Current;

                    if (enumerator.MoveNext())
                    {
                        throw new InvalidOperationException("Sequence contains more than one element");
                    }

                    return result.ToMaybe();
                }

                return Maybe<T>.Nothing;
            }
        }

        public static Maybe<T> MaybeSingle<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            if (enumerable == null)
            {
                return Maybe<T>.Nothing;
            }

            predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));

            using (var enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var result = enumerator.Current;

                    if (predicate(result))
                    {
                        while (enumerator.MoveNext())
                        {
                            if (predicate(enumerator.Current))
                            {
                                throw new InvalidOperationException("Sequence contains more than one element");
                            }
                        }

                        return result.ToMaybe();
                    }
                }

                return Maybe<T>.Nothing;
            }
        }

        public static Maybe<T> MaybeFirst<T>(this IEnumerable<T> enumerable)
        {
            return (enumerable == null || !enumerable.Any()) ? Maybe<T>.Nothing : enumerable.First().ToMaybe();
        }

        // Note that if T is a value type, enumerable.FirstOrDefault(predicate).ToMaybe() will NOT return a Maybe.Nothing
        // when no element is found
        public static Maybe<T> MaybeFirst<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            if (enumerable == null)
            {
                return Maybe<T>.Nothing;
            }
            try
            {
                return enumerable.First(predicate).ToMaybe();
            }
            catch (InvalidOperationException)
            {
                return Maybe<T>.Nothing;
            }
        }

        public static Maybe<T> MaybeFirst<T>(this IEnumerable<T?> enumerable) where T : struct
        {
            return (enumerable == null || !enumerable.Any()) ? Maybe<T>.Nothing : enumerable.First().ToMaybe();
        }

        public static Maybe<V> MaybeGet<K, V>(this IDictionary<K, V> dictionary, K key)
        {
            if (dictionary == null)
            {
                return Maybe<V>.Nothing;
            }

            return dictionary.TryGetValue(key, out var value) ? value.ToMaybe() : Maybe<V>.Nothing;
        }

        public static Maybe<V> MaybeGet<K, V>(this IDictionary<K, V> dictionary, Maybe<K> key)
        {
            if (dictionary == null)
            {
                return Maybe<V>.Nothing;
            }

            return key.HasValue && dictionary.TryGetValue(key.Value, out var value) ? value.ToMaybe() : Maybe<V>.Nothing;
        }

        public static Maybe<V> MaybeGet<K, V>(this IDictionary<K, Maybe<V>> dictionary, K key)
        {
            if (dictionary == null)
            {
                return Maybe<V>.Nothing;
            }

            return dictionary.TryGetValue(key, out var value) ? value : Maybe<V>.Nothing;
        }

        public static Maybe<V> MaybeGet<K, V>(this IDictionary<K, Maybe<V>> dictionary, Maybe<K> key)
        {
            if (dictionary == null)
            {
                return Maybe<V>.Nothing;
            }

            return key.HasValue && dictionary.TryGetValue(key.Value, out var value) ? value : Maybe<V>.Nothing;
        }
        #endregion

        #region Compact

        public static IEnumerable<T> Compact<T>(this IEnumerable<Maybe<T>> enumerable)
            => enumerable.Where(m => m.HasValue).Select(m => m.Value);

        public static IEnumerable<T> Compact<T>(this IEnumerable<T?> enumerable)
            where T : struct
           => enumerable.Where(m => m.HasValue).Select(m => m.Value);

        #endregion
    }
}
