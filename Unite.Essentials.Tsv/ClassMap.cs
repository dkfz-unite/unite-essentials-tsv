using System.Linq.Expressions;
using System.Reflection;
using Unite.Essentials.Tsv.Attributes;

namespace Unite.Essentials.Tsv;

public class ClassMap<T> where T : class
{
    private List<ColumnMap<T>> _columns;

    public IEnumerable<ColumnMap<T>> Columns => _columns;


    public ClassMap()
    {
        _columns = new List<ColumnMap<T>>();
    }

    public ClassMap(IEnumerable<ColumnMap<T>> columns)
    {
        _columns = columns.ToList();
    }

    public ClassMap<T> Map(PropertyInfo property, string name = null, int? index = null)
    {
        _columns.Add(new ColumnMap<T>(property, name, index));

        return this;
    }

    public ClassMap<T> Map<TProp>(Expression<Func<T, TProp>> property, string name = null, int? index = null)
    {
        _columns.Add(new ColumnMap<T, TProp>(property, name, index));

        return this;
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
                Map(property, columnAttribute.Name, columnAttribute.Index);
            }
        }
    }
}
