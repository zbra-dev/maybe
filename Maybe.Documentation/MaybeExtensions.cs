using ZBRA.Maybe;

namespace Maybe.Documentation
{
    public class MaybeExtensions
    {
        public static void OrEmptyExample_WithValue()
        {
            Console.WriteLine("OrEmpty example with values");
            var maybe = "some value".ToMaybe();
            var v = maybe.OrEmpty();
            Console.WriteLine($"Print Value: {v}");
            // Print Value: some value
        }

        public static void OrEmptyExample_WithoutValue()
        {
            Console.WriteLine("OrEmpty example without values");
            var maybe = Maybe<string>.Nothing;
            var v = maybe.OrEmpty();
            Console.WriteLine($"Print Value: {v}");
            // Print Value: 
        }

        public static void OrNullExample_WithValue()
        {
            Console.WriteLine("OrNull example with values");
            var maybe = "some value".ToMaybe();
            var v = maybe.OrNull();
            Console.WriteLine($"Print Value: {v}");
            // Print Value: some value
        }

        public static void OrNullExample_WithoutValue()
        {
            Console.WriteLine("OrNull example without values");
            var maybe = Maybe<string>.Nothing;
            var v = maybe.OrNull();
            Console.WriteLine($"Print Value: {v}");
            // Print Value: 
        }


        public static void OrTrueExample_WithValue()
        {
            Console.WriteLine("OrTrue example with values");
            var maybe = false.ToMaybe();
            var v = maybe.OrTrue();
            Console.WriteLine($"Print Value: {v}");
            // Print Value: false
        }

        public static void OrTrueExample_WithoutValue()
        {
            Console.WriteLine("OrTrue example without values");
            var maybe = Maybe<bool>.Nothing;
            var v = maybe.OrTrue();
            Console.WriteLine($"Print Value: {v}");
            // Print Value: true
        }

        public static void OrFalseExample_WithValue()
        {
            Console.WriteLine("OrFalse example with values");
            var maybe = false.ToMaybe();
            var v = maybe.OrFalse();
            Console.WriteLine($"Print Value: {v}");
            // Print Value: False
        }

        public static void OrFalseExample_WithoutValue()
        {
            Console.WriteLine("OrFalse example without values");
            var maybe = Maybe<bool>.Nothing;
            var v = maybe.OrFalse();
            Console.WriteLine($"Print Value: {v}");
            // Print Value: False   
        }

        internal static void ToNullableExample_WithValue()
        {
            Console.WriteLine("ToNullable example with value");
            var maybe = 1234.ToMaybe();
            var v = maybe.ToNullable();
            Console.WriteLine($"Print Value: {v}");
            // Print Value: 1234
        }

        internal static void ToNullableExample_WithoutValue()
        {
            Console.WriteLine("ToNullable example without value");
            var maybe = Maybe<int>.Nothing;
            var v = maybe.ToNullable();
            Console.WriteLine($"Print Value: {v}");
            // Print Value: 
        }

        internal static void ToMaybeExample_WithObjectValue()
        {
            Console.WriteLine("ToMaybe example with object");
            var myObject = new { PropertyA = "Value Of Property A", PropertyB = 66 };
            var maybe = myObject.ToMaybe();
            Console.WriteLine($"Print Value: {maybe}");
            // Print Value: { PropertyA = Value Of Property A, PropertyB = 66 }
            Console.WriteLine($"Maybe PropertyA value: {maybe.Select(o => o.PropertyA)}");
            // Maybe PropertyA value: Value Of Property A
            Console.WriteLine($"Maybe PropertyB value: {maybe.Select(o => o.PropertyB)}");
            // Maybe PropertyB value: 66
        }

        internal static void ToMaybeExample_WithStructValue()
        {
            Console.WriteLine("ToMaybe example with struct");
            var myInt = 66;
            var maybe = myInt.ToMaybe();
            Console.WriteLine($"Print Value: {maybe}");
            // Print Value: 66
            Console.WriteLine($"Print Maybe value: {maybe.Select(i => i)}");
            //Print Maybe value: 66
        }

        internal static void ToMaybeExample_WithMaybeValue()
        {
            Console.WriteLine("ToMaybe example with a Maybe of an object");
            var myObject = new { PropertyA = "Value Of Property A", PropertyB = 66 };
            var maybe = myObject.ToMaybe();
            var secondMaybe = maybe.ToMaybe();
            Console.WriteLine($"First Maybe Print Value: {maybe}");
            // First Maybe Print Value: { PropertyA = Value Of Property A, PropertyB = 66 }
            Console.WriteLine($"Second Maybe Print Value: {secondMaybe}");
            // Second Maybe Print Value: { PropertyA = Value Of Property A, PropertyB = 66 }
        }

        internal static void ToMaybeExample_WithMaybeNothingValue()
        {
            Console.WriteLine("ToMaybe example with a Maybe of an object");
            var maybe = Maybe<string>.Nothing;
            var secondMaybe = maybe.ToMaybe();
            Console.WriteLine($"First Maybe Print Value: {maybe}");
            // First Maybe Print Value: 
            Console.WriteLine($"Second Maybe Print Value: {secondMaybe}");
            // Second Maybe Print Value: 
        }
    }
}
