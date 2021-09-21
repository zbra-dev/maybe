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
                var subjectEnumerator = source.GetEnumerator();

                enumeratorMock.Setup(m => m.Current).Returns(() => subjectEnumerator.Current);
                enumeratorMock.Setup(m => m.MoveNext()).Returns(() => subjectEnumerator.MoveNext());

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
                var subjectEnumerator = source.GetEnumerator();

                enumeratorMock.Setup(m => m.Current).Returns(() => subjectEnumerator.Current);
                enumeratorMock.Setup(m => m.MoveNext()).Returns(() => subjectEnumerator.MoveNext());

                return enumeratorMock.Object;
            });

            return (enumerableMock, enumeratorMock);
        }
    }
}
