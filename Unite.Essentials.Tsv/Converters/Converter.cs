using System;
using System.Reflection;

namespace Unite.Essentials.Tsv.Converters
{
    internal class Converter
    {
        internal static object Convert(string value, PropertyInfo property)
        {
            var actualType = GetActualType(property.PropertyType, out var isNullable);

            if (string.IsNullOrWhiteSpace(value))
            {
                if (isNullable || actualType.IsAssignableFrom(typeof(string)))
                    return null;
                else
                    throw new ArgumentNullException(property.Name, "Value of non-nullable type properties should not be empty");
            }

            if (actualType.IsAssignableFrom(typeof(string)))
                return value;
            else if (actualType.IsAssignableFrom(typeof(bool)))
                return bool.Parse(value);
            else if (actualType.IsAssignableFrom(typeof(int)))
                return int.Parse(value);
            else if (actualType.IsAssignableFrom(typeof(double)))
                return double.Parse(value);
            else if (actualType.IsAssignableFrom(typeof(DateTime)))
                return DateTime.Parse(value);
            else if (actualType.IsEnum)
                return EnumConverter.Parse(actualType, value);
            else
                throw new NotSupportedException($"Type '{property.PropertyType.Name}' is not supported");
        }

        private static Type GetActualType(Type type, out bool isNullable)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);

            if (underlyingType != null)
            {
                isNullable = true;
                return underlyingType;
            }
            else
            {
                isNullable = false;
                return type;
            }
        }
    }
}
