using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Maybe.Test
{
    public class MaybeSingleNullableStructsTests
    {
        public class NullableStructsTestData<T> where T : struct
        {
            public IEnumerable<T?> Subject { get; set; }
            public Func<T?, bool> Predicate { get; set; }
            public int ExpectedGetEnumeratorCalls { get; set; }
            public int ExpectedCurrentCalls { get; set; }
            public int ExpectedMoveNextCalls { get; set; }
            public Maybe<T> ExpectedResult { get; set; }
            public bool ShouldThrow { get; set; }
        }

        [Theory]
        [MemberData(nameof(MaybeSingle_WithNullableStructElements_TestData))]
        public void MaybeSingle_NullPredicate_ShouldThrow<T>(NullableStructsTestData<T> testData)
            where T : struct
        {
            var (enumerableMock, enumeratorMock) = CreateMocks(testData.Subject);

            if (testData.Predicate == null)
            {
                Func<Maybe<T>> getResult = () => enumerableMock.Object.MaybeSingle(testData.Predicate);

                getResult.Should().Throw<ArgumentNullException>();
            }

            enumerableMock.Verify(it => it.GetEnumerator(), Times.Never);
            enumeratorMock.Verify(it => it.Current, Times.Never);
            enumeratorMock.Verify(it => it.MoveNext(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(MaybeSingle_WithNullableStructElements_TestData))]
        public void MaybeSingle_WithNullableStructElements_ShouldHaveExpectedBehavior<T>(
            NullableStructsTestData<T> testData) 
            where T : struct
        {
            var (enumerableMock, enumeratorMock) = CreateMocks(testData.Subject);

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

        private static (Mock<IEnumerable<T?>>, Mock<IEnumerator<T?>>) CreateMocks<T>(
            IEnumerable<T?> subject) where T : struct
        {
            var enumerableMock = new Mock<IEnumerable<T?>>();
            var enumeratorMock = new Mock<IEnumerator<T?>>();

            enumerableMock.Setup(m => m.GetEnumerator()).Returns(() =>
            {
                var subjectEnumerator = subject.GetEnumerator();

                enumeratorMock.Setup(m => m.Current).Returns(() => subjectEnumerator.Current);
                enumeratorMock.Setup(m => m.MoveNext()).Returns(() => subjectEnumerator.MoveNext());

                return enumeratorMock.Object;
            });

            return (enumerableMock, enumeratorMock);
        }

        public static IEnumerable<object[]> MaybeSingle_WithNullableStructElements_TestData()
        {
            yield return new object[]
            {
                new NullableStructsTestData<int> {
                    Subject = new int?[] { null },
                    Predicate = i => i > 1,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 1,
                    ExpectedMoveNextCalls = 2,
                    ExpectedResult = Maybe<int>.Nothing
                }
            };
            yield return new object[]
            {
                new NullableStructsTestData<int> {
                    Subject = new int?[] { },
                    Predicate = i => i > 1,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 0,
                    ExpectedMoveNextCalls = 1,
                    ExpectedResult = Maybe<int>.Nothing
                }
            };
            yield return new object[]
            {
                new NullableStructsTestData<int> {
                    Subject = new int?[] { 1 },
                    Predicate = i => i > 1,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 1,
                    ExpectedMoveNextCalls = 2,
                    ExpectedResult = Maybe<int>.Nothing
                }
            };
            yield return new object[]
            {
                new NullableStructsTestData<int> {
                    Subject = new int?[] { 1, 2, 3, 4, 5 },
                    Predicate = i => i == 3,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 5,
                    ExpectedMoveNextCalls = 6,
                    ExpectedResult = 3.ToMaybe()
                }
            };
            yield return new object[]
            {
                new NullableStructsTestData<int> {
                    Subject = new int?[] { },
                    Predicate = null,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 0,
                    ExpectedMoveNextCalls = 1,
                    ExpectedResult = Maybe<int>.Nothing,
                }
            };
            yield return new object[]
            {
                new NullableStructsTestData<int> {
                    Subject = new int?[] { 1 },
                    Predicate = null,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 1,
                    ExpectedMoveNextCalls = 2,
                    ExpectedResult = 1.ToMaybe()
                }
            };
            yield return new object[]
            {
                new NullableStructsTestData<int> {
                    Subject = new int?[] { 1, 2 },
                    Predicate = null,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 1,
                    ExpectedMoveNextCalls = 2,
                    ShouldThrow = true
                }
            };
            yield return new object[]
            {
                new NullableStructsTestData<int> {
                    Subject = new int?[] { 1, 2, 3, 4, 5, 6, 7 },
                    Predicate = i => i > 2,
                    ExpectedGetEnumeratorCalls = 1,
                    ExpectedCurrentCalls = 4,
                    ExpectedMoveNextCalls = 4,
                    ShouldThrow = true
                }
            };
        }

        [Fact]
        public void MaybeSingle_NullArgument_ShouldReturnMaybeNothing()
        {
            ((IEnumerable<int?>)null).MaybeSingle().Should().Be(Maybe<int>.Nothing);
        }
    }
}
