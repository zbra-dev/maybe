using System;
using System.Linq;
using System.Reflection;

namespace Maybe
{
    public static class MaybeReflection
    {
        public static object ReflectionMaybeNothing(Type returnValueType)
        {
            Type generic = typeof(Maybe<>);
            Type constructed = generic.MakeGenericType(new Type[] { returnValueType });

            return constructed.GetMethod("ValueOfNothing").MakeGenericMethod(new Type[] { returnValueType }).Invoke(null, null);
        }

        public static object ReflectionMaybeStruct(Type returnValueType, object val)
        {
            Type generic = typeof(Maybe<>);
            Type constructed = generic.MakeGenericType(new Type[] { returnValueType });

            return constructed.GetMethod("ValueOfStruct").MakeGenericMethod(new Type[] { returnValueType }).Invoke(null, new object[] { val });
        }


        public static object ReflectionMaybe(Type returnValueType, object val)
        {
            Type generic = typeof(Maybe<>);
            Type constructed = generic.MakeGenericType(new Type[] { returnValueType });

            return constructed.GetMethod("ValueOfValue", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(new Type[] { returnValueType }).Invoke(null, new object[] { val });
        }

        public static bool IsMaybe(this Type type)
        {
            return type.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(Maybe<>))) ||
                type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Maybe<>));
        }

        public static Maybe<object> ExtractMaybe(object value)
        {
            if (!(bool)value.GetType().GetProperty("HasValue").GetValue(value))
            {
                return Maybe<object>.Nothing;
            }

            return value.GetType().GetProperty("Value").GetValue(value).ToMaybe();
        }
    }
}