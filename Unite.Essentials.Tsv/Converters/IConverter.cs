namespace Unite.Essentials.Tsv.Converters;

/// <summary>
/// Interface for converters.
/// </summary>
public interface IConverter
{
    /// <summary>
    /// Converts string to value of required type.
    /// </summary>
    /// <param name="value">String to convert.</param>
    /// <returns>Value converted from input string.</returns>
    public object Convert(string value);

    /// <summary>
    /// Converts value to string.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    /// <returns>String representation of the value.</returns>
    public string Convert(object value);
}

/// <summary>
/// Generic interface for converters.
/// </summary>
public interface IConverter<T> : IConverter
{
}
