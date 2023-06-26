using Unite.Essentials.Tsv.Tests.Models.Enums;

namespace Unite.Essentials.Tsv.Tests.Models;

public class TestModel
{
    public virtual string StringValue { get; set; }
    public virtual char? CharValue { get; set; }
    public virtual bool? BoolValue { get; set; }
    public virtual DateTime? DateValue { get; set; }
    public virtual byte? ByteValue { get; set; }
    public virtual ushort? UShortValue { get; set; }
    public virtual uint? UIntValue { get; set; }
    public virtual ulong? ULongValue { get; set; }
    public virtual sbyte? SByteValue { get; set; }
    public virtual short? ShortValue { get; set; }
    public virtual int? IntValue { get; set; }
    public virtual long? LongValue { get; set; }
    public virtual decimal? DecimalValue { get; set; }
    public virtual double? DoubleValue { get; set; }
    public virtual float? FloatValue { get; set; }
    public virtual TestEnum? EnumValue { get; set; }
    public virtual int[] ArrayValue { get; set; }
}
