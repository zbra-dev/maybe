using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Maybe.Test.CompareTo
{
    public class SortingTests
    {
        [Theory]
        [MemberData(nameof(Sorted_TestData))]
        public void ArraySort_ComparableData_ShouldSort<T>(
            Maybe<T>[] subject, 
            Maybe<T>[] sortedSubjectAscending, 
            Maybe<T>[] sortedSubjectDescending)
        {
            var sortedSubject = subject;

            Array.Sort(sortedSubject);

            sortedSubject.Should().BeEquivalentTo(sortedSubjectAscending);

            sortedSubject = sortedSubject.Reverse().ToArray();

            sortedSubject.Should().BeEquivalentTo(sortedSubjectDescending);
        }

        [Theory]
        [MemberData(nameof(Sorted_TestData))]
        public void Ordering_ComparableData_ShouldSort<T>(
            Maybe<T>[] subject,
            Maybe<T>[] sortedSubjectAscending,
            Maybe<T>[] sortedSubjectDescending)
        {
            var sortedSubject = subject.OrderBy(it => it).ToArray();

            sortedSubject.Should().BeEquivalentTo(sortedSubjectAscending, options => options.WithStrictOrdering());

            sortedSubject = subject.OrderByDescending(it => it).ToArray();

            sortedSubject.Should().BeEquivalentTo(sortedSubjectDescending, options => options.WithStrictOrdering());
        }

        public static IEnumerable<object[]> Sorted_TestData()
        {
            static object[] Create<T>(
                Maybe<T>[] subject, 
                Maybe<T>[] sortedSubjectAscending, 
                Maybe<T> [] sortedSubjectDescending)
            {
                return new object[]
                {
                    subject, sortedSubjectAscending, sortedSubjectDescending
                };
            }

            yield return Create(
                new [] { 2.ToMaybe(), 1.ToMaybe(), Maybe<int>.Nothing, Maybe<int>.Nothing, 3.ToMaybe() },
                new[] { Maybe<int>.Nothing, Maybe<int>.Nothing, 1.ToMaybe(), 2.ToMaybe(), 3.ToMaybe() },
                new[] { 3.ToMaybe(), 2.ToMaybe(), 1.ToMaybe(), Maybe<int>.Nothing, Maybe<int>.Nothing });

            yield return Create(
                new[] { 2.ToMaybe(), 1.ToMaybe(), 3.ToMaybe() },
                new[] { 1.ToMaybe(), 2.ToMaybe(), 3.ToMaybe() },
                new[] { 3.ToMaybe(), 2.ToMaybe(), 1.ToMaybe() });

            yield return Create(
                new[] { 2.0.ToMaybe(), 1.0.ToMaybe(), 3.0.ToMaybe() },
                new[] { 1.0.ToMaybe(), 2.0.ToMaybe(), 3.0.ToMaybe() },
                new[] { 3.0.ToMaybe(), 2.0.ToMaybe(), 1.0.ToMaybe() });

            yield return Create(
                new[] { "2".ToMaybe(), "1".ToMaybe(), "3".ToMaybe() },
                new[] { "1".ToMaybe(), "2".ToMaybe(), "3".ToMaybe() },
                new[] { "3".ToMaybe(), "2".ToMaybe(), "1".ToMaybe() });

            yield return Create(
                new[] { '2'.ToMaybe(), '1'.ToMaybe(), '3'.ToMaybe() },
                new[] { '1'.ToMaybe(), '2'.ToMaybe(), '3'.ToMaybe() },
                new[] { '3'.ToMaybe(), '2'.ToMaybe(), '1'.ToMaybe() });
        }
    }
}
