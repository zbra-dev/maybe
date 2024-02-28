using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace ZBRA.Maybe.Test
{
    public class MaybeSingleTests
    {
        public class TestData<T>
        {
            public IEnumerable<T> Subject { get; set; }
            public Func<T, bool> Predicate { get; set; }
            public int ExpectedGetEnumeratorCalls { get; set; }
            public int ExpectedCurrentCalls { get; set; }
            public int ExpectedMoveNextCalls { get; set; }
            public Maybe<T> ExpectedResult { get; set; }
        }

        [Fact]
        public void MaybeSingle_WithNullSequence_ShouldReturnMaybeNothing()
        {
            ((IEnumerable<int>)null).MaybeSingle().Should().Be(Maybe<int>.Nothing);
            ((IEnumerable<string>)null).MaybeSingle().Should().Be(Maybe<string>.Nothing);
            ((IEnumerable<int>)null).MaybeSingle(it => true).Should().Be(Maybe<int>.Nothing);
            ((IEnumerable<string>)null).MaybeSingle(it => true).Should().Be(Maybe<string>.Nothing);
        }

        [Theory]
        [MemberData(nameof(MaybeSingle_WithNullPredicate_TestData))]
        public void MaybeSingle_WithNullPredicate_ShouldThrow<T>(TestData<T> testData)
        {
            var (enumerableMock, enumeratorMock) = testData.Subject.GetMocks();

            Func<Maybe<T>> getResult = () => enumerableMock.Object.MaybeSingle(testData.Predicate);

            getResult.Should().ThrowExactly<ArgumentNullException>();

            enumerableMock.Verify(it => it.GetEnumerator(), Times.Exactly(testData.ExpectedGetEnumeratorCalls));
            enumeratorMock.Verify(it => it.Current, Times.Exactly(testData.ExpectedCurrentCalls));
            enumeratorMock.Verify(it => it.MoveNext(), Times.Exactly(testData.ExpectedMoveNextCalls));
        }

        [Theory]
        [MemberData(nameof(MaybeSingle_WithLessThanTwoElements_TestData))]
        public void MaybeSingle_WithLessThanTwoElements_ShouldHaveExpectedBehavior<T>(TestData<T> testData)
        {
            var (enumerableMock, enumeratorMock) = testData.Subject.GetMocks();

            enumerableMock.Object.MaybeSingle().Should().BeEquivalentTo(testData.ExpectedResult);

            enumerableMock.Verify(it => it.GetEnumerator(), Times.Exactly(testData.ExpectedGetEnumeratorCalls));
            enumeratorMock.Verify(it => it.Current, Times.Exactly(testData.ExpectedCurrentCalls));
            enumeratorMock.Verify(it => it.MoveNext(), Times.Exactly(testData.ExpectedMoveNextCalls));
        }

        [Theory]
        [MemberData(nameof(MaybeSingle_WithLessThanTwoFilteredElements_TestData))]
        public void MaybeSingle_WithLessThanTwoFilteredElements_ShouldHaveExpectedBehavior<T>(TestData<T> testData)
        {
            var (enumerableMock, enumeratorMock) = testData.Subject.GetMocks();

            enumerableMock.Object.MaybeSingle(testData.Predicate).Should().BeEquivalentTo(testData.ExpectedResult);

            enumerableMock.Verify(it => it.GetEnumerator(), Times.Exactly(testData.ExpectedGetEnumeratorCalls));
            enumeratorMock.Verify(it => it.Current, Times.Exactly(testData.ExpectedCurrentCalls));
            enumeratorMock.Verify(it => it.MoveNext(), Times.Exactly(testData.ExpectedMoveNextCalls));
        }

        [Theory]
        [MemberData(nameof(MaybeSingle_WithMoreThanOneElement_TestData))]
        public void MaybeSingle_WithMoreThanOneElement_ShouldThrow<T>(TestData<T> testData)
        {
            var (enumerableMock, enumeratorMock) = testData.Subject.GetMocks();

            Func<Maybe<T>> getResult = () => enumerableMock.Object.MaybeSingle();

            getResult.Should().ThrowExactly<InvalidOperationException>().WithMessage("*Sequence contains more than one element");

            enumerableMock.Verify(it => it.GetEnumerator(), Times.Exactly(testData.ExpectedGetEnumeratorCalls));
            enumeratorMock.Verify(it => it.Current, Times.Exactly(testData.ExpectedCurrentCalls));
            enumeratorMock.Verify(it => it.MoveNext(), Times.Exactly(testData.ExpectedMoveNextCalls));
        }

        [Theory]
        [MemberData(nameof(MaybeSingle_WithMoreThanOneFilteredElement_TestData))]
        public void MaybeSingle_WithMoreThanOneFilteredElement_ShouldThrow<T>(TestData<T> testData)
        {
            var (enumerableMock, enumeratorMock) = testData.Subject.GetMocks();

            Func<Maybe<T>> getResult = () => enumerableMock.Object.MaybeSingle(testData.Predicate);

            getResult.Should().ThrowExactly<InvalidOperationException>().WithMessage("*Sequence contains more than one matching element");

            enumerableMock.Verify(it => it.GetEnumerator(), Times.Exactly(testData.ExpectedGetEnumeratorCalls));
            enumeratorMock.Verify(it => it.Current, Times.Exactly(testData.ExpectedCurrentCalls));
            enumeratorMock.Verify(it => it.MoveNext(), Times.Exactly(testData.ExpectedMoveNextCalls));
        }

        #region Test Data
        public static IEnumerable<object[]> MaybeSingle_WithNullPredicate_TestData()
        {
            static object[] CreateNullPredicateTestData<T>(IEnumerable<T> subject)
            {
                return new object[]
                {
                    new TestData<T>
                    {
                        Subject = subject,
                        Predicate = null,
                        ExpectedGetEnumeratorCalls = 0,
                        ExpectedCurrentCalls = 0,
                        ExpectedMoveNextCalls = 0,
                    }
                };
            }

            yield return CreateNullPredicateTestData(new int[] { });
            yield return CreateNullPredicateTestData(new int[] { 1 });
            yield return CreateNullPredicateTestData(new int[] { 1, 2 });
            yield return CreateNullPredicateTestData(new string[] { "1" });
            yield return CreateNullPredicateTestData(new string[] { "1", "2" });
        }

        public static IEnumerable<object[]> MaybeSingle_WithLessThanTwoElements_TestData()
        {
            yield return new object[]
            {
                new TestData<int>
                {
                    Subject = new int[] { },
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 0,
                    ExpectedMoveNextCalls = 1,
                    ExpectedResult = Maybe<int>.Nothing,
                }
            };
            yield return new object[]
            {
                new TestData<int>
                {
                    Subject = new int[] { 1 },
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 1,
                    ExpectedMoveNextCalls = 2,
                    ExpectedResult = 1.ToMaybe(),
                }
            };
            yield return new object[]
            {
                new TestData<string>
                {
                    Subject = new string[] { },
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 0,
                    ExpectedMoveNextCalls = 1,
                    ExpectedResult = Maybe<string>.Nothing,
                }
            };
            yield return new object[]
            {
                new TestData<string>
                {
                    Subject = new string[] { "1" },
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 1,
                    ExpectedMoveNextCalls = 2,
                    ExpectedResult = "1".ToMaybe(),
                }
            };
        }

        public static IEnumerable<object[]> MaybeSingle_WithLessThanTwoFilteredElements_TestData()
        {
            yield return new object[]
            {
                new TestData<int>
                {
                    Subject = new int[] { },
                    Predicate = i => i > 1,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 0,
                    ExpectedMoveNextCalls = 1,
                    ExpectedResult = Maybe<int>.Nothing,
                }
            };
            yield return new object[]
            {
                new TestData<int>
                {
                    Subject = new int[] { 1 },
                    Predicate = i => i > 1,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 1,
                    ExpectedMoveNextCalls = 2,
                    ExpectedResult = Maybe<int>.Nothing,
                }
            };
            yield return new object[]
            {
                new TestData<int>
                {
                    Subject = new int[] { 1, 2, 3, 4, 5 },
                    Predicate = i => i == 3,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 5,
                    ExpectedMoveNextCalls = 6,
                    ExpectedResult = 3.ToMaybe(),
                }
            };
            yield return new object[]
            {
                new TestData<string>
                {
                    Subject = new string[] { },
                    Predicate = i => i.Length > 1,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 0,
                    ExpectedMoveNextCalls = 1,
                    ExpectedResult = Maybe<string>.Nothing,
                }
            };
            yield return new object[]
            {
                new TestData<string>
                {
                    Subject = new string[] { "1" },
                    Predicate = i => int.Parse(i) > 1,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 1,
                    ExpectedMoveNextCalls = 2,
                    ExpectedResult = Maybe<string>.Nothing,
                }
            };
            yield return new object[]
            {
                new TestData<string>
                {
                    Subject = new string[] { "1", "2", "3", "4", "5" },
                    Predicate = i => int.Parse(i) == 3,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 5,
                    ExpectedMoveNextCalls = 6,
                    ExpectedResult = "3".ToMaybe(),
                }
            };
        }

        public static IEnumerable<object[]> MaybeSingle_WithMoreThanOneElement_TestData()
        {
            yield return new object[]
            {
                new TestData<int>
                {
                    Subject = new int[] { 1, 2 },
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 1,
                    ExpectedMoveNextCalls = 2,
                }
            };
            yield return new object[]
            {
                new TestData<string>
                {
                    Subject = new string[] { "1", "2" },
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 1,
                    ExpectedMoveNextCalls = 2,
                }
            };
        }

        public static IEnumerable<object[]> MaybeSingle_WithMoreThanOneFilteredElement_TestData()
        {
            yield return new object[]
            {
                new TestData<int>
                {
                    Subject = new int[] { 1, 2, 3, 4, 5, 6, 7 },
                    Predicate = i => i > 2,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 4,
                    ExpectedMoveNextCalls = 4,
                }
            };
            yield return new object[]
            {
                new TestData<string>
                {
                    Subject = new string[] { "1", "2", "3", "4", "5", "6", "7" },
                    Predicate = i => int.Parse(i) > 2,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 4,
                    ExpectedMoveNextCalls = 4,
                }
            };
        }
        #endregion
    }
}
