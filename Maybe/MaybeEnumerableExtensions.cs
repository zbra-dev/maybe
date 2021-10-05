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
            => subject.Or(() => Enumerable.Empty<T>());

        /// <summary>
        /// Returns the encapsulated value or an empty collection.
        /// </summary>
        /// <returns>
        /// An empty collection if subject.HasValue is false, otherwise subject.Value
        /// </returns>
        /// <param name="subject"> The subject.</param>
        public static ICollection<T> OrEmpty<T>(this Maybe<ICollection<T>> subject)
            => subject.Or(() => new HashSet<T>());

        /// <summary>
        /// Returns the encapsulated value or an empty read only collection.
        /// </summary>
        /// <returns>
        /// An empty read only collection if subject.HasValue is false, otherwise subject.Value
        /// </returns>
        /// <param name="subject"> The subject.</param>
        public static IReadOnlyCollection<T> OrEmpty<T>(this Maybe<IReadOnlyCollection<T>> subject)
            => subject.Or(() => new HashSet<T>());

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
            => subject.Or(() => new T[0]);

        /// <summary>
        /// Returns the encapsulated value or an empty set.
        /// </summary>
        /// <returns>
        /// An empty set if subject.HasValue is false, otherwise subject.Value
        /// </returns>
        /// <param name="subject"> The subject.</param>
        public static ISet<T> OrEmpty<T>(this Maybe<ISet<T>> subject)
            => subject.Or(() => new HashSet<T>());

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
        /// Returns <see cref="Maybe{T}" /> of the only element of a sequence or <see cref="Maybe{T}.Nothing" /> 
        /// if the sequence is empty or the only element is <see langword="null" />.<br/>
        /// This method throws an <see cref="ArgumentNullException"/> if there is more than one element in the sequence.
        /// </summary>
        /// <returns>
        /// <see cref="Maybe{T}" /> of the only element in the sequence or <see cref="Maybe{T}.Nothing" /> if the sequence is empty.<br/>
        /// <see cref="Maybe{T}.Nothing" /> is also returned if <paramref name="source"/> is <see langword="null" /> or the only element is <see langword="null" />.
        /// </returns>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to return a single element from.</param>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="source"/> contains more than one element.</exception>
        public static Maybe<T> MaybeSingle<T>(this IEnumerable<T?> source) where T : struct
        {
            if (source == null)
            {
                return Maybe<T>.Nothing;
            }

            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;

                    if (enumerator.MoveNext())
                    {
                        throw new InvalidOperationException("Sequence contains more than one element");
                    }

                    return current.ToMaybe();
                }

                return Maybe<T>.Nothing;
            }
        }

        /// <summary>
        /// Returns the only element of a sequence that satisfies a specified condition or 
        /// <see cref="Maybe{T}.Nothing" /> if no such element exists or it is <see langword="null" />.<br/> 
        /// This method throws an <see cref="ArgumentNullException"/> if more than one element satisfies the condition.
        /// </summary>
        /// <returns>
        /// <see cref="Maybe{T}" /> of the only element that satisfies a specified condition or <see cref="Maybe{T}.Nothing" /> if no such element is found.<br/>
        /// <see cref="Maybe{T}.Nothing" /> is also returned if <paramref name="source"/> is empty, <see langword="null" /> or the only element found is <see langword="null" />.
        /// </returns>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to return a single element from.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="source"/> contains more than one element.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="predicate" /> is <see langword="null" />.</exception>
        public static Maybe<T> MaybeSingle<T>(this IEnumerable<T?> source, Func<T?, bool> predicate) where T : struct
        {
            if (source == null)
            {
                return Maybe<T>.Nothing;
            }

            predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));

            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;

                    if (predicate(current))
                    {
                        while (enumerator.MoveNext())
                        {
                            if (predicate(enumerator.Current))
                            {
                                throw new InvalidOperationException("Sequence contains more than one matching element");
                            }
                        }

                        return current.ToMaybe();
                    }
                }

                return Maybe<T>.Nothing;
            }
        }

        /// <summary>
        /// Returns <see cref="Maybe{T}" /> of the only element of a sequence or <see cref="Maybe{T}.Nothing" /> 
        /// if the sequence is empty or the only element is <see langword="null" />.<br/>
        /// This method throws an <see cref="ArgumentNullException"/> if there is more than one element in the sequence.
        /// </summary>
        /// <returns>
        /// <see cref="Maybe{T}" /> of the only element in the sequence or <see cref="Maybe{T}.Nothing" /> if the sequence is empty.<br/>
        /// <see cref="Maybe{T}.Nothing" /> is also returned if <paramref name="source"/> is <see langword="null" /> or the only element is <see langword="null" />.
        /// </returns>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to return a single element from.</param>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="source"/> contains more than one element.</exception>
        public static Maybe<T> MaybeSingle<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                return Maybe<T>.Nothing;
            }

            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;

                    if (enumerator.MoveNext())
                    {
                        throw new InvalidOperationException("Sequence contains more than one element");
                    }

                    return current.ToMaybe();
                }

                return Maybe<T>.Nothing;
            }
        }

        /// <summary>
        /// Returns the only element of a sequence that satisfies a specified condition or 
        /// <see cref="Maybe{T}.Nothing" /> if no such element exists or it is <see langword="null" />.<br/> 
        /// This method throws an <see cref="ArgumentNullException"/> if more than one element satisfies the condition.
        /// </summary>
        /// <returns>
        /// <see cref="Maybe{T}" /> of the only element that satisfies a specified condition or <see cref="Maybe{T}.Nothing" /> if no such element is found.<br/>
        /// <see cref="Maybe{T}.Nothing" /> is also returned if <paramref name="source"/> is empty, <see langword="null" /> or the only element found is <see langword="null" />.
        /// </returns>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to return a single element from.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="source"/> contains more than one element.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="predicate" /> is <see langword="null" />.</exception>
        public static Maybe<T> MaybeSingle<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
            {
                return Maybe<T>.Nothing;
            }

            predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));

            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;

                    if (predicate(current))
                    {
                        while (enumerator.MoveNext())
                        {
                            if (predicate(enumerator.Current))
                            {
                                throw new InvalidOperationException("Sequence contains more than one matching element");
                            }
                        }

                        return current.ToMaybe();
                    }
                }

                return Maybe<T>.Nothing;
            }
        }

        /// <summary>
        /// Returns <see cref="Maybe{T}" /> of the first element of a sequence or <see cref="Maybe{T}.Nothing" /> 
        /// if the sequence is empty or the first element is <see langword="null" />.<br/>
        /// </summary>
        /// <returns>
        /// <see cref="Maybe{T}" /> of the first element in the sequence or <see cref="Maybe{T}.Nothing" /> if the sequence is empty.<br/>
        /// <see cref="Maybe{T}.Nothing" /> is also returned if <paramref name="source"/> is <see langword="null" /> or the first element is <see langword="null" />.
        /// </returns>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to return the first element from.</param>
        public static Maybe<T> MaybeFirst<T>(this IEnumerable<T?> source) where T : struct
        {
            if (source == null)
            {
                return Maybe<T>.Nothing;
            }

            foreach (var element in source)
            {
                return element.ToMaybe();
            }

            return Maybe<T>.Nothing;
        }

        /// <summary>
        /// Returns the first element of a sequence that satisfies a specified condition or 
        /// <see cref="Maybe{T}.Nothing" /> if no such element exists or it is <see langword="null" />.<br/>
        /// </summary>
        /// <returns>
        /// <see cref="Maybe{T}" /> of the first element that satisfies a specified condition or <see cref="Maybe{T}.Nothing" /> if no such element is found.<br/>
        /// <see cref="Maybe{T}.Nothing" /> is also returned if <paramref name="source"/> is empty, <see langword="null" /> or the first element found is <see langword="null" />.
        /// </returns>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to return the first element from.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="predicate" /> is <see langword="null" />.</exception>
        public static Maybe<T> MaybeFirst<T>(this IEnumerable<T?> source, Func<T?, bool> predicate) where T : struct
        {
            if (source == null)
            {
                return Maybe<T>.Nothing;
            }

            predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));

            foreach (var element in source)
            {
                if (predicate(element))
                {
                    return element.ToMaybe();
                }
            }

            return Maybe<T>.Nothing;
        }

        /// <summary>
        /// Returns <see cref="Maybe{T}" /> of the first element of a sequence or <see cref="Maybe{T}.Nothing" /> 
        /// if the sequence is empty or the first element is <see langword="null" />.<br/>
        /// </summary>
        /// <returns>
        /// <see cref="Maybe{T}" /> of the first element in the sequence or <see cref="Maybe{T}.Nothing" /> if the sequence is empty.<br/>
        /// <see cref="Maybe{T}.Nothing" /> is also returned if <paramref name="source"/> is <see langword="null" /> or the first element is <see langword="null" />.
        /// </returns>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to return the first element from.</param>
        public static Maybe<T> MaybeFirst<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                return Maybe<T>.Nothing;
            }

            foreach (var element in source)
            {
                return element.ToMaybe();
            }

            return Maybe<T>.Nothing;
        }

        /// <summary>
        /// Returns the first element of a sequence that satisfies a specified condition or 
        /// <see cref="Maybe{T}.Nothing" /> if no such element exists or it is <see langword="null" />.<br/>
        /// </summary>
        /// <returns>
        /// <see cref="Maybe{T}" /> of the first element that satisfies a specified condition or <see cref="Maybe{T}.Nothing" /> if no such element is found.<br/>
        /// <see cref="Maybe{T}.Nothing" /> is also returned if <paramref name="source"/> is empty, <see langword="null" /> or the first element found is <see langword="null" />.
        /// </returns>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to return the first element from.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="predicate" /> is <see langword="null" />.</exception>
        public static Maybe<T> MaybeFirst<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
            {
                return Maybe<T>.Nothing;
            }

            predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));

            foreach (var element in source)
            {
                if (predicate(element))
                {
                    return element.ToMaybe();
                }
            }

            return Maybe<T>.Nothing;
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
        public static Maybe<V> MaybeGet<K, V>(this IDictionary<K, V> source, K key)
        {
            if (source == null)
            {
                return Maybe<V>.Nothing;
            }

            return source.TryGetValue(key, out var value) ? value.ToMaybe() : Maybe<V>.Nothing;
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
        public static Maybe<V> MaybeGet<K, V>(this IDictionary<K, V> source, Maybe<K> key)
        {
            if (source == null)
            {
                return Maybe<V>.Nothing;
            }

            return key.HasValue && source.TryGetValue(key.Value, out var value) ? value.ToMaybe() : Maybe<V>.Nothing;
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
        public static Maybe<V> MaybeGet<K, V>(this IDictionary<K, Maybe<V>> source, K key)
        {
            if (source == null)
            {
                return Maybe<V>.Nothing;
            }

            return source.TryGetValue(key, out var value) ? value : Maybe<V>.Nothing;
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
        public static Maybe<V> MaybeGet<K, V>(this IDictionary<K, Maybe<V>> source, Maybe<K> key)
        {
            if (source == null)
            {
                return Maybe<V>.Nothing;
            }

            return key.HasValue && source.TryGetValue(key.Value, out var value) ? value : Maybe<V>.Nothing;
        }
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
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            return source.Where(m => m.HasValue).Select(m => m.Value);
        }

        /// <summary>
        /// Extract the values from source, ignoring nulls.
        /// </summary>
        /// <returns>
        /// An IEnumerable&lt;<typeparamref name="T"/>&gt; with all the values in source, ignoring nulls.
        /// </returns>
        /// <param name="source"> The source.</param>
        public static IEnumerable<T> Compact<T>(this IEnumerable<T?> source)
            where T : struct
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            return source.Where(m => m.HasValue).Select(m => m.Value);
        }

        #endregion
    }
}
