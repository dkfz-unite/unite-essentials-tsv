using System.Globalization;

namespace Unite.Essentials.Tsv.Converters;

internal class BaseTypeConverter
{
    internal static object Convert(string value, Type type, IFormatProvider formatProvider = null)
    {
        var format = formatProvider ?? CultureInfo.InvariantCulture;

        try 
        {
            return type.Name switch
            {
                nameof(String) => value,
                nameof(Char) => char.Parse(value),
                nameof(Boolean) => bool.Parse(value),
                nameof(DateTime) => DateTime.Parse(value, format),
                nameof(DateOnly) => DateOnly.Parse(value, format),
                nameof(TimeOnly) => TimeOnly.Parse(value, format),
                nameof(Byte) => byte.Parse(value, NumberStyles.Any, format),
                nameof(UInt16) => ushort.Parse(value, NumberStyles.Any, format),
                nameof(UInt32) => uint.Parse(value, NumberStyles.Any, format),
                nameof(UInt64) => ulong.Parse(value, NumberStyles.Any, format),
                nameof(SByte) => sbyte.Parse(value, NumberStyles.Any, format),
                nameof(Int16) => short.Parse(value, NumberStyles.Any, format),
                nameof(Int32) => int.Parse(value, NumberStyles.Any, format),
                nameof(Int64) => long.Parse(value, NumberStyles.Any, format),
                nameof(Decimal) => decimal.Parse(value, NumberStyles.Any, format),
                nameof(Double) => double.Parse(value, NumberStyles.Any, format),
                nameof(Single) => float.Parse(value, NumberStyles.Any, format),
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
            return type.Name switch
            {
                nameof(String) => value.ToString(),
                nameof(Char) => value.ToString(),
                nameof(Boolean) => ((bool)value).ToString(format),
                nameof(DateTime) => ((DateTime)value).ToString(format),
                nameof(DateOnly) => ((DateOnly)value).ToString(format),
                nameof(TimeOnly) => ((TimeOnly)value).ToString(format),
                nameof(Byte) => ((byte)value).ToString(format),
                nameof(UInt16) => ((ushort)value).ToString(format),
                nameof(UInt32) => ((uint)value).ToString(format),
                nameof(UInt64) => ((ulong)value).ToString(format),
                nameof(SByte) => ((sbyte)value).ToString(format),
                nameof(Int16) => ((short)value).ToString(format),
                nameof(Int32) => ((int)value).ToString(format),
                nameof(Int64) => ((long)value).ToString(format),
                nameof(Decimal) => ((decimal)value).ToString(format),
                nameof(Double) => ((double)value).ToString(format),
                nameof(Single) => ((float)value).ToString(format),
                _ => string.Empty
            };
        }
        catch
        {
            throw new Exception($"Specified value '{value}' of type '{type.Name}' cannot be converted to string.");
        }
    }
}
