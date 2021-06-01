using FluentAssertions;
using Xunit;

namespace Maybe.Test
{
    public class MaybeExtensionsTests
    {
        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData("value", "value")]
        public void OrEmpty_ReturnsValueOrEmpty(string subject, string expected)
        {
            var result = subject.ToMaybe()
                .OrEmpty();

            result.Should().Be(expected);
        }

        [Fact]
        public void OrTrue_ReturnsValueOrTrue()
        {
            var result = Maybe<bool>.Nothing
                .OrTrue();
            result.Should().BeTrue();

            result = true.ToMaybe()
                .OrTrue();
            result.Should().BeTrue();

            result = false.ToMaybe()
                .OrTrue();
            result.Should().BeFalse();
        }

        [Fact]
        public void OrFalse_ReturnsValueOrFalse()
        {
            var result = Maybe<bool>.Nothing
                .OrFalse();
            result.Should().BeFalse();

            result = false.ToMaybe()
               .OrFalse();
            result.Should().BeFalse();

            result = true.ToMaybe()
                .OrFalse();
            result.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(Select_WithNonNullablePropertyTestCases))]
        public void Select_WithNonNullableProperty_ShouldReturnSelectedProperty(Maybe<IntObj> subject, Maybe<int> expected)
        {
            var result = subject.Select(i => i.Count);

            result.Should().Be(expected);
        }

        public static TheoryData<Maybe<IntObj>, Maybe<int>> Select_WithNonNullablePropertyTestCases()
        {
            return new TheoryData<Maybe<IntObj>, Maybe<int>>
            {
                { Maybe<IntObj>.Nothing, Maybe<int>.Nothing },
                { new IntObj(1).ToMaybe(), 1.ToMaybe() },
            };
        }

        [Theory]
        [MemberData(nameof(Select_WithNullablePropertyTestCases))]
        public void Select_WithNullableProperty_ReturnsSelectedProperty(Maybe<NullableIntObj> subject, Maybe<int> expected)
        {
            var result = subject.Select(o => o.Count);

            result.Should().Be(expected);
        }

        public static TheoryData<Maybe<NullableIntObj>, Maybe<int>> Select_WithNullablePropertyTestCases()
        {
            return new TheoryData<Maybe<NullableIntObj>, Maybe<int>>
            {
                { Maybe<NullableIntObj>.Nothing, Maybe<int>.Nothing },
                { new NullableIntObj(null).ToMaybe(), Maybe<int>.Nothing },
                { new NullableIntObj(1).ToMaybe(), 1.ToMaybe() },
            };
        }

        [Theory]
        [MemberData(nameof(SelectMany_WithMaybePropertyTestCases))]
        public void SelectMany_WithMaybeProperty_ReturnsSelectedProperty(Maybe<MaybeIntObj> subject, Maybe<int> expected)
        {
            var result = subject.SelectMany(o => o.Count);

            result.Should().Be(expected);
        }

        public static TheoryData<Maybe<MaybeIntObj>, Maybe<int>> SelectMany_WithMaybePropertyTestCases()
        {
            return new TheoryData<Maybe<MaybeIntObj>, Maybe<int>>
            {
                { Maybe<MaybeIntObj>.Nothing, Maybe<int>.Nothing },
                { new MaybeIntObj(null).ToMaybe(), Maybe<int>.Nothing },
                { new MaybeIntObj(1).ToMaybe(), 1.ToMaybe() },
            };
        }

        [Theory]
        [MemberData(nameof(OrGetAlternative_WithAlternative_ShouldReturnSubjectOrAlternativeTestCases))]
        public void OrGetAlternative_WithAlternative_ShouldReturnSubjectOrAlternative(Maybe<StringObj> subject, string alternative, string expected)
        {
            var result = subject
                .Select(s => s.Name)
                .OrGetAlternative(() => alternative.ToMaybe());
            result.Should().Be(expected.ToMaybe());

            result = subject
                .Select(s => s.Name)
                .OrGetAlternative(() => alternative);
            result.Should().Be(expected.ToMaybe());
        }

        public static TheoryData<Maybe<StringObj>, string, string> OrGetAlternative_WithAlternative_ShouldReturnSubjectOrAlternativeTestCases()
        {
            return new TheoryData<Maybe<StringObj>, string, string>
            {
                { Maybe<StringObj>.Nothing, null, null },
                { Maybe<StringObj>.Nothing, "alternative", "alternative" },
                { new StringObj("subject").ToMaybe(), null, "subject" },
                { new StringObj("subject").ToMaybe(), "alternative", "subject" },
            };
        }
    }
}
