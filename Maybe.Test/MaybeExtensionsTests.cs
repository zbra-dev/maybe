using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ZBRA.Maybe.Test
{
    public class MaybeExtensionsTests
    {
        [Fact]
        public void ToMaybe_WithMaybeSubject_ReturnsItself()
        {
            var subject = 1.ToMaybe();
            var result = subject.ToMaybe();
            result.Should().Be(subject);

            subject = Maybe<int>.Nothing;
            result = subject.ToMaybe();
            result.Should().Be(subject);
        }

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

        [Fact]
        public void ToMaybe_WithNullString_ShouldReturnMaybeNothing()
        {
            string subject = null;
            subject.ToMaybe().Should().Be(Maybe<string>.Nothing);
        }

        [Fact]
        public void ToMaybe_WithEmptyString_ShouldNotReturnMaybeNothing()
        {
            var subject = "";
            var maybe = subject.ToMaybe();
            maybe.Should().NotBe(Maybe<string>.Nothing);
            maybe.HasValue.Should().BeTrue();
            maybe.Value.Should().Be(subject);
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

        [Theory]
        [MemberData(nameof(Select_WithNullablePropertyTestCases))]
        public async Task SelectAsync_WithNullableProperty_ReturnsSelectedProperty(Maybe<NullableIntObj> subject, Maybe<int> expected)
        {
            var result = await subject.SelectAsync(async o => await Task.FromResult(o.Count));

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

        [Fact]
        public void Select_NullArgument_ShouldThrow()
        {
            Action subject = () => 1.ToMaybe().Select<int, int>(null);

            subject.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Select_ConstrainedStructNullArgument_ShouldThrow()
        {
            Action subject = () => 1.ToMaybe().Select((Func<int, int?>)null);

            subject.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(SelectMany_WithMaybePropertyTestCases))]
        public void SelectMany_WithMaybeProperty_ReturnsSelectedProperty(Maybe<MaybeIntObj> subject, Maybe<int> expected)
        {
            var result = subject.SelectMany(o => o.Count);

            result.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(SelectMany_WithMaybePropertyTestCases))]
        public async Task SelectManyAsync_WithMaybeProperty_ReturnsSelectedProperty(Maybe<MaybeIntObj> subject, Maybe<int> expected)
        {
            var result = await subject.SelectManyAsync(async o => await Task.FromResult(o.Count));

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

        [Fact]
        public void SelectMany_NullArgument_ShouldThrow()
        {
            Action subject = () => 1.ToMaybe().SelectMany<int, int>(null);

            subject.Should().ThrowExactly<ArgumentNullException>();
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
        public async Task ConsumeAsync_WithValue_ShouldExecuteAction()
        {
            string result = null;
            const int expected = 1;

            await expected.ToMaybe()
                .ConsumeAsync(async i =>
                {
                    result = i.ToString();
                    await Task.CompletedTask;
                });

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

        [Fact]
        public async Task ConsumeAsync_WithNoValue_ShouldNotExecuteAction()
        {
            var result = "a";

            await Maybe<string>.Nothing
                .ConsumeAsync(async _ =>
                {
                    result = "b";
                    await Task.CompletedTask;
                });

            result.Should().Be("a");
        }

        [Fact]
        public void Consume_NullArgument_ShouldThrow()
        {
            Action subject = () => 1.ToMaybe().Consume((Action<int>)null);

            subject.Should().ThrowExactly<ArgumentNullException>();
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

        [Fact]
        public void Zip_NullArgument_ShouldThrow()
        {
            Action subject = () => 1.ToMaybe().Zip(Maybe<int>.Nothing, (Func<int, int, int>)null);

            subject.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Zip_MaybeNullArgument_ShouldThrow()
        {
            Action subject = () => 1.ToMaybe().Zip(Maybe<int>.Nothing, (Func<int, int, Maybe<int>>)null);

            subject.Should().ThrowExactly<ArgumentNullException>();
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

        [Fact]
        public void ZipAndConsume_NullArgument_ShouldThrow()
        {
            Action subject = () => 1.ToMaybe().ZipAndConsume(Maybe<int>.Nothing, null);

            subject.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
