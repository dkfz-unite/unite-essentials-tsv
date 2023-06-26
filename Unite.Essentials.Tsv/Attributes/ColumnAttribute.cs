using Unite.Essentials.Tsv.Converters;

namespace Unite.Essentials.Tsv.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ColumnAttribute : Attribute
{
    public string Name { get; protected set; }
    public IConverter Converter { get; protected set; }

    public ColumnAttribute(string name = null, Type converterType = null) : base()
    { 
        Name = name;

        if (converterType != null)
        {
            if ((typeof(IConverter)).IsAssignableFrom(converterType))
            {
                Converter = (IConverter)Activator.CreateInstance(converterType);
            }
            else
            {
                throw new ArgumentException($"Type '{converterType.Name}' should implement 'IConverter' interface and have parameterless constructor.");
            }
        }
    }
}
