using Moq;
using System;
using System.Collections.Generic;

namespace Maybe.Test
{
    public static class Extensions
    {
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
    }
}
