namespace Maybe.Test
{
    public class IntObj
    {
        public int Count { get; set; }

        public IntObj(int count)
        {
            Count = count;
        }
    }

    public class StringObj
    {
        public string Name { get; set; }

        public StringObj(string name)
        {
            Name = name;
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
    public static class Numbers
    {
        public static object[] Constants { get; } = new object[]
        {
            int.MinValue,
            int.MaxValue,

            float.Epsilon,
            float.MinValue,
            float.MaxValue,
            float.PositiveInfinity,
            float.NegativeInfinity,
            float.NaN,

            double.Epsilon,
            double.MinValue,
            double.MaxValue,
            double.PositiveInfinity,
            double.NegativeInfinity,
            double.NaN,

            // https://github.com/xunit/xunit/issues/1771
            //decimal.MinValue,
            //decimal.MaxValue,
            //decimal.MinusOne,
            //decimal.Zero,
            //decimal.One,
        };

        public static (object A, object B)[] ConstantPairs { get; } = new (object A, object B)[]
        {
            (int.MinValue, int.MinValue),
            (int.MinValue, int.MaxValue),
            (int.MaxValue, int.MinValue),
            (int.MaxValue, int.MaxValue),

            (float.Epsilon, float.Epsilon),
            (float.Epsilon, float.MinValue),
            (float.Epsilon, float.MaxValue),
            (float.Epsilon, float.PositiveInfinity),
            (float.Epsilon, float.NegativeInfinity),
            (float.Epsilon, float.NaN),
            (float.MinValue, float.Epsilon),
            (float.MinValue, float.MinValue),
            (float.MinValue, float.MaxValue),
            (float.MinValue, float.PositiveInfinity),
            (float.MinValue, float.NegativeInfinity),
            (float.MinValue, float.NaN),
            (float.MaxValue, float.Epsilon),
            (float.MaxValue, float.MinValue),
            (float.MaxValue, float.MaxValue),
            (float.MaxValue, float.PositiveInfinity),
            (float.MaxValue, float.NegativeInfinity),
            (float.MaxValue, float.NaN),
            (float.PositiveInfinity, float.Epsilon),
            (float.PositiveInfinity, float.MinValue),
            (float.PositiveInfinity, float.MaxValue),
            (float.PositiveInfinity, float.PositiveInfinity),
            (float.PositiveInfinity, float.NegativeInfinity),
            (float.PositiveInfinity, float.NaN),
            (float.NegativeInfinity, float.Epsilon),
            (float.NegativeInfinity, float.MinValue),
            (float.NegativeInfinity, float.MaxValue),
            (float.NegativeInfinity, float.PositiveInfinity),
            (float.NegativeInfinity, float.NegativeInfinity),
            (float.NegativeInfinity, float.NaN),
            (float.NaN, float.Epsilon),
            (float.NaN, float.MinValue),
            (float.NaN, float.MaxValue),
            (float.NaN, float.PositiveInfinity),
            (float.NaN, float.NegativeInfinity),
            (float.NaN, float.NaN),

            (double.Epsilon, double.Epsilon),
            (double.Epsilon, double.MinValue),
            (double.Epsilon, double.MaxValue),
            (double.Epsilon, double.PositiveInfinity),
            (double.Epsilon, double.NegativeInfinity),
            (double.Epsilon, double.NaN),
            (double.MinValue, double.Epsilon),
            (double.MinValue, double.MinValue),
            (double.MinValue, double.MaxValue),
            (double.MinValue, double.PositiveInfinity),
            (double.MinValue, double.NegativeInfinity),
            (double.MinValue, double.NaN),
            (double.MaxValue, double.Epsilon),
            (double.MaxValue, double.MinValue),
            (double.MaxValue, double.MaxValue),
            (double.MaxValue, double.PositiveInfinity),
            (double.MaxValue, double.NegativeInfinity),
            (double.MaxValue, double.NaN),
            (double.PositiveInfinity, double.Epsilon),
            (double.PositiveInfinity, double.MinValue),
            (double.PositiveInfinity, double.MaxValue),
            (double.PositiveInfinity, double.PositiveInfinity),
            (double.PositiveInfinity, double.NegativeInfinity),
            (double.PositiveInfinity, double.NaN),
            (double.NegativeInfinity, double.Epsilon),
            (double.NegativeInfinity, double.MinValue),
            (double.NegativeInfinity, double.MaxValue),
            (double.NegativeInfinity, double.PositiveInfinity),
            (double.NegativeInfinity, double.NegativeInfinity),
            (double.NegativeInfinity, double.NaN),
            (double.NaN, double.Epsilon),
            (double.NaN, double.MinValue),
            (double.NaN, double.MaxValue),
            (double.NaN, double.PositiveInfinity),
            (double.NaN, double.NegativeInfinity),
            (double.NaN, double.NaN),

            // https://github.com/xunit/xunit/issues/1771
            //(decimal.MinValue, decimal.MinValue),
            //(decimal.MinValue, decimal.MaxValue),
            //(decimal.MinValue, decimal.MinusOne),
            //(decimal.MinValue, decimal.Zero),
            //(decimal.MinValue, decimal.One),
            //(decimal.MaxValue, decimal.MinValue),
            //(decimal.MaxValue, decimal.MaxValue),
            //(decimal.MaxValue, decimal.MinusOne),
            //(decimal.MaxValue, decimal.Zero),
            //(decimal.MaxValue, decimal.One),
            //(decimal.MinusOne, decimal.MinValue),
            //(decimal.MinusOne, decimal.MaxValue),
            //(decimal.MinusOne, decimal.MinusOne),
            //(decimal.MinusOne, decimal.Zero),
            //(decimal.MinusOne, decimal.One),
            //(decimal.Zero, decimal.MinValue),
            //(decimal.Zero, decimal.MaxValue),
            //(decimal.Zero, decimal.MinusOne),
            //(decimal.Zero, decimal.Zero),
            //(decimal.Zero, decimal.One),
            //(decimal.One, decimal.MinValue),
            //(decimal.One, decimal.MaxValue),
            //(decimal.One, decimal.MinusOne),
            //(decimal.One, decimal.Zero),
            //(decimal.One, decimal.One),
        };
    }
}
