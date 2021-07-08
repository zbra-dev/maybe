using System;
using System.Collections.Generic;
using System.Linq;

namespace Maybe
{
    /// <summary>
    /// Contains extensions methods for enumerables.
    /// </summary>
    public static class MaybeEnumerableExtentions
    {
        #region Empties
        /// <summary>
        /// Returns the encapsulated value or an empty enumerable.
        /// </summary>
        /// <returns>
        /// An empty enumerable if subject.HasValue is false, otherwise subject.Value
        /// </returns>
        /// <param name="subject"> The subject.</param>
        public static IEnumerable<T> OrEmpty<T>(this Maybe<IEnumerable<T>> subject)
            => subject.OrGet(() => Enumerable.Empty<T>());

        /// <summary>
        /// Returns the encapsulated value or an empty collection.
        /// </summary>
        /// <returns>
        /// An empty collection if subject.HasValue is false, otherwise subject.Value
        /// </returns>
        /// <param name="subject"> The subject.</param>
        public static ICollection<T> OrEmpty<T>(this Maybe<ICollection<T>> subject)
            => subject.OrGet(() => new HashSet<T>());

        /// <summary>
        /// Returns the encapsulated value or an empty read only collection.
        /// </summary>
        /// <returns>
        /// An empty read only collection if subject.HasValue is false, otherwise subject.Value
        /// </returns>
        /// <param name="subject"> The subject.</param>
        public static IReadOnlyCollection<T> OrEmpty<T>(this Maybe<IReadOnlyCollection<T>> subject)
            => subject.OrGet(() => new HashSet<T>());

        /// <summary>
        /// Returns the encapsulated value or an empty list.
        /// </summary>
        /// <returns>
        /// An empty list if subject.HasValue is false, otherwise subject.Value
        /// </returns>
        /// <param name="subject"> The subject.</param>
        public static IList<T> OrEmpty<T>(this Maybe<List<T>> subject)
        {
            if (subject.HasValue)
            {
                return subject.Value;
            }
            else
            {
                return new T[0];
            }
        }

        /// <summary>
        /// Returns the encapsulated value or an empty list.
        /// </summary>
        /// <returns>
        /// An empty list if subject.HasValue is false, otherwise subject.Value
        /// </returns>
        /// <param name="subject"> The subject.</param>
        public static IList<T> OrEmpty<T>(this Maybe<IList<T>> subject)
            => subject.OrGet(() => new T[0]);

        /// <summary>
        /// Returns the encapsulated value or an empty set.
        /// </summary>
        /// <returns>
        /// An empty set if subject.HasValue is false, otherwise subject.Value
        /// </returns>
        /// <param name="subject"> The subject.</param>
        public static ISet<T> OrEmpty<T>(this Maybe<ISet<T>> subject)
            => subject.OrGet(() => new HashSet<T>());

        #endregion

        #region Conditional Get
        /// <summary>
        /// Returns the element at the specified position in source.
        /// </summary>
        /// <returns>
        /// Maybe&lt;<typeparamref name="T"/>&gt;.Nothing if source is null, position is negative or larger than source.Count.
        /// Otherwise returns the encapsulated value at the position
        /// </returns>
        /// <param name="source"> The source.</param>
        /// <param name="position"> The position, a 0 based index.</param>
        public static Maybe<T> MaybeGet<T>(this IList<T> source, int position)
        {
            if (source == null)
            {
                return Maybe<T>.Nothing;
            }

            if (position >= 0 && source.Count > position)
            {
                return source[position].ToMaybe();
            }
            return Maybe<T>.Nothing;
        }

        /// <summary>
        /// Returns the single element of the source.
        /// </summary>
        /// <returns>
        /// Maybe&lt;<typeparamref name="T"/>&gt;.Nothing if source is empty or null.
        /// Otherwise returns the encapsulated single value
        /// </returns>
        /// <param name="source"> The source.</param>
        /// <exception cref="InvalidOperationException">Thrown if source contains more than one element.</exception>
        public static Maybe<T> MaybeSingle<T>(this IEnumerable<T?> source) where T : struct
        {
            if (source == null || !source.Any())
            {
                return Maybe<T>.Nothing;
            }
            return source.Single().ToMaybe();
        }

        /// <summary>
        /// Returns the single element of the source.
        /// </summary>
        /// <returns>
        /// Maybe&lt;<typeparamref name="T"/>&gt;.Nothing if source is empty or null.
        /// Otherwise returns the encapsulated single value
        /// </returns>
        /// <param name="source"> The source.</param>
        /// <exception cref="InvalidOperationException">Thrown if source contains more than one element.</exception>
        public static Maybe<T> MaybeSingle<T>(this IEnumerable<T> source)
        {
            if (source == null || !source.Any())
            {
                return Maybe<T>.Nothing;
            }
            return source.Single().ToMaybe();
        }

        /// <summary>
        /// Returns the single element of the source that matches the predicate.
        /// </summary>
        /// <returns>
        /// Maybe&lt;<typeparamref name="T"/>&gt;.Nothing if source is null or no elements match the predicate.
        /// Otherwise returns the encapsulated single matching value
        /// </returns>
        /// <param name="source"> The source.</param>
        /// <param name="predicate"> The predicate.</param>
        /// <exception cref="InvalidOperationException">Thrown if multiple elements match the predicate.</exception>
        public static Maybe<T> MaybeSingle<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return MaybeSingle(source?.Where(predicate));
        }

        /// <summary>
        /// Returns the first element of source.
        /// </summary>
        /// <returns>
        /// Maybe&lt;<typeparamref name="T"/>&gt;.Nothing if source is null or empty.
        /// Otherwise returns the encapsulated first value in source
        /// </returns>
        /// <param name="source"> The source.</param>
        public static Maybe<T> MaybeFirst<T>(this IEnumerable<T> source)
        {
            return (source == null || !source.Any()) ? Maybe<T>.Nothing : source.First().ToMaybe();
        }

        // Note that if T is a value type, enumerable.FirstOrDefault(predicate).ToMaybe() will NOT return a Maybe.Nothing
        // when no element is found
        /// <summary>
        /// Returns the first element of source that matches the predicate.
        /// </summary>
        /// <returns>
        /// Maybe&lt;<typeparamref name="T"/>&gt;.Nothing if source is null or no elements match the predicate.
        /// Otherwise returns the encapsulated first matching value in source
        /// </returns>
        /// <param name="source"> The source.</param>
        /// <param name="predicate"> The predicate.</param>
        public static Maybe<T> MaybeFirst<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
            {
                return Maybe<T>.Nothing;
            }
            try
            {
                return source.First(predicate).ToMaybe();
            }
            catch (InvalidOperationException)
            {
                return Maybe<T>.Nothing;
            }
        }

        /// <summary>
        /// Returns the first element of source.
        /// </summary>
        /// <returns>
        /// Maybe&lt;<typeparamref name="T"/>&gt;.Nothing if source is null or empty.
        /// Otherwise returns the encapsulated first value in source
        /// </returns>
        /// <param name="source"> The source.</param>
        public static Maybe<T> MaybeFirst<T>(this IEnumerable<T?> source) where T : struct
        {
            return (source == null || !source.Any()) ? Maybe<T>.Nothing : source.First().ToMaybe();
        }

        /// <summary>
        /// Returns the value that is associated with the key.
        /// </summary>
        /// <returns>
        /// Maybe&lt;<typeparamref name="V"/>&gt;.Nothing if source is null or doesn't contain the key.
        /// Otherwise returns the encapsulated value associated with the key in the source
        /// </returns>
        /// <param name="source"> The dictionary that will be searched.</param>
        /// <param name="key"> The key.</param>
        /// <exception cref="ArgumentNullException">Thrown if key is null.</exception>
        public static Maybe<V> MaybeGet<K, V>(this IEnumerable<KeyValuePair<K, V>> source, K key)
        {
            return DoMaybeGet(source, key);
        }

        private static Maybe<V> DoMaybeGet<K, V>(this IEnumerable<KeyValuePair<K, V>> dictionary, K key)
        {
            return DoGeneralTryGetValue(dictionary, key, out var value) ? value.ToMaybe() : Maybe<V>.Nothing;
        }

        private static bool DoGeneralTryGetValue<K, V>(this IEnumerable<KeyValuePair<K, V>> dictionary, K key, out V value)
        {
            if (dictionary == null || !dictionary.Any())
            {
                value = default;
                return false;
            }


            //  most common since is the mutable version
            IDictionary<K, V> readOther = dictionary as IDictionary<K, V>;

            if (readOther == null)
            {
                // thy this one before falling back to list search
                IReadOnlyDictionary<K, V> dic = dictionary as IReadOnlyDictionary<K, V>;

                if (dic == null)
                {
                    // fall back to direct search
                    // use to List to overcome the problem with SingleOrDefault since if 
                    // V is a struct the default is a , possible and otherwise present, value.
                    var list = dictionary.Where(pair => pair.Key.Equals(key)).ToList();
                    if (list.Count > 0)
                    {
                        value = list[0].Value;
                        return true;
                    }
                }
                else
                {
                    return dic.TryGetValue(key, out value);
                }
            }
            else
            {
                return readOther.TryGetValue(key, out value);
            }


            value = default;
            return false;
        }

        /// <summary>
        /// Returns the value that is associated with the key.
        /// </summary>
        /// <returns>
        /// Maybe&lt;<typeparamref name="V"/>&gt;.Nothing if source is null or doesn't contain the key.
        /// Otherwise returns the encapsulated value associated with the key in the source
        /// </returns>
        /// <param name="source"> The dictionary that will be searched.</param>
        /// <param name="key"> The key.</param>
        /// <exception cref="ArgumentNullException">Thrown if key is null.</exception>
        public static Maybe<V> MaybeGet<K, V>(this IEnumerable<KeyValuePair<K, V>> source, Maybe<K> key)
            => (key.HasValue) ? DoMaybeGet(source, key.Value) : Maybe<V>.Nothing;

        /// <summary>
        /// Returns the value that is associated with the key.
        /// </summary>
        /// <returns>
        /// Maybe&lt;<typeparamref name="V"/>&gt;.Nothing if source is null or doesn't contain the key.
        /// Otherwise returns the encapsulated value associated with the key in the source
        /// </returns>
        /// <param name="source"> The dictionary that will be searched.</param>
        /// <param name="key"> The key.</param>
        /// <exception cref="ArgumentNullException">Thrown if key is null.</exception>
        public static Maybe<V> MaybeGet<K, V>(this IEnumerable<KeyValuePair<K, Maybe<V>>> source, K key)
            => DoMaybeGetOnDictionaryWithMaybeValues(source, key);

        private static Maybe<V> DoMaybeGetOnDictionaryWithMaybeValues<K, V>(this IEnumerable<KeyValuePair<K, Maybe<V>>> dictionary, K key)
            => DoGeneralTryGetValue(dictionary, key, out Maybe<V> value) ? value : Maybe<V>.Nothing;

        /// <summary>
        /// Returns the value that is associated with the key.
        /// </summary>
        /// <returns>
        /// Maybe&lt;<typeparamref name="V"/>&gt;.Nothing if source is null or doesn't contain the key.
        /// Otherwise returns the encapsulated value associated with the key in the source
        /// </returns>
        /// <param name="source"> The dictionary that will be searched.</param>
        /// <param name="key"> The key.</param>
        public static Maybe<V> MaybeGet<K, V>(this IDictionary<K, Maybe<V>> source, Maybe<K> key)
            => key.HasValue ? DoMaybeGetOnDictionaryWithMaybeValues(source, key.Value) : Maybe<V>.Nothing;

        #endregion

        #region Compact
        /// <summary>
        /// Extract the values from source, ignoring Maybe.Nothing.
        /// </summary>
        /// <returns>
        /// An IEnumerable&lt;<typeparamref name="T"/>&gt; with all the values in source, ignoring Maybe.Nothing.
        /// </returns>
        /// <param name="source"> The source.</param>
        public static IEnumerable<T> Compact<T>(this IEnumerable<Maybe<T>> source)
            => source.Where(m => m.HasValue).Select(m => m.Value);

        /// <summary>
        /// Extract the values from source, ignoring nulls.
        /// </summary>
        /// <returns>
        /// An IEnumerable&lt;<typeparamref name="T"/>&gt; with all the values in source, ignoring nulls.
        /// </returns>
        /// <param name="source"> The source.</param>
        public static IEnumerable<T> Compact<T>(this IEnumerable<T?> source)
            where T : struct
           => source.Where(m => m.HasValue).Select(m => m.Value);

        #endregion
    }
}
