using FluentAssertions;
using FluentAssertions.Primitives;
using Moq;
using System;
using System.Collections.Generic;

namespace ZBRA.Maybe.Test
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            action = action ?? throw new ArgumentNullException(nameof(action));

            foreach (var item in source)
            {
                action(item);
            }
        }

        public static (Mock<IEnumerable<T?>>, Mock<IEnumerator<T?>>) GetMocks<T>(this IEnumerable<T?> source)
            where T : struct
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            var enumerableMock = new Mock<IEnumerable<T?>>();
            var enumeratorMock = new Mock<IEnumerator<T?>>();

            enumerableMock.Setup(m => m.GetEnumerator()).Returns(() =>
            {
                var sourceEnumerator = source.GetEnumerator();

                enumeratorMock.Setup(m => m.Current).Returns(() => sourceEnumerator.Current);
                enumeratorMock.Setup(m => m.MoveNext()).Returns(() => sourceEnumerator.MoveNext());

                return enumeratorMock.Object;
            });

            return (enumerableMock, enumeratorMock);
        }

        public static (Mock<IEnumerable<T>>, Mock<IEnumerator<T>>) GetMocks<T>(this IEnumerable<T> source)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            var enumerableMock = new Mock<IEnumerable<T>>();
            var enumeratorMock = new Mock<IEnumerator<T>>();

            enumerableMock.Setup(m => m.GetEnumerator()).Returns(() =>
            {
                var sourceEnumerator = source.GetEnumerator();

                enumeratorMock.Setup(m => m.Current).Returns(() => sourceEnumerator.Current);
                enumeratorMock.Setup(m => m.MoveNext()).Returns(() => sourceEnumerator.MoveNext());

                return enumeratorMock.Object;
            });

            return (enumerableMock, enumeratorMock);
        }

        public static AndConstraint<BooleanAssertions> NotBe(this BooleanAssertions booleanAssertions, bool expectedResult)
        {
            booleanAssertions = booleanAssertions ?? throw new ArgumentNullException(nameof(booleanAssertions));

            return booleanAssertions.Be(!expectedResult);
        }
    }
}
