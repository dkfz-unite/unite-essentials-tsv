using Unite.Essentials.Tsv.Attributes;
using Unite.Essentials.Tsv.Tests.Converters;
using Unite.Essentials.Tsv.Tests.Models.Enums;

namespace Unite.Essentials.Tsv.Tests.Models;

public class TestModelAttributed : TestModel
{
    public const string StringColumn = "string_value";
    public const string CharColum = "char_value";
    public const string BoolColumn = "bool_value";
    public const string DateColumn = "date_value";
    public const string ByteColumn = "byte_value";
    public const string UshortColumn = "ushort_value";
    public const string UintColumn = "uint_value";
    public const string UlongColumn = "ulong_value";
    public const string SbyteColumn = "sbyte_value";
    public const string ShortColumn = "short_value";
    public const string IntColumn = "int_value";
    public const string LongColumn = "long_value";
    public const string DecimalColumn = "decimal_value";
    public const string DoubleColumn = "double_value";
    public const string FloatColumn = "float_value";
    public const string EnumColumn = "enum_value";
    public const string ArrayColumn = "array_value";


    [Column(StringColumn)]
    public override string StringValue { get; set; }

    [Column(CharColum)]
    public override char? CharValue { get; set; }

    [Column(BoolColumn)]
    public override bool? BoolValue { get; set; }

    [Column(DateColumn)]
    public override DateTime? DateValue { get; set; }

    [Column(ByteColumn)]
    public override byte? ByteValue { get; set; }

    [Column(UshortColumn)]
    public override ushort? UShortValue { get; set; }

    [Column(UintColumn)]
    public override uint? UIntValue { get; set; }

    [Column(UlongColumn)]
    public override ulong? ULongValue { get; set; }

    [Column(SbyteColumn)]
    public override sbyte? SByteValue { get; set; }

    [Column(ShortColumn)]
    public override short? ShortValue { get; set; }

    [Column(IntColumn)]
    public override int? IntValue { get; set; }

    [Column(LongColumn)]
    public override long? LongValue { get; set; }

    [Column(DecimalColumn)]
    public override decimal? DecimalValue { get; set; }

    [Column(DoubleColumn)]
    public override double? DoubleValue { get; set; }

    [Column(FloatColumn)]
    public override float? FloatValue { get; set; }

    [Column(EnumColumn)]
    public override TestEnum? EnumValue { get; set; }

    [Column(ArrayColumn, typeof(ArrayConverter))]
    public override int[] ArrayValue { get; set; }
    
    public string ShadowValue { get; set; }

    public static string TsvHeader()
    {
        return string.Join('\t', 
            StringColumn, CharColum, BoolColumn, DateColumn, 
            ByteColumn, UshortColumn, UintColumn, UlongColumn, 
            SbyteColumn, ShortColumn, IntColumn, LongColumn, 
            DecimalColumn, DoubleColumn, FloatColumn, 
            EnumColumn, ArrayColumn);
    }
}
