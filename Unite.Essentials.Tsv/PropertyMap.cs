using System.Linq.Expressions;
using System.Reflection;
using Unite.Essentials.Tsv.Converters;

namespace Unite.Essentials.Tsv;


public class PropertyMap
{
    public string ColumnName { get; set; }
    public string PropertyName { get; set; }
    public Type PropertyType { get; set; }
    public MemberInfo[] PropertyPath { get; set; }
    public bool IsNullable { get; set; }
    public bool IsNested { get; set; }
    public IConverter Converter { get; set; }

    public PropertyMap()
    {
        
    }


    internal static PropertyMap Create(PropertyInfo property, string columnName = null, IConverter converter = null)
    {
        var propertyType = GetPropertyType(property, out var isNullable);
        var propertyPath = new[] { property };

        return new PropertyMap()
        {
            ColumnName = columnName,
            PropertyName = property.Name,
            PropertyType = propertyType,
            PropertyPath = propertyPath,
            IsNullable = isNullable,
            Converter = converter
        };
    }

    public static PropertyMap Create<T, TProp>(Expression<Func<T, TProp>> property, string columnName = null, IConverter<TProp> converter = null)
        where T : class
    {
        var propertyInfo = GetProperty(property, out var propertyPath);
        var propertyType = GetPropertyType(propertyInfo, out var isNullable);
        var propertyName = string.Join(".", propertyPath.Select(member => member.Name));

        return new PropertyMap() 
        {
            ColumnName = columnName,
            PropertyName = propertyName,
            PropertyType = propertyType,
            PropertyPath = propertyPath,
            IsNullable = isNullable,
            Converter = converter
        };
    }

    public static PropertyMap Nest<T, TProp>(Expression<Func<T, TProp>> property)
        where T : class
    {
        var propertyInfo = GetProperty(property, out var propertyPath);
        var propertyType = GetPropertyType(propertyInfo, out var isNullable);
        var propertyName = string.Join(".", propertyPath.Select(member => member.Name));

        return new PropertyMap()
        {
            ColumnName = null,
            PropertyName = propertyName,
            PropertyType = propertyType,
            PropertyPath = propertyPath,
            IsNullable = isNullable,
            IsNested = true
        };
    }


    protected static Type GetPropertyType(PropertyInfo property, out bool isNullable)
    {
        var actualType = property.PropertyType;
        var underlyingType = Nullable.GetUnderlyingType(actualType);

        if (underlyingType != null)
        {
            isNullable = true;
            return underlyingType;
        }
        else
        {
            isNullable = false;
            return actualType;
        }
    }

    protected static PropertyInfo GetProperty<T, TProp>(Expression<Func<T, TProp>> property, out MemberInfo[] path)
        where T : class
    {
        var visitor = new PropertyVisitor();
        visitor.Visit(property.Body);
        visitor.Path.Reverse();

        path = visitor.Path.ToArray();

        return visitor.Path.Last() as PropertyInfo;
    }

    private class PropertyVisitor : ExpressionVisitor
    {
        internal readonly List<MemberInfo> Path = new List<MemberInfo>();

        protected override Expression VisitMember(MemberExpression node)
        {
            if (!(node.Member is PropertyInfo))
            {
                throw new ArgumentException("The path can only contain properties", nameof(node));
            }

            Path.Add(node.Member);

            return base.VisitMember(node);
        }
    }
}
