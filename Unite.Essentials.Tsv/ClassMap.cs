using System.Linq.Expressions;
using System.Reflection;
using Unite.Essentials.Tsv.Attributes;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Essentials.Tsv;

public class ClassMap<T> where T : class
{
    private List<PropertyMap> _properties;

    public IEnumerable<PropertyMap> Properties => _properties;


    public ClassMap()
    {
        _properties = new List<PropertyMap>();
    }


    public ClassMap<T> AutoMap()
    {
        var type = typeof(T);
        var properties = type.GetProperties();
        var hasAttributes = properties.Any(propery => propery.GetCustomAttribute<ColumnAttribute>() != null);

        if (hasAttributes)
        {
            AutoMapByAttributes(properties);
        }
        else
        {
            AutoMapByProperties(properties);
        }

        return this;
    }

    public ClassMap<T> Map<TProp>(Expression<Func<T, TProp>> property, string columnName = null, IConverter<TProp> converter = null)
    {
        var newMap = PropertyMap.Create(property, columnName, converter);

        var existingMap = _properties.FirstOrDefault(map => map.PropertyName == newMap.PropertyName);

        if (existingMap != null)
        {
            existingMap.ColumnName = newMap.ColumnName;
            existingMap.Converter = newMap.Converter;
        }
        else
        {
            _properties.Add(newMap);
        }

        return this;
    }

    public ClassMap<T> Ignore<TProp>(Expression<Func<T, TProp>> property, string columnName = null)
    {
        var newMap = PropertyMap.Create(property, columnName);

        var existingMap = _properties.FirstOrDefault(map => map.PropertyName == newMap.PropertyName);

        if (existingMap != null)
        {
            _properties.Remove(existingMap);
        }

        return this;
    }

    public ClassMap<T> Nest<TProp>(Expression<Func<T, TProp>> property)
    {
        var newMap = PropertyMap.Nest(property);

        var existingMap = _properties.FirstOrDefault(map => map.PropertyName == newMap.PropertyName);

        if (existingMap != null)
        {
            existingMap.IsNested = true;
        }
        else
        {
            _properties.Add(newMap);
        }

        return this;
    }


    private ClassMap<T> Map(PropertyInfo property, string columnName = null, IConverter converter = null)
    {
        var newMap = PropertyMap.Create(property, columnName, converter);
    
        var existingMap = _properties.FirstOrDefault(map => map.PropertyName == newMap.PropertyName);

        if (existingMap != null)
        {
            existingMap.ColumnName = newMap.ColumnName;
            existingMap.Converter = newMap.Converter;
        }
        else
        {
            _properties.Add(newMap);
        }

        return this;
    }

    private void AutoMapByProperties(PropertyInfo[] properties)
    {
        foreach (var property in properties)
        {
            Map(property);
        }
    }

    private void AutoMapByAttributes(PropertyInfo[] properties)
    {
        foreach (var property in properties)
        {
            var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();

            if (columnAttribute != null)
            {
                Map(property, columnAttribute.Name, columnAttribute.Converter);
            }
        }
    }
}
