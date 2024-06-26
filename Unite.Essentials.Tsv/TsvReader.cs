﻿using System.Reflection;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Essentials.Tsv;

public class TsvReader
{
    /// <summary>
    /// Reads tsv string and returns collection of objects of type T.
    /// </summary>
    /// <typeparam name="T">Model type.</typeparam>
    /// <param name="tsv">Tsv string.</param>
    /// <param name="map">Class map.</param>
    /// <param name="header">Header row is present.</param>
    /// <param name="comments">List to cellect comments to (ignored if null).</param>
    /// <returns>Collection of objects of type T.</returns>
    public static IEnumerable<T> Read<T>(string tsv, ClassMap<T> map = null, bool header = true, IList<string> comments = null) where T : class
    {
        using var reader = new StringReader(tsv);

        var readComments = true;
        var readHeader = header;
        var classMap = map ?? new ClassMap<T>().AutoMap();
        var columnsMap = GetColumnsMap(classMap);

        while (reader.Peek() != -1)
        {
            var line = reader.ReadLine();

            if (readComments)
            {
                if (line.StartsWith("#"))
                {
                    comments?.Add(line.TrimStart('#'));
                    continue;
                }
                else
                {
                    readComments = false;
                }
            }

            if (readHeader)
            {
                columnsMap = GetColumnsMap(line);
                readHeader = false;
                continue;
            }
            
            var entry = Activator.CreateInstance<T>();
            ParseLine(line, columnsMap, classMap, ref entry);

            yield return entry;
        }
    }

    /// <summary>
    /// Reads tsv stream and returns collection of objects of type T.
    /// </summary>
    /// <typeparam name="T">Model type.</typeparam>
    /// <param name="stream">Tsv stream.</param>
    /// <param name="map">Class map.</param>
    /// <param name="header">Header row is present.</param>
    /// <param name="comments">List to cellect comments to (ignored if null).</param>
    /// <returns>Collection of objects of type T.</returns>
    public static IEnumerable<T> Read<T>(StreamReader reader, ClassMap<T> map = null, bool header = true, IList<string> comments = null) where T : class
    {
        var readComments = true;
        var readHeader = header;
        var classMap = map ?? new ClassMap<T>().AutoMap();
        var columnsMap = GetColumnsMap(classMap);

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();

            if (readComments)
            {
                if (line.StartsWith("#"))
                {
                    comments?.Add(line.TrimStart('#'));
                    continue;
                }
                else
                {
                    readComments = false;
                }
            }

            if (readHeader)
            {
                columnsMap = GetColumnsMap(line);
                readHeader = false;
                continue;
            }

            var entry = Activator.CreateInstance<T>();
            ParseLine(line, columnsMap, classMap, ref entry);

            yield return entry;
        }
    }


    private static void ParseLine<T>(string dataRow, IDictionary<string, int> columnsMap, ClassMap<T> classMap, ref T dataEntry) where T : class
    {
        var columns = dataRow.Split('\t');

        foreach (var propertyMap in classMap.Properties)
        {
            var columnIndex = propertyMap.ColumnName != null 
                ? columnsMap.ContainsKey(propertyMap.ColumnName) ? columnsMap[propertyMap.ColumnName] : -1
                : columnsMap.ContainsKey(propertyMap.PropertyName) ? columnsMap[propertyMap.PropertyName] : -1;

            if (columnIndex < 0)
            {
                continue;
                // throw new Exception($"Column '{propertyMap.Name ?? propertyMap.PropertyName}' not found.");
            }

            if (columnIndex >= columns.Length)
            {
                continue;
                // throw new Exception($"Column '{propertyMap.Name ?? propertyMap.PropertyName}' index is greater than columns number.");
            }

            var rawValue = columns[columnIndex];

            if (string.IsNullOrWhiteSpace(rawValue))
            {
                continue;
            }
            else
            {
                if (propertyMap.Converter != null)
                {
                    var convertedValue = propertyMap.Converter.Convert(rawValue, dataRow);
                    SetValue(propertyMap.PropertyPath, dataEntry, convertedValue);
                }
                else if (propertyMap.PropertyType.IsEnum)
                {
                    var convertedValue = EnumMemberConverter.Convert(rawValue, propertyMap.PropertyType);
                    SetValue(propertyMap.PropertyPath, dataEntry, convertedValue);
                }
                else
                {
                    var convertedValue = BaseTypeConverter.Convert(rawValue, propertyMap.PropertyType);
                    SetValue(propertyMap.PropertyPath, dataEntry, convertedValue);
                }
            }
        }
    }

    private static void SetValue(IEnumerable<MemberInfo> path, object entry, object value)
    {
        if (value == null)
        {
            return;
        }

        if (path.Count() == 1)
        {
            var property = (PropertyInfo)path.First();
            property.SetValue(entry, value);
        }
        else
        {
            var property = (PropertyInfo)path.First();
            var subEntry = property.GetValue(entry);

            if (subEntry == null)
            {
                subEntry = Activator.CreateInstance(property.PropertyType);
                property.SetValue(entry, subEntry);
            }

            var remainingPath = path.Skip(1);
            SetValue(remainingPath, subEntry, value);
        }
    }

    private static Dictionary<string, int> GetColumnsMap(string row)
    {
        var columns = row.Split('\t');
        var columnsMap = new Dictionary<string, int>();

        for (var i = 0; i < columns.Length; i++)
        {
            columnsMap.Add(columns[i], i);
        }

        return columnsMap;
    }

    private static Dictionary<string, int> GetColumnsMap<T>(ClassMap<T> classMap) where T : class
    {
        var properties = classMap.Properties.ToArray();

        var columnsMap = new Dictionary<string, int>();
        
        for (var i = 0; i < properties.Length; i++)
        {
            columnsMap.Add(properties[i].ColumnName ?? properties[i].PropertyName, i);
        }

        return columnsMap;
    }
}
