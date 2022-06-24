using System.Linq.Expressions;
using System.Reflection;

namespace Unite.Essentials.Tsv;

public class ColumnMap<T> where T : class
{
    public string Name { get; protected set; }
    public int? Index { get; protected set; }
    public PropertyInfo Property { get; protected set; }


    public ColumnMap()
    {

    }

    public ColumnMap(PropertyInfo property)
    {
        Property = property;
        Name = property.Name;
    }

    public ColumnMap(PropertyInfo property, string name = null, int? index = null) : this(property)
    {
        if (name != null)
            Name = name;

        if (index != null)
            Index = index;
    }


    public void SetValue(T target, object value)
    {
        Property.SetValue(target, value);
    }

    public object GetValue(T target)
    {
        return Property.GetValue(target);
    }


    protected PropertyInfo GetProperty<TProp>(Expression<Func<T, TProp>> property)
    {
        var memberExpression = property?.Body as MemberExpression;
        var propertyInfo = memberExpression?.Member as PropertyInfo;
        return propertyInfo;
    }
}

public class ColumnMap<T, TProp> : ColumnMap<T> where T : class
{
    public ColumnMap(Expression<Func<T, TProp>> property)
    {
        Property = GetProperty(property);
        Name = Property.Name;
    }

    public ColumnMap(Expression<Func<T, TProp>> property, string name = null, int? index = null) : this(property)
    {
        if (name != null)
            Name = name;

        if (index != null)
            Index = index;
    }
}
