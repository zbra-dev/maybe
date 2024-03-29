using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ZBRA.Maybe.Test
{
    public class MaybeTests
    {
        [Theory]
        [InlineData(1, true)]
        [InlineData(null, false)]
        public void HasValue_ShouldReturnCorrectly(int? value, bool expected)
        {
            Maybe<int> maybe = value.ToMaybe();

            maybe.HasValue.Should().Be(expected);
        }

        [Fact]
        public void Nothing_DirectAccessToValue_ShouldThrow()
        {
            Maybe<int> maybe = Maybe<int>.Nothing;

            Func<int> func = () => maybe.Value;

            func.Should().ThrowExactly<Exception>().WithMessage("*No Value is present");
        }

        [Theory]
        [InlineData(1, 2, 1)]
        [InlineData(null, 2, 2)]
        public void Or_ShouldReturnValueOrDefaultValue(int? value, int defaultValue, int expected)
        {
            var result = value.ToMaybe()
                .Or(defaultValue);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(1, 2, 1)]
        [InlineData(null, 2, 2)]
        public void Or_Func_ShouldReturnValueOrDefaultValue(int? value, int defaultValue, int expected)
        {
            var result = value.ToMaybe()
                .Or(() => defaultValue);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(1, 2, 1)]
        [InlineData(null, 2, 2)]
        public async Task OrAsync_Func_ShouldReturnValueOrDefaultValue(int? value, int defaultValue, int expected)
        {
            var result = await value.ToMaybe()
                .OrAsync(async () => await Task.FromResult(defaultValue));

            result.Should().Be(expected);
        }

        [Fact]
        public void Or_Func_NullArgument_ShouldThrow()
        {
            Action subject = () => 1.ToMaybe().Or((Func<int>)null);

            subject.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void OrMaybe_FuncMaybe_NullArgument_ShouldThrow()
        {
            Action subject = () => 1.ToMaybe().OrMaybe((Func<Maybe<int>>)null);

            subject.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void OrThrow_WithNoValue_ShouldThrowException()
        {
            var subject = Maybe<int>.Nothing;

            subject.Invoking(s => s.OrThrow(() => new ArgumentException()))
                .Should()
                .ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void OrThrow_WithValue_ShouldNotThrowException()
        {
            var expected = 1;

            var result = expected.ToMaybe()
                .OrThrow(() => new ArgumentException());

            result.Should().Be(expected);
        }

        [Fact]
        public void OrThrow_NullArgument_ShouldThrow()
        {
            Action subject = () => 1.ToMaybe().OrThrow(null);

            subject.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Equals_WhenObjectReferencesAreEqual()
        {
            var a = new object().ToMaybe();
            var b = new object().ToMaybe();
            (a == b).Should().BeFalse();
            a = b;
            (a == b).Should().BeTrue();
        }

        [Theory]
        [InlineData(null, null, true)]
        [InlineData(null, 0, false)]
        [InlineData(0, null, false)]
        [InlineData(1, 1, true)]
        [InlineData(1, 2, false)]
        public void Equals_WhenObjectsAreEqual_ShouldReturnTrue(int? value, int? otherValue, bool expected)
        {
            value.ToMaybe().Equals(otherValue.ToMaybe()).Should().Be(expected);
        }

        [Theory]
        [InlineData(null, null, true)]
        [InlineData(null, 0, false)]
        [InlineData(0, null, false)]
        [InlineData(1, 1, true)]
        [InlineData(1, 2, false)]
        public void EqualityOperator_WhenObjectsAreEqual_ShouldReturnTrue(int? value, int? otherValue, bool expected)
        {
            static bool AreEqual(int? value, int? otherValue) => value.ToMaybe() == otherValue.ToMaybe();
            static bool AreDifferent(int? value, int? otherValue) => value.ToMaybe() != otherValue.ToMaybe();

            AreEqual(value, otherValue).Should().Be(expected);
            AreDifferent(value, otherValue).Should().NotBe(expected);
        }

        [Fact]
        public void Equals_WhenTypesAreDifferent_ShouldReturnFalse()
        {
            var value = 1.ToMaybe();
            var otherValue = 1.0.ToMaybe();
            value.Equals(otherValue).Should().BeFalse();
        }

        [Theory]
        [InlineData(null, null, false)]
        [InlineData(null, "1", false)]
        [InlineData("1", null, false)]
        [InlineData("1", "1", true)]
        [InlineData("1", "2", false)]
        public void Is_WithoutPredicate_ShouldMatchValueWithEquals(string value, string otherValue, bool expected)
        {
            value.ToMaybe().Is(otherValue).Should().Be(expected);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("1", false)]
        [InlineData("2", true)]
        public void Is_WithPredicate_ShouldMatchValueWithPredicate(string value, bool expected)
        {
            static bool predicate(string s) => s == null || int.Parse(s) > 1;
            value.ToMaybe().Is(predicate).Should().Be(expected);
        }

        [Fact]
        public void Is_NullArgument_ShouldThrow()
        {
            Action subject = () => 1.ToMaybe().Is(null);

            subject.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("1", null)]
        [InlineData("2", "2")]
        public void Where_WithPredicate_ShouldApplyPredicate(string value, string expected)
        {
            static bool predicate(string s) => s == null || int.Parse(s) > 1;
            var result = value.ToMaybe()
                .Where(predicate);

            result.Should().Be(expected.ToMaybe());
        }

        [Fact]
        public void Where_NullArgument_ShouldThrow()
        {
            Action subject = () => 1.ToMaybe().Where(null);

            subject.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(OrMaybe_WithAlternative_ShouldReturnSubjectOrAlternativeTestCases))]
        public void OrMaybe_WithAlternative_ShouldReturnSubjectOrAlternative(Maybe<StringObj> subject, string alternative, string expected)
        {
            var result = subject
                .Select(s => s.Name)
                .OrMaybe(alternative.ToMaybe);
            result.Should().Be(expected.ToMaybe());

            result = subject
                .Select(s => s.Name)
                .OrMaybe(alternative.ToMaybe());
            result.Should().Be(expected.ToMaybe());

            result = subject
                .Select(s => s.Name)
                .OrMaybe(alternative);
            result.Should().Be(expected.ToMaybe());

            result = subject
                .Select(s => s.Name)
                .OrMaybe(() => alternative);
            result.Should().Be(expected.ToMaybe());
        }

        [Theory]
        [MemberData(nameof(OrMaybe_WithAlternative_ShouldReturnSubjectOrAlternativeTestCases))]
        public async Task OrMaybeAsync_WithAlternative_ShouldReturnSubjectOrAlternative(Maybe<StringObj> subject, string alternative, string expected)
        {
            var result = await subject
                .Select(s => s.Name)
                .OrMaybeAsync(async () => await Task.FromResult(alternative));
            result.Should().Be(expected.ToMaybe());
        }

        public static TheoryData<Maybe<StringObj>, string, string> OrMaybe_WithAlternative_ShouldReturnSubjectOrAlternativeTestCases()
        {
            return new TheoryData<Maybe<StringObj>, string, string>
            {
                { Maybe<StringObj>.Nothing, null, null },
                { Maybe<StringObj>.Nothing, "alternative", "alternative" },
                { new StringObj("subject").ToMaybe(), null, "subject" },
                { new StringObj("subject").ToMaybe(), "alternative", "subject" },
            };
        }

        [Fact]
        public void OrAsync_NullArgument_ShouldThrow()
        {
            Func<Task> subject = async () => await 1.ToMaybe().OrAsync(null);

            subject.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void OrMaybe_Func_MaybeNullArgument_ShouldThrow()
        {
            Action subject = () => 1.ToMaybe().OrMaybe((Func<int>)null);

            subject.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void OrMaybeAsync_Func_MaybeNullArgument_ShouldThrow()
        {
            Func<Task> subject = async () => await 1.ToMaybe().OrMaybeAsync(null);

            subject.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void ImplicitConversion_FromNullObject_ShouldReturnMaybeWithoutValue()
        {
            Maybe<object> subject = null;

            subject.Should().Be(Maybe<object>.Nothing);
        }

        [Theory]
        [MemberData(nameof(NonNullData))]
        public void ImplicitConversion_FromNonNullObject_ShouldReturnMaybeWithValue<T>(T value)
        {
            Maybe<T> subject = value;

            subject.Value.Should().Be(value);
        }

        public static TheoryData<object> NonNullData()
        {
            return new TheoryData<object>()
            {
                '1',
                1,
                0,
                1.0,
                DateTime.Now,
                new List<string>(),
                1.ToMaybe(),
                (1, 2),
                "1",
                true,
                (byte)1,
                (sbyte)1,
                (float)1.0,
                (uint)1,
                (long)1,
                (ulong)1,
                (short)1,
                (ushort)1,
            };
        }
    }
}
