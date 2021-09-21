using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Maybe.Test
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
            public bool ShouldThrow { get; set; }
        }

        [Theory]
        [MemberData(nameof(MaybeSingle_WithStructOrClassElements_TestData))]
        public void MaybeSingle_NullPredicate_ShouldThrow<T>(TestData<T> testData)
        {
            var (enumerableMock, enumeratorMock) = testData.Subject.GetMocks();

            if (testData.Predicate == null)
            {
                Func<Maybe<T>> getResult = () => enumerableMock.Object.MaybeFirst(testData.Predicate);

                getResult.Should().Throw<ArgumentNullException>();
            }

            enumerableMock.Verify(it => it.GetEnumerator(), Times.Never);
            enumeratorMock.Verify(it => it.Current, Times.Never);
            enumeratorMock.Verify(it => it.MoveNext(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(MaybeSingle_WithStructOrClassElements_TestData))]
        public void MaybeSingle_WithStructOrClassElements_ShouldHaveExpectedBehavior<T>(
            TestData<T> testData)
        {
            var (enumerableMock, enumeratorMock) = testData.Subject.GetMocks();

            Func<Maybe<T>> getResult = () => testData.Predicate == null
                ? enumerableMock.Object.MaybeSingle()
                : enumerableMock.Object.MaybeSingle(testData.Predicate);

            if (testData.ShouldThrow)
            {
                getResult.Should().Throw<InvalidOperationException>().WithMessage("Sequence contains more than one element");
            }
            else
            {
                getResult().Should().BeEquivalentTo(testData.ExpectedResult);
            }

            enumerableMock.Verify(it => it.GetEnumerator(), Times.Exactly(testData.ExpectedGetEnumeratorCalls));
            enumeratorMock.Verify(it => it.Current, Times.Exactly(testData.ExpectedCurrentCalls));
            enumeratorMock.Verify(it => it.MoveNext(), Times.Exactly(testData.ExpectedMoveNextCalls));
        }

        public static IEnumerable<object[]> MaybeSingle_WithStructOrClassElements_TestData()
        {
            yield return new object[]
            {
                new TestData<int> {
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
                new TestData<int> {
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
                new TestData<int> {
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
                new TestData<int> {
                    Subject = new int[] { },
                    Predicate = null,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 0,
                    ExpectedMoveNextCalls = 1,
                    ExpectedResult = Maybe<int>.Nothing,
                }
            };
            yield return new object[]
            {
                new TestData<int> {
                    Subject = new int[] { 1 },
                    Predicate = null,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 1,
                    ExpectedMoveNextCalls = 2,
                    ExpectedResult = 1.ToMaybe(),
                }
            };
            yield return new object[]
            {
                new TestData<int> {
                    Subject = new int[] { 1, 2 },
                    Predicate = null,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 1,
                    ExpectedMoveNextCalls = 2,
                    ShouldThrow = true,
                }
            };
            yield return new object[]
            {
                new TestData<int> {
                    Subject = new int[] { 1, 2, 3, 4, 5, 6, 7 },
                    Predicate = i => i > 2,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 4,
                    ExpectedMoveNextCalls = 4,
                    ShouldThrow = true,
                }
            };

            yield return new object[]
            {
                new TestData<string> {
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
                new TestData<string> {
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
                new TestData<string> {
                    Subject = new string[] { "1", "2", "3", "4", "5" },
                    Predicate = i => int.Parse(i) == 3,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 5,
                    ExpectedMoveNextCalls = 6,
                    ExpectedResult = "3".ToMaybe(),
                }
            };
            yield return new object[]
            {
                new TestData<string> {
                    Subject = new string[] { },
                    Predicate = null,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 0,
                    ExpectedMoveNextCalls = 1,
                    ExpectedResult = Maybe<string>.Nothing,
                }
            };
            yield return new object[]
            {
                new TestData<string> {
                    Subject = new string[] { "1" },
                    Predicate = null,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 1,
                    ExpectedMoveNextCalls = 2,
                    ExpectedResult = "1".ToMaybe(),
                }
            };
            yield return new object[]
            {
                new TestData<string> {
                    Subject = new string[] { "1", "2" },
                    Predicate = null,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 1,
                    ExpectedMoveNextCalls = 2,
                    ShouldThrow = true,
                }
            };
            yield return new object[]
            {
                new TestData<string> {
                    Subject = new string[] { "1", "2", "3", "4", "5", "6", "7" },
                    Predicate = i => int.Parse(i) > 2,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 4,
                    ExpectedMoveNextCalls = 4,
                    ShouldThrow = true,
                }
            };
        }

        [Fact]
        public void MaybeSingle_NullArgument_ShouldReturnMaybeNothing()
        {
            ((IEnumerable<int>)null).MaybeSingle().Should().Be(Maybe<int>.Nothing);
            ((IEnumerable<string>)null).MaybeSingle().Should().Be(Maybe<string>.Nothing);
        }
    }
}
