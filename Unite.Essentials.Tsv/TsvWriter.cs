using System.Reflection;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Essentials.Tsv;

public class TsvWriter
{
    /// <summary>
    /// Writes the specified rows to a tsv string.
    /// </summary>
    /// <typeparam name="T">Model type.</typeparam>
    /// <param name="rows">Rows to write.</param>
    /// <param name="map">Class map.</param>
    /// <param name="header">Include header row.</param>
    /// <param name="comments">Comments to include.</param>
    /// <returns>Tsv string.</returns>
    public static string Write<T>(IEnumerable<T> rows, ClassMap<T> map = null, bool header = true, IEnumerable<string> comments = null) where T : class
    {
        using var stream = new MemoryStream();

        using (var writer = new StreamWriter(stream, leaveOpen: true))
        {
            Write(writer, rows, map, header, comments);
        }

        stream.Position = 0;

        using (var reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }

    /// <summary>
    /// Writes the specified rows to a stream.
    /// </summary>
    /// <typeparam name="T">Model type.</typeparam>
    /// <param name="streamWriter">Stream writer.</param>
    /// <param name="rows">Rows to write.</param>
    /// <param name="map">Class map.</param>
    /// <param name="header">Include header row.</param>
    public static void Write<T>(StreamWriter streamWriter, IEnumerable<T> rows, ClassMap<T> map = null, bool header = true, IEnumerable<string> comments = null) where T : class
    {
        var classMap = map ?? new ClassMap<T>().AutoMap();
        var properties = classMap.Properties.Where(propertyMap => !propertyMap.IsNested);

        if (comments != null)
        {
            foreach (var comment in comments)
            {
                streamWriter.WriteLine($"#{comment}");
            }
        }

        if (header)
        {
            var headerRow = string.Join('\t', properties.Select(propertyMap => propertyMap.ColumnName ?? propertyMap.PropertyName));

            streamWriter.WriteLine(headerRow);
        }

        foreach (var row in rows)
        {
            var dataRow = string.Join('\t', properties.Select(propertyMap =>
            {
                var rawValue = GetValue(propertyMap.PropertyPath, row);

                if (rawValue == null || rawValue == default)
                {
                    return string.Empty;
                }
                else
                {
                    if (propertyMap.Converter != null)
                    {
                        return propertyMap.Converter.Convert(rawValue, row);
                    }
                    else if (propertyMap.PropertyType.IsEnum)
                    {
                        return EnumMemberConverter.Convert(rawValue, propertyMap.PropertyType);
                    }
                    else
                    {
                        return BaseTypeConverter.Convert(rawValue, propertyMap.PropertyType);
                    }
                }
            }));

            streamWriter.WriteLine(dataRow);
        }
    }


    private static object GetValue(IEnumerable<MemberInfo> path, object entry)
    {
        if (entry == null)
        {
            return null;
        }

        var property = (PropertyInfo)path.First();

        var value = property.GetValue(entry);

        var remainingPath = path.Skip(1);

        if (remainingPath.Any())
        {
            return GetValue(remainingPath, value);
        }
        else
        {
            return value;
        }
    }
}
