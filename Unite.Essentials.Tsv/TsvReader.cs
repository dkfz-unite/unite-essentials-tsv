using Unite.Essentials.Tsv.Converters;

namespace Unite.Essentials.Tsv;

public class TsvReader
{
    public static IEnumerable<T> Read<T>(string filePath, ClassMap<T> map = null) where T : class
    {
        ValidateFilePath(filePath);

        using var reader = new StreamReader(filePath);

        var headerRow = reader.ReadLine();
        var headerColumns = GetColumnIndices(headerRow);
        var classMap = map ?? new ClassMap<T>().AutoMap();

        while (!reader.EndOfStream)
        {
            var dataRow = reader.ReadLine();
            var dataColumns = dataRow.Split('\t');
            var dataEntry = Activator.CreateInstance<T>();

            foreach (var columnMap in classMap.Columns)
            {
                var rawValue = headerColumns.ContainsKey(columnMap.Name) ? dataColumns[headerColumns[columnMap.Name]] : null;
                var convertedValue = Converter.Convert(rawValue, columnMap.Property);
                columnMap.SetValue(dataEntry, convertedValue);
            }

            yield return dataEntry;
        }
    }

    public static IEnumerable<T> Read<T>(Stream stream, ClassMap<T> map = null) where T : class
    {
        using var reader = new StreamReader(stream);

        var headerRow = reader.ReadLine();
        var headerColumns = GetColumnIndices(headerRow);
        var classMap = map ?? new ClassMap<T>().AutoMap();

        while (!reader.EndOfStream)
        {
            var dataRow = reader.ReadLine();
            var dataColumns = dataRow.Split('\t');
            var dataEntry = Activator.CreateInstance<T>();

            foreach (var columnMap in classMap.Columns)
            {
                var rawValue = headerColumns.ContainsKey(columnMap.Name) ? dataColumns[headerColumns[columnMap.Name]] : null;
                var convertedValue = Converter.Convert(rawValue, columnMap.Property);
                columnMap.SetValue(dataEntry, convertedValue);
            }

            yield return dataEntry;
        }
    }


    private static Dictionary<string, int> GetColumnIndices(string header)
    {
        var columns = header.Split('\t');

        var columnsMap = new Dictionary<string, int>();

        for (var i = 0; i < columns.Length; i++)
        {
            columnsMap.Add(columns[i], i);
        }

        return columnsMap;
    }

    private static void ValidateFilePath(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentNullException(nameof(filePath), "File path has to be set");
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File does not exist or is not accessible", filePath);
        }
    }
}
