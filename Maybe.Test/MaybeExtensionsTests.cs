using FluentAssertions;
using Xunit;

namespace Maybe.Test
{
    public class MaybeExtensionsTests
    {
        [Theory]
        [MemberData(nameof(Select_WithNonNullablePropertyTestCases))]
        public void Select_WithNonNullableProperty_ReturnsSelectedProperty(Maybe<IntObj> subject, Maybe<int> expected)
        {
            var result = subject.Select(i => i.Count);

            result.Should().Be(expected);
        }

        public static TheoryData<Maybe<IntObj>, Maybe<int>> Select_WithNonNullablePropertyTestCases()
        {
            var data = new TheoryData<Maybe<IntObj>, Maybe<int>>
            {

            };

            data.Add(Maybe<IntObj>.Nothing, Maybe<int>.Nothing);
            data.Add(new IntObj(1).ToMaybe(), 1.ToMaybe());

            return data;
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
            var data = new TheoryData<Maybe<NullableIntObj>, Maybe<int>>
            {
                
            };

            data.Add(Maybe<NullableIntObj>.Nothing, Maybe<int>.Nothing);
            data.Add(new NullableIntObj(null).ToMaybe(), Maybe<int>.Nothing);
            data.Add(new NullableIntObj(1).ToMaybe(), 1.ToMaybe());

            return data;
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
            var data = new TheoryData<Maybe<MaybeIntObj>, Maybe<int>>
            {

            };

            data.Add(Maybe<MaybeIntObj>.Nothing, Maybe<int>.Nothing);
            data.Add(new MaybeIntObj(null).ToMaybe(), Maybe<int>.Nothing);
            data.Add(new MaybeIntObj(1).ToMaybe(), 1.ToMaybe());

            return data;
        }

        public class IntObj
        {
            public int Count { get; set; }

            public IntObj(int count)
            {
                Count = count;
            }
        }

        public class NullableIntObj
        {
            public int? Count { get; set; }

            public NullableIntObj(int? count)
            {
                Count = count;
            }
        }

        public class MaybeIntObj
        {
            public Maybe<int> Count { get; set; }

            public MaybeIntObj(int? count)
            {
                Count = count.ToMaybe();
            }
        }
    }
}
