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
            if (enumerable == null || !enumerable.Any())
            {
                return Maybe<T>.Nothing;
            }
            return enumerable.Single().ToMaybe();
        }

        public static Maybe<T> MaybeSingle<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null || !enumerable.Any())
            {
                return Maybe<T>.Nothing;
            }
            return enumerable.Single().ToMaybe();
        }

        public static Maybe<T> MaybeSingle<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            return MaybeSingle(enumerable?.Where(predicate));
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

        public static Maybe<V> MaybeGet<K, V>(this IEnumerable<KeyValuePair<K, V>> dictionary, K key)
        {
            return DoMaybeGet(dictionary, key);
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

        public static Maybe<V> MaybeGet<K, V>(this IEnumerable<KeyValuePair<K, V>> dictionary, Maybe<K> key)
            => (key.HasValue) ? DoMaybeGet(dictionary, key.Value) : Maybe<V>.Nothing;


        public static Maybe<V> MaybeGet<K, V>(this IEnumerable<KeyValuePair<K, Maybe<V>>> dictionary, K key)
            => DoMaybeGetOnDictionaryWithMaybeValues(dictionary, key);


        private static Maybe<V> DoMaybeGetOnDictionaryWithMaybeValues<K, V>(this IEnumerable<KeyValuePair<K, Maybe<V>>> dictionary, K key)
            => DoGeneralTryGetValue(dictionary, key, out Maybe<V> value) ? value : Maybe<V>.Nothing;


        public static Maybe<V> MaybeGet<K, V>(this IDictionary<K, Maybe<V>> dictionary, Maybe<K> key)
            => key.HasValue ? DoMaybeGetOnDictionaryWithMaybeValues(dictionary, key.Value) : Maybe<V>.Nothing;

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
