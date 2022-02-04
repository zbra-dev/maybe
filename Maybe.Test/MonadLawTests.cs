using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace ZBRA.Maybe.Test
{
    // https://mikhail.io/2018/07/monads-explained-in-csharp-again/
    // In our case SelectMany = Bind of the article
    public class MonadLawTests
    {
        [Theory]
        [MemberData(nameof(NonRelatedData))]
        public void Maybe_ShouldSatisfyLeftIdentityLaw<T>(T value)
        {
            static Maybe<string> F(T it) => it == null ? Maybe<string>.Nothing : it.ToString().ToMaybe();

            value.ToMaybe().SelectMany(F).Should().Be(F(value));
        }

        [Theory]
        [MemberData(nameof(NonRelatedData))]
        public void Maybe_ShouldSatisfyRightIdentityLaw<T>(T value)
        {
            var monadicValue = value.ToMaybe();

            monadicValue.SelectMany(it => it.ToMaybe()).Should().Be(monadicValue);
        }

        [Theory]
        [MemberData(nameof(NonRelatedData))]
        public void Maybe_ShouldSatisfyAssociativityLaw<T>(T value)
        {
            var monodicValue = value.ToMaybe();

            static Maybe<string> F(T it) => it.ToString().ToMaybe();
            static Maybe<int> G(string it) => it.GetHashCode().ToMaybe();

            monodicValue.SelectMany(F).SelectMany(G).Should().Be(monodicValue.SelectMany(it => F(it).SelectMany(G)));
        }

        public static TheoryData<object> NonRelatedData()
        {
            return new TheoryData<object>()
            {
                '1',
                1,
                1.0,
                DateTime.Now,
                new List<string>(),
                1.ToMaybe(),
                (1, 2),
                "1",
                null,
            };
        }
    }
}
