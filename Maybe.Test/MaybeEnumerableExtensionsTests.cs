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

        // Should this really throw an exception?
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
            var data = new TheoryData<IList<int?>, int?>
            {

            };

            data.Add(null, null);
            data.Add(new int?[] { }, null);
            data.Add(new int?[] { null }, null);
            data.Add(new int?[] { 1 }, 1);

            return data;
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
            var data = new TheoryData<IList<int?>, int?>
            {
                
            };

            data.Add(null, null);
            data.Add(new int?[] { }, null);
            data.Add(new int?[] { null }, null);
            data.Add(new int?[] { 1 }, 1);
            data.Add(new int?[] { 1, 2}, 1);

            return data;
        }
    }
}
