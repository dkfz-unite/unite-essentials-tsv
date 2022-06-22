using System;

namespace Unite.Essentials.Tsv.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ColumnAttribute : Attribute
{
    public string Name { get; protected set; }
    public int? Index { get; protected set; }

    public ColumnAttribute() : base()
    {

    }

    /// <summary>
    /// Map column by name
    /// </summary>
    /// <param name="name">Column name</param>
    public ColumnAttribute(string name) : this()
    {
        Name = name;
    }

    /// <summary>
    /// Map column by index
    /// </summary>
    /// <param name="index">Column index starting from '0'</param>
    public ColumnAttribute(int index) : this()
    {
        Index = index;
    }

    /// <summary>
    /// Map column by name at read, order by index at write
    /// </summary>
    /// <param name="name">Column name at read</param>
    /// <param name="index">Column index at write starting from '0'</param>
    public ColumnAttribute(string name, int index) : this()
    {
        Name = name;
        Index = index;
    }
}
