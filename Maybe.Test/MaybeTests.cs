using FluentAssertions;
using System;
using Xunit;

namespace Maybe.Test
{
    public class MaybeTests
    {
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
        public void OrGet_ShouldReturnValueOrDefaultValue(int? value, int defaultValue, int expected)
        {
            var result = value.ToMaybe()
                .OrGet(() => defaultValue);

            result.Should().Be(expected);
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
        public void Consume_WithValue_ShouldExecuteAction()
        {
            string result = null;
            var expected = 1;

            expected.ToMaybe()
                .Consume(i => result = i.ToString());

            result.Should().Be(expected.ToString());
        }

        [Fact]
        public void Consume_WithNoValue_ShouldNotExecuteAction()
        {
            var result = "a";

            Maybe<string>.Nothing
                .Consume(i => result = "b");

            result.Should().Be("a");
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, 2, null)]
        [InlineData(2, null, null)]
        [InlineData(1, 2, "3")]
        public void Zip_WithTransformer_ShouldZipValues(int? value, double? otherValue, string expected)
        {
            static string transformer(int v, double o) => (v + o).ToString();

            var result = value.ToMaybe()
                .Zip(otherValue.ToMaybe(), transformer);

            result.Should().Be(expected.ToMaybe());
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, 2, null)]
        [InlineData(2, null, null)]
        [InlineData(1, 2, "3")]
        public void Zip_WithMaybeTransformer_ShouldZipValues(int? value, double? otherValue, string expected)
        {
            static Maybe<string> transformer(int v, double o) => (v + o).ToString().ToMaybe();

            var result = value.ToMaybe()
                .Zip(otherValue.ToMaybe(), transformer);

            result.Should().Be(expected.ToMaybe());
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, 2, null)]
        [InlineData(2, null, null)]
        [InlineData(1, 2, "3")]
        public void ZipConsume_WhenBothValuesExist_ShouldExecuteAction(int? value, double? otherValue, string expected)
        {
            string result = null;
            void action(int v, double o) => result = (v + o).ToString();

            value.ToMaybe()
                .ZipAndConsume(otherValue.ToMaybe(), action);

            result.Should().Be(expected);
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
    }
}
