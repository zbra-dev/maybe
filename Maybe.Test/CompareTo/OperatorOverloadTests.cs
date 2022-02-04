using FluentAssertions;
using System;
using System.Collections;
using System.Linq;
using Xunit;

namespace ZBRA.Maybe.Test.CompareTo
{
    public class OperatorOverloadTests
    {
        [Theory]
        [MemberData(nameof(GreaterThan_TestData))]
        public void GreaterThan_ComparableData_ShouldPass<T>(T value, T otherValue)
        {
            var subject = value.ToMaybe();
            var otherSubject = otherValue.ToMaybe();

            IsGreaterThan(subject, otherSubject).Should().BeTrue();
            IsGreaterThanOrEqual(subject, otherSubject).Should().BeTrue();
            IsLessThan(subject, otherSubject).Should().BeFalse();
            IsLessThanOrEqual(subject, otherSubject).Should().BeFalse();
        }

        private bool IsGreaterThan<T>(Maybe<T> subject, Maybe<T> otherSubject) => subject > otherSubject;
        private bool IsGreaterThanOrEqual<T>(Maybe<T> subject, Maybe<T> otherSubject) => subject >= otherSubject;
        private bool IsLessThan<T>(Maybe<T> subject, Maybe<T> otherSubject) => subject < otherSubject;
        private bool IsLessThanOrEqual<T>(Maybe<T> subject, Maybe<T> otherSubject) => subject <= otherSubject;

        [Theory]
        [MemberData(nameof(LessThan_TestData))]
        public void LessThan_ComparableData_ShouldPass<T>(T value, T otherValue)
        {
            var subject = value.ToMaybe();
            var otherSubject = otherValue.ToMaybe();

            IsGreaterThan(subject, otherSubject).Should().BeFalse();
            IsGreaterThanOrEqual(subject, otherSubject).Should().BeFalse();
            IsLessThan(subject, otherSubject).Should().BeTrue();
            IsLessThanOrEqual(subject, otherSubject).Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(Equal_TestData))]
        public void Equal_ComparableData_ShouldPass<T>(T value, T otherValue)
        {
            var subject = value.ToMaybe();
            var otherSubject = otherValue.ToMaybe();

            IsGreaterThan(subject, otherSubject).Should().BeFalse();
            IsGreaterThanOrEqual(subject, otherSubject).Should().BeTrue();
            IsLessThan(subject, otherSubject).Should().BeFalse();
            IsLessThanOrEqual(subject, otherSubject).Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(NonComparableData))]
        public void GreaterThan_NonComparableData_ShouldThrow<T>(T value, T otherValue)
        {
            var subject = value.ToMaybe();
            var otherSubject = otherValue.ToMaybe();

            Func<bool> isGreaterThan = () => subject > otherSubject;

            isGreaterThan
                .Should()
                .ThrowExactly<ArgumentException>()
                .WithMessage("At least one object must implement IComparable.");
        }

        [Theory]
        [MemberData(nameof(NonComparableData))]
        public void GreaterThanOrEqual_NonComparableData_ShouldThrow<T>(T value, T otherValue)
        {
            var subject = value.ToMaybe();
            var otherSubject = otherValue.ToMaybe();

            Func<bool> isGreaterThanOrEqual = () => subject >= otherSubject;

            isGreaterThanOrEqual
                .Should()
                .ThrowExactly<ArgumentException>()
                .WithMessage("At least one object must implement IComparable.");
        }

        [Theory]
        [MemberData(nameof(NonComparableData))]
        public void LessThan_NonComparableData_ShouldThrow<T>(T value, T otherValue)
        {
            var subject = value.ToMaybe();
            var otherSubject = otherValue.ToMaybe();

            Func<bool> isLessThan = () => subject < otherSubject;

            isLessThan
                .Should()
                .ThrowExactly<ArgumentException>()
                .WithMessage("At least one object must implement IComparable.");
        }

        [Theory]
        [MemberData(nameof(NonComparableData))]
        public void LessThanOrEqual_NonComparableData_ShouldThrow<T>(T value, T otherValue)
        {
            var subject = value.ToMaybe();
            var otherSubject = otherValue.ToMaybe();

            Func<bool> isLessThanOrEqual = () => subject <= otherSubject;

            isLessThanOrEqual
                .Should()
                .ThrowExactly<ArgumentException>()
                .WithMessage("At least one object must implement IComparable.");
        }

        public static TheoryData<object, object> GreaterThan_TestData()
        {
            var now = DateTime.Now;

            var data = new TheoryData<object, object>()
            {
                { '2', '1' },
                { 2, 1 },
                { 2.0, 1.0 },
                { "b", "a" },
                {  now.AddDays(1), now },
            };

            Numbers.ConstantPairs
                .Where(it => Comparer.Default.Compare(it.A, it.B) > 0)
                .ForEach(it => data.Add(it.A, it.B));

            return data;
        }

        public static TheoryData<object, object> LessThan_TestData()
        {
            var now = DateTime.Now;

            var data = new TheoryData<object, object>()
            {
                { '1', '2' },
                { 1, 2 },
                { 1.0, 2.0 },
                { "a", "b" },
                {  now, now.AddDays(1) },
            };

            Numbers.ConstantPairs
                .Where(it => Comparer.Default.Compare(it.A, it.B) < 0)
                .ForEach(it => data.Add(it.A, it.B));

            return data;
        }

        public static TheoryData<object, object> Equal_TestData()
        {
            var now = DateTime.Now;

            var data = new TheoryData<object, object>()
            {
                { '2', '2' },
                { 2, 2 },
                { 2.0, 2.0 },
                { "b", "b" },
                {  now, now },
            };

            Numbers.ConstantPairs
                .Where(it => Comparer.Default.Compare(it.A, it.B) == 0)
                .ForEach(it => data.Add(it.A, it.B));

            return data;
        }

        public static TheoryData<object, object> NonComparableData()
        {
            return new TheoryData<object, object>()
            {
                { new IntObj(1), new IntObj(2) },
                { new StringObj(""), new StringObj("1") },
                { new NullableIntObj(null), new NullableIntObj(1) },
                { new MaybeIntObj(null), new MaybeIntObj(1) },
            };
        }
    }
}
