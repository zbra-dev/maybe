using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Maybe.Test
{
    public class MaybeEnumerableExtensionsTests
    {
        [Theory]
        [InlineData(null, 0, null)]
        [InlineData(new int[] { }, 0, null)]
        [InlineData(new int[] { 1, 2 }, -1, null)]
        [InlineData(new int[] { 1, 2 }, 2, null)]
        [InlineData(new int[] { 1, 2 }, 0, 1)]
        [InlineData(new int[] { 1, 2 }, 1, 2)]
        public void MaybeGet_WithPosition_ReturnsItemAtPosition(IList<int> subject, int position, int? expected)
        {
            var result = subject
                .MaybeGet(position);

            result.Should().Be(expected.ToMaybe());
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(new int[] { }, null)]
        [InlineData(new int[] { 1 }, 1)]
        public void MaybeSingle_WithoutPredicate_ReturnsSingleItem(IList<int> subject, int? expected)
        {
            var result = subject
                .MaybeSingle();

            result.Should().Be(expected.ToMaybe());
        }

        [Fact]
        public void MaybeSingle_WithMultipleElements_ThrowsException()
        {
            var subject = new[] { 1, 2 };
            subject.Invoking(s => s.MaybeSingle())
                .Should()
                .ThrowExactly<InvalidOperationException>();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(new int[] { }, null)]
        [InlineData(new int[] { 1 }, 1)]
        [InlineData(new int[] { 1, 12 }, 1)]
        public void MaybeSingle_WithPredicate_ReturnsSingleMatchingItem(IList<int> subject, int? expected)
        {
            static bool predicate(int i) => i < 10;
            var result = subject
                .MaybeSingle(predicate);

            result.Should().Be(expected.ToMaybe());
        }

        [Fact]
        public void MaybeSingle_WithPredicateAndMultipleMatchingElements_ThrowsException()
        {
            static bool predicate(int i) => i < 10;
            var subject = new[] { 1, 20, 2 };
            subject.Invoking(s => s.MaybeSingle(predicate))
                .Should()
                .ThrowExactly<InvalidOperationException>();
        }

        [Theory]
        [MemberData(nameof(MaybeSingle_WithNullableItems_ReturnsSingleItemTestCases))]
        public void MaybeSingle_WithNullableItems_ReturnsSingleItem(IList<int?> subject, int? expected)
        {
            var result = subject
                .MaybeSingle();

            result.Should().Be(expected.ToMaybe());
        }

        public static TheoryData<IList<int?>, int?> MaybeSingle_WithNullableItems_ReturnsSingleItemTestCases()
        {
            return new TheoryData<IList<int?>, int?>
            {
                { null, null },
                { new int?[] { }, null },
                { new int?[] { null }, null },
                { new int?[] { 1 }, 1 },
            };
        }

        [Fact]
        public void MaybeSingle_WithMultipleNullableElements_ThrowsException()
        {
            var subject = new int?[] { 1, 2 };
            subject.Invoking(s => s.MaybeSingle())
                .Should()
                .ThrowExactly<InvalidOperationException>();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(new int[] { }, null)]
        [InlineData(new int[] { 1 }, 1)]
        [InlineData(new int[] { 2, 1 }, 2)]
        public void MaybeFirst_WithoutPredicate_ReturnsFirstItem(IList<int> subject, int? expected)
        {
            var result = subject
                .MaybeFirst();

            result.Should().Be(expected.ToMaybe());
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(new int[] { }, null)]
        [InlineData(new int[] { 1 }, null)]
        [InlineData(new int[] { 1, 15 }, 15)]
        [InlineData(new int[] { 1, 15, 1, 20 }, 15)]
        public void MaybeFirst_WithPredicate_ReturnsFirstMatchingItem(IList<int> subject, int? expected)
        {
            static bool predicate(int i) => i > 10;
            var result = subject
                .MaybeFirst(predicate);

            result.Should().Be(expected.ToMaybe());
        }

        [Theory]
        [MemberData(nameof(MaybeFirst_WithNullableItems_ReturnsFirstItemTestCases))]
        public void MaybeFirst_WithNullableItems_ReturnsFirstItem(IList<int?> subject, int? expected)
        {
            var result = subject
                .MaybeFirst();

            result.Should().Be(expected.ToMaybe());
        }

        public static TheoryData<IList<int?>, int?> MaybeFirst_WithNullableItems_ReturnsFirstItemTestCases()
        {
            return new TheoryData<IList<int?>, int?>
            {
                { null, null },
                { new int?[] { }, null },
                { new int?[] { null }, null },
                { new int?[] { 1 }, 1 },
                { new int?[] { 1, 2 }, 1 },
            };
        }

        [Theory]
        [InlineData(0, "0")]
        [InlineData(1, "1")]
        [InlineData(2, null)]
        public void MaybeGet_WithDictionary_ReturnsValueAssociatedWithKey(int key, string expected)
        {
            var dictionary = new Dictionary<int, string>
            {
                { 0, "0" },
                { 1, "1" },
            };

            var result = dictionary.MaybeGet(key);
            result.Should().Be(expected.ToMaybe());
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(0, "0")]
        [InlineData(1, "1")]
        [InlineData(2, null)]
        public void MaybeGet_WithDictionaryAndMaybeKey_ReturnsValueAssociatedWithKey(int? key, string expected)
        {
            var dictionary = new Dictionary<int, string>
            {
                { 0, "0" },
                { 1, "1" },
            };

            var result = dictionary.MaybeGet(key.ToMaybe());
            result.Should().Be(expected.ToMaybe());
        }

        [Theory]
        [InlineData(-1, null)]
        [InlineData(0, "0")]
        [InlineData(1, "1")]
        [InlineData(2, null)]
        public void MaybeGet_WithDictionaryAndMaybeValue_ReturnsValueAssociatedWithKey(int key, string expected)
        {
            var dictionary = new Dictionary<int, Maybe<string>>
            {
                { -1, Maybe<string>.Nothing },
                { 0, "0".ToMaybe() },
                { 1, "1".ToMaybe() },
            };

            var result = dictionary.MaybeGet(key);
            result.Should().Be(expected.ToMaybe());
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(-1, null)]
        [InlineData(0, "0")]
        [InlineData(1, "1")]
        [InlineData(2, null)]
        public void MaybeGet_WithDictionaryAndMaybeValueAndMaybeKey_ReturnsValueAssociatedWithKey(int? key, string expected)
        {
            var dictionary = new Dictionary<int, Maybe<string>>
            {
                { -1, Maybe<string>.Nothing },
                { 0, "0".ToMaybe() },
                { 1, "1".ToMaybe() },
            };

            var result = dictionary.MaybeGet(key.ToMaybe());
            result.Should().Be(expected.ToMaybe());
        }

        [Theory]
        [MemberData(nameof(Compact_WithEnumerableOfMaybe_ReturnsValuesTestCases))]
        public void Compact_WithEnumerableOfMaybe_ReturnsValues(IList<Maybe<string>> subject, IList<string> expected)
        {
            var result = subject.Compact();
            result.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        public static TheoryData<IList<Maybe<string>>, IList<string>> Compact_WithEnumerableOfMaybe_ReturnsValuesTestCases()
        {
            return new TheoryData<IList<Maybe<string>>, IList<string>>
            {
                { new Maybe<string>[] { }, new string[] { } },
                { new Maybe<string>[] { Maybe<string>.Nothing }, new string[] { } },
                {
                    new Maybe<string>[]
                    {
                        Maybe<string>.Nothing,
                        "a".ToMaybe(),
                        Maybe<string>.Nothing,
                        "b".ToMaybe(),
                        Maybe<string>.Nothing,
                        "c".ToMaybe(),
                        Maybe<string>.Nothing
                    },
                    new string[] { "a", "b", "c" }
                },
                {
                    new Maybe<string>[]
                    {
                        "a".ToMaybe(),
                        "b".ToMaybe(),
                        "c".ToMaybe(),
                    },
                    new string[] { "a", "b", "c" }
                },
            };
        }

        [Theory]
        [MemberData(nameof(Compact_WithEnumerableOfNullables_ReturnsValuesTestCases))]
        public void Compact_WithEnumerableOfNullables_ReturnsValues(IList<int?> subject, IList<int> expected)
        {
            var result = subject.Compact();
            result.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        public static TheoryData<IList<int?>, IList<int>> Compact_WithEnumerableOfNullables_ReturnsValuesTestCases()
        {
            return new TheoryData<IList<int?>, IList<int>>
            {
                { new int?[] { }, new int[] { } },
                { new int?[] { null }, new int[] { } },
                { new int?[] { null, 1, null, 2, null, 3, null }, new int[] { 1, 2, 3 } },
                { new int?[] { 1, 2, 3 }, new int[] { 1, 2, 3 } },
            };
        }
    }
}
