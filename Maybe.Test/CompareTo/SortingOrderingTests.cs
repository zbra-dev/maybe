using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ZBRA.Maybe.Test.CompareTo
{
    public class SortingOrderingTests
    {
        [Theory]
        [MemberData(nameof(ComparableData))]
        public void ArraySort_ComparableData_ShouldSort<T>(
            Maybe<T>[] subject,
            Maybe<T>[] sortedSubjectAscending,
            Maybe<T>[] sortedSubjectDescending)
        {
            var sortedSubject = subject;

            Array.Sort(sortedSubject);

            sortedSubject.Should().BeEquivalentTo(sortedSubjectAscending, options => options.WithStrictOrdering());

            sortedSubject = sortedSubject.Reverse().ToArray();

            sortedSubject.Should().BeEquivalentTo(sortedSubjectDescending, options => options.WithStrictOrdering());
        }

        [Theory]
        [MemberData(nameof(ComparableData))]
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

        [Theory]
        [MemberData(nameof(NonComparableData))]
        public void ArraySort_NonComparableData_ShouldThrow<T>(Maybe<T>[] subject)
        {
            Action sortSubject = () => Array.Sort(subject);

            sortSubject
                .Should()
                .ThrowExactly<InvalidOperationException>()
                .WithMessage("Failed to compare two elements in the array.");
        }

        [Theory]
        [MemberData(nameof(NonComparableData))]
        public void OrderBy_NonComparableData_ShouldThrow<T>(Maybe<T>[] subject)
        {
            Action orderSubjectAscending = () => subject.OrderBy(it => it).ToArray();

            orderSubjectAscending
                .Should()
                .ThrowExactly<InvalidOperationException>()
                .WithMessage("Failed to compare two elements in the array.");
        }

        [Theory]
        [MemberData(nameof(NonComparableData))]
        public void OrderByDescending_NonComparableData_ShouldThrow<T>(Maybe<T>[] subject)
        {
            Action orderSubjectDescending = () => subject.OrderByDescending(it => it).ToArray();

            orderSubjectDescending
                .Should()
                .ThrowExactly<InvalidOperationException>()
                .WithMessage("Failed to compare two elements in the array.");
        }

        public static IEnumerable<object[]> ComparableData()
        {
            static object[] Create<T>(
                Maybe<T>[] subject,
                Maybe<T>[] sortedSubjectAscending,
                Maybe<T>[] sortedSubjectDescending)
            {
                return new object[]
                {
                    subject, sortedSubjectAscending, sortedSubjectDescending
                };
            }

            yield return Create(
                new[] { 2.ToMaybe(), 1.ToMaybe(), Maybe<int>.Nothing, Maybe<int>.Nothing, 3.ToMaybe() },
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

        public static IEnumerable<object[]> NonComparableData()
        {
            static object[] Create<T>(params T[] items)
            {
                return new object[] { items.Select(item => item.ToMaybe()).ToArray() };
            }

            yield return Create(new IntObj(1), new IntObj(2));
            yield return Create(new StringObj(""), new StringObj("1"));
            yield return Create(new NullableIntObj(null), new NullableIntObj(1));
            yield return Create(new MaybeIntObj(null), new MaybeIntObj(1));
        }
    }
}
