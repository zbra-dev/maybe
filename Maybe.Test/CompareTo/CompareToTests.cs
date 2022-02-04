using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Maybe.Test.CompareTo
{
    public class CompareToTests
    {
        [Theory]
        [MemberData(nameof(ComparableData))]
#pragma warning disable xUnit1026 // does not use parameter _
        public void CompareTo_BothNothing_ResultShouldBeZero<T>(T _)
#pragma warning restore xUnit1026 // does not use parameter _
        {
            var subject = Maybe<T>.Nothing;
            var otherSubject = Maybe<T>.Nothing;

            var comparisonResult = subject.CompareTo(otherSubject);

            comparisonResult.Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(ComparableData))]
        public void CompareTo_SubjectNothingOtherSomething_ResultShoulBeMinusOne<T>(T otherValue)
        {
            var subject = Maybe<T>.Nothing;
            var otherSubject = otherValue.ToMaybe();

            var comparisonResult = subject.CompareTo(otherSubject);

            comparisonResult.Should().Be(-1);
        }

        [Theory]
        [MemberData(nameof(ComparableData))]
        public void CompareTo_SubjectSomethingOtherNothing_ResultShouldBeOne<T>(T value)
        {
            var subject = value.ToMaybe();
            var otherSubject = Maybe<T>.Nothing;

            var comparisonResult = subject.CompareTo(otherSubject);

            comparisonResult.Should().Be(1);
        }

        [Theory]
        [MemberData(nameof(ComparableData))]
        public void CompareTo_SameSubjects_ResultShouldBeZero<T>(T value)
        {
            var subject = value.ToMaybe();
            var otherSubject = value.ToMaybe();

            var comparisonResult = subject.CompareTo(otherSubject);

            comparisonResult.Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(NonComparableData))]
        public void CompareTo_NonComparableData_ShouldThrow<T>(T value, T otherValue)
        {
            var subject = value.ToMaybe();
            var otherSubject = otherValue.ToMaybe();

            Func<int> comparison = () => subject.CompareTo(otherSubject);

            comparison
                .Should()
                .ThrowExactly<ArgumentException>()
                .WithMessage("At least one object must implement IComparable.");
        }

        public static IEnumerable<object[]> ComparableData()
        {
            var data = new TheoryData<object>()
            {
                '1',
                1,
                1.0,
                DateTime.Now,
                new List<string>(),
                1.ToMaybe(),
                (1, 2),
                "1",
                0,
                -1,
            };

            Numbers.Constants.ForEach(data.Add);

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
