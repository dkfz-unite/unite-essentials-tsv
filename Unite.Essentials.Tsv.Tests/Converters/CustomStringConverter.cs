using System.Text.Json;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Essentials.Tsv.Tests.Converters;

public class ArrayConverter : IConverter<int[]>
{
    public object Convert(string value)
    {
        return JsonSerializer.Deserialize<int[]>(value);
    }

    public string Convert(object value)
    {
        return JsonSerializer.Serialize((int[])value);
    }
}
