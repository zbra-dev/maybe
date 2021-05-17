using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Maybe
{
    internal class GenericTypeConverter
    {
        private static readonly GenericTypeConverter converter = new GenericTypeConverter();

        private IDictionary<Type, Func<string, object>> types = new Dictionary<Type, Func<string, object>>();

        public GenericTypeConverter()
        {
            AddConverter(typeof(long), s => new Int64Converter().ConvertFromInvariantString(s));
            AddConverter(typeof(int), s => new Int32Converter().ConvertFromInvariantString(s));
            AddConverter(typeof(short), s => new Int16Converter().ConvertFromInvariantString(s));
            AddConverter(typeof(byte), s => new SByteConverter().ConvertFromInvariantString(s));
            AddConverter(typeof(bool), s => "true".Equals(s.ToLower()));
            AddConverter(typeof(double), s => new DoubleConverter().ConvertFromInvariantString(s));
            AddConverter(typeof(float), s => (float)new DoubleConverter().ConvertFromInvariantString(s));
            AddConverter(typeof(decimal), s => new DecimalConverter().ConvertFromInvariantString(s));
        }

        public T Convert<T>(string value)
        {
            return DoConvert<T>(value);
        }

        public object Convert<T>(string value, Type type)
        {
            return DoConvert(value, type);
        }

        public static GenericTypeConverter GetConverter()
        {
            return converter;
        }

        public void AddConverter(Type type, Func<string, object> func)
        {
            types.Add(type, func);
        }

        protected object DoConvert(string value, Type type)
        {
            var func = types.MaybeGet(type);

            if (func.HasValue)
            {
                try
                {
                    return func.Value(value);
                }
                catch (Exception e)
                {
                    throw new Exception(value + " is not valid for a conversion to " + type.Name, e);
                }
            }
            else
            {
                throw new Exception("Cannot convert type " + type.Name);
            }
        }

        protected T DoConvert<T>(string value)
        {
            var func = types.MaybeGet(typeof(T));

            if (func.HasValue)
            {
                try
                {
                    return (T)func.Value(value);
                }
                catch (Exception e)
                {
                    throw new Exception(value + " is not valid for a conversion to " + typeof(T).Name, e);
                }
            }
            else
            {
                throw new Exception("Cannot convert type " + typeof(T).Name);
            }
        }
    }
}
