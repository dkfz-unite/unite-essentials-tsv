namespace Unite.Essentials.Tsv.Converters;

/// <summary>
/// Interface for converters.
/// </summary>
public interface IConverter
{
    /// <summary>
    /// Converts string to value of required type.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    /// <param name="row">Row string.</param>
    /// <returns>Value converted from input string.</returns>
    public object Convert(string value, string row);

    /// <summary>
    /// Converts value to string.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    /// <param name="row">Row object.</param>
    /// <returns>String representation of the value.</returns>
    public string Convert(object value, object row);
}

/// <summary>
/// Generic interface for converters.
/// </summary>
public interface IConverter<T> : IConverter
{
}
