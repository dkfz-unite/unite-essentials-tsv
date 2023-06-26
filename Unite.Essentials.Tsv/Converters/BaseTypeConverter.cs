using System.Globalization;

namespace Unite.Essentials.Tsv.Converters;

internal class BaseTypeConverter
{
    internal static object Convert(string value, Type type, IFormatProvider formatProvider = null)
    {
        var format = formatProvider ?? CultureInfo.InvariantCulture;

        try 
        {
            return Type.GetTypeCode(type) switch
            {
                TypeCode.String => value,
                TypeCode.Char => char.Parse(value),
                TypeCode.Boolean => bool.Parse(value),
                TypeCode.DateTime => DateTime.Parse(value, format),
                TypeCode.Byte => byte.Parse(value, NumberStyles.Any, format),
                TypeCode.UInt16 => ushort.Parse(value, NumberStyles.Any, format),
                TypeCode.UInt32 => uint.Parse(value, NumberStyles.Any, format),
                TypeCode.UInt64 => ulong.Parse(value, NumberStyles.Any, format),
                TypeCode.SByte => sbyte.Parse(value, NumberStyles.Any, format),
                TypeCode.Int16 => short.Parse(value, NumberStyles.Any, format),
                TypeCode.Int32 => int.Parse(value, NumberStyles.Any, format),
                TypeCode.Int64 => long.Parse(value, NumberStyles.Any, format),
                TypeCode.Decimal => decimal.Parse(value, NumberStyles.Any, format),
                TypeCode.Double => double.Parse(value, NumberStyles.Any, format),
                TypeCode.Single => float.Parse(value, NumberStyles.Any, format),
                _ => null
            };
        }
        catch
        {
            throw new Exception($"Specified value '{value}' cannot be converted to type '{type.Name}'.");
        }
    }

    internal static string Convert(object value, Type type, IFormatProvider formatProvider = null)
    {
        var format = formatProvider ?? CultureInfo.InvariantCulture;

        try
        {
            return Type.GetTypeCode(type) switch
            {
                TypeCode.String => value.ToString(),
                TypeCode.Char => value.ToString(),
                TypeCode.Boolean => ((bool)value).ToString(format),
                TypeCode.DateTime => ((DateTime)value).ToString(format),
                TypeCode.SByte => ((sbyte)value).ToString(format),
                TypeCode.Byte => ((byte)value).ToString(format),
                TypeCode.Int16 => ((short)value).ToString(format),
                TypeCode.Int32 => ((int)value).ToString(format),
                TypeCode.Int64 => ((long)value).ToString(format),
                TypeCode.UInt16 => ((ushort)value).ToString(format),
                TypeCode.UInt32 => ((uint)value).ToString(format),
                TypeCode.UInt64 => ((ulong)value).ToString(format),
                TypeCode.Decimal => ((decimal)value).ToString(format),
                TypeCode.Double => ((double)value).ToString(format),
                TypeCode.Single => ((float)value).ToString(format),
                _ => string.Empty
            };
        }
        catch
        {
            throw new Exception($"Specified value '{value}' of type '{type.Name}' cannot be converted to string.");
        }
    }
}
