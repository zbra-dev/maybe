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
}
