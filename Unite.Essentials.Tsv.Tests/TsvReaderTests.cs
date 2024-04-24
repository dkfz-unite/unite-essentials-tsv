using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unite.Essentials.Tsv.Tests.Converters;
using Unite.Essentials.Tsv.Tests.Models;
using Unite.Essentials.Tsv.Tests.Models.Enums;

namespace Unite.Essentials.Tsv.Tests;

[TestClass]
public class TsvReaderTests
{
    [TestMethod]
    public void ShoulReadByAttributes()
    {
        var tsv = new StringBuilder();
        tsv.AppendLine("#comment1");
        tsv.AppendLine("#comment2");
        tsv.AppendLine(TestModelAttributed.TsvHeader());
        tsv.AppendLine(string.Join('\t', "string", "c", "true", "2022-01-10 10:00:00", "2022-01-10", "10:00:00", "255", "65535", "65536", "65537", "-128", "-32768", "-32769", "-32770", "65540", "1.79E+308", "3.4028E+38", "A"));
        tsv.AppendLine(string.Join('\t', "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));

        using var stream = PrepareStream(tsv.ToString());
        using var reader = new StreamReader(stream);

        var comments = new List<string>();
        var models = TsvReader.Read<TestModelAttributed>(reader, comments: comments).ToArray();
        Assert.AreEqual(2, comments.Count);
        Assert.AreEqual("comment1", comments[0]);
        Assert.AreEqual("comment2", comments[1]);
        Assert.AreEqual(2, models.Length);
        AssertDataLine(models[0]);
        AssertEmptyLine(models[1]);
    }

    [TestMethod]
    public void ShoulReadByAttributesWithoutHeader()
    {
        var tsv = new StringBuilder();
        tsv.AppendLine(string.Join('\t', "string", "c", "true", "2022-01-10 10:00:00", "2022-01-10", "10:00:00", "255", "65535", "65536", "65537", "-128", "-32768", "-32769", "-32770", "65540", "1.79E+308", "3.4028E+38", "A"));
        tsv.AppendLine(string.Join('\t', "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));

        var models = TsvReader.Read<TestModelAttributed>(tsv.ToString(), header: false).ToArray();
        Assert.AreEqual(2, models.Length);
        AssertDataLine(models[0]);
        AssertEmptyLine(models[1]);
    }

    [TestMethod]
    public void ShoulReadByProperties()
    {
        var tsv = new StringBuilder();
        tsv.AppendLine("#comment1");
        tsv.AppendLine("#comment2");
        tsv.AppendLine(string.Join('\t', "StringValue", "CharValue", "BoolValue", "DateValue", "DateOnlyValue", "TimeOnlyValue", "ByteValue", "UShortValue", "UIntValue", "ULongValue", "SByteValue", "ShortValue", "IntValue", "LongValue", "DecimalValue", "DoubleValue", "FloatValue", "EnumValue"));
        tsv.AppendLine(string.Join('\t', "string", "c", "true", "2022-01-10 10:00:00", "2022-01-10", "10:00:00", "255", "65535", "65536", "65537", "-128", "-32768", "-32769", "-32770", "65540", "1.79E+308", "3.4028E+38", "A"));
        tsv.AppendLine(string.Join('\t', "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));

        using var stream = PrepareStream(tsv.ToString());
        using var reader = new StreamReader(stream);

        var comments = new List<string>();
        var models = TsvReader.Read<TestModel>(reader, comments: comments).ToArray();
        Assert.AreEqual(2, comments.Count);
        Assert.AreEqual("comment1", comments[0]);
        Assert.AreEqual("comment2", comments[1]);
        Assert.AreEqual(2, models.Length);
        AssertDataLine(models[0]);
        AssertEmptyLine(models[1]);
    }

    [TestMethod]
    public void ShoulReadByPropertiesWithoutHeader()
    {
        var tsv = new StringBuilder();
        tsv.AppendLine(string.Join('\t', "string", "c", "true", "2022-01-10 10:00:00", "2022-01-10", "10:00:00", "255", "65535", "65536", "65537", "-128", "-32768", "-32769", "-32770", "65540", "1.79E+308", "3.4028E+38", "A"));
        tsv.AppendLine(string.Join('\t', "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));

        var models = TsvReader.Read<TestModel>(tsv.ToString(), header: false).ToArray();
        Assert.AreEqual(2, models.Length);
        AssertDataLine(models[0]);
        AssertEmptyLine(models[1]);
    }

    [TestMethod]
    public void ShouldReadMappedByProperties()
    {
        var tsv = new StringBuilder();
        tsv.AppendLine("#comment");
        tsv.AppendLine(string.Join('\t', "StringValue", "DateValue", "EnumValue", "ArrayValue"));
        tsv.AppendLine(string.Join('\t', "string", "2022-01-10T10:00:00", "A", "[1, 2, 3]"));

        var arrayConverter = new ArrayConverter();

        var map = new ClassMap<TestModel>()
            .Map(x => x.StringValue)
            .Map(x => x.DateValue)
            .Map(x => x.EnumValue)
            .Map(x => x.ArrayValue, converter: arrayConverter);

        var models = TsvReader.Read(tsv.ToString(), map).ToArray();

        Assert.AreEqual(1, models.Length);

        Assert.AreEqual("string", models[0].StringValue);
        Assert.AreEqual(new DateTime(2022, 1, 10, 10, 0, 0), models[0].DateValue);
        Assert.AreEqual(TestEnum.A, models[0].EnumValue);
        AssertArray(new[] { 1, 2, 3 }, models[0].ArrayValue);

        Assert.AreEqual(null, models[0].BoolValue);
        Assert.AreEqual(null, models[0].IntValue);
        Assert.AreEqual(null, models[0].DoubleValue);
    }

    [TestMethod]
    public void ShouldReadMappedByPropertiesWithoutHeader()
    {
        var tsv = new StringBuilder();
        tsv.AppendLine("#comment");
        tsv.AppendLine(string.Join('\t', "string", "2022-01-10T10:00:00", "A", "[1, 2, 3]"));

        var arrayConverter = new ArrayConverter();

        var map = new ClassMap<TestModel>()
            .Map(x => x.StringValue)
            .Map(x => x.DateValue)
            .Map(x => x.EnumValue)
            .Map(x => x.ArrayValue, converter: arrayConverter);

        var models = TsvReader.Read(tsv.ToString(), map, false).ToArray();

        Assert.AreEqual(1, models.Length);

        Assert.AreEqual("string", models[0].StringValue);
        Assert.AreEqual(new DateTime(2022, 1, 10, 10, 0, 0), models[0].DateValue);
        Assert.AreEqual(TestEnum.A, models[0].EnumValue);
        AssertArray(new[] { 1, 2, 3 }, models[0].ArrayValue);

        Assert.AreEqual(null, models[0].BoolValue);
        Assert.AreEqual(null, models[0].IntValue);
        Assert.AreEqual(null, models[0].DoubleValue);
    }

    [TestMethod]
    public void ShouldReadMappedByNames()
    {
        var tsv = new StringBuilder();
        tsv.AppendLine("#comment");
        tsv.AppendLine(string.Join('\t', "String_Value", "Date_Value", "Enum_Value", "Array_Value"));
        tsv.AppendLine(string.Join('\t', "string", "2022-01-10T10:00:00", "A", "[1, 2, 3]"));

        var arrayConverter = new ArrayConverter();

        var map = new ClassMap<TestModel>()
            .Map(x => x.StringValue, "String_Value")
            .Map(x => x.DateValue, "Date_Value")
            .Map(x => x.EnumValue, "Enum_Value")
            .Map(x => x.ArrayValue, "Array_Value", arrayConverter);

        var models = TsvReader.Read(tsv.ToString(), map).ToArray();

        Assert.AreEqual(1, models.Length);

        Assert.AreEqual("string", models[0].StringValue);
        Assert.AreEqual(new DateTime(2022, 1, 10, 10, 0, 0), models[0].DateValue);
        Assert.AreEqual(TestEnum.A, models[0].EnumValue);
        AssertArray(new[] { 1, 2, 3 }, models[0].ArrayValue);

        Assert.AreEqual(null, models[0].BoolValue);
        Assert.AreEqual(null, models[0].IntValue);
        Assert.AreEqual(null, models[0].DoubleValue);
    }

    [TestMethod]
    public void ShouldReadMixedMapping()
    {
        var tsv = new StringBuilder();
        tsv.AppendLine("#comment");
        tsv.AppendLine(string.Join('\t', "String_Value", "CharValue", "BoolValue", "Date_Value", "ByteValue", "UShortValue", "UIntValue", "ULongValue", "SByteValue", "ShortValue", "IntValue", "LongValue", "DecimalValue", "DoubleValue", "FloatValue", "Enum_Value", "ArrayValue"));
        tsv.AppendLine(string.Join('\t', "string", "c", "true", "2022-01-10 10:00:00", "255", "65535", "65536", "65537", "-128", "-32768", "-32769", "-32770", "65540", "1.79E+308", "3.4028E+38", "A", "[1, 2, 3]"));

        var arrayConverter = new ArrayConverter();

        var map = new ClassMap<TestModel>()
            .AutoMap()
            .Map(x => x.StringValue, "String_Value")
            .Map(x => x.DateValue, "Date_Value")
            .Map(x => x.ArrayValue, converter: arrayConverter)
            .Ignore(x => x.EnumValue, "Enum_Value");

        var models = TsvReader.Read(tsv.ToString(), map).ToArray();

        Assert.AreEqual(1, models.Length);
        Assert.AreEqual("string", models[0].StringValue);
        Assert.AreEqual('c', models[0].CharValue);
        Assert.AreEqual(true, models[0].BoolValue);
        Assert.AreEqual(new DateTime(2022, 1, 10, 10, 0, 0), models[0].DateValue);
        Assert.AreEqual((byte)255, models[0].ByteValue);
        Assert.AreEqual((ushort)65535, models[0].UShortValue);
        Assert.AreEqual((uint)65536, models[0].UIntValue);
        Assert.AreEqual((ulong)65537, models[0].ULongValue);
        Assert.AreEqual((sbyte)-128, models[0].SByteValue);
        Assert.AreEqual((short)-32768, models[0].ShortValue);
        Assert.AreEqual((int)-32769, models[0].IntValue);
        Assert.AreEqual((long)-32770, models[0].LongValue);
        Assert.AreEqual((decimal)65540, models[0].DecimalValue);
        Assert.AreEqual((double)1.79E+308, models[0].DoubleValue);
        Assert.AreEqual((float)3.4028E+38, models[0].FloatValue);
        Assert.AreEqual(null, models[0].EnumValue);
        AssertArray(new[] { 1, 2, 3 }, models[0].ArrayValue);
    }


    private static void AssertDataLine(TestModel line)
    {
        Assert.AreEqual("string", line.StringValue);
        Assert.AreEqual('c', line.CharValue);
        Assert.AreEqual(true, line.BoolValue);
        Assert.AreEqual(new DateTime(2022, 1, 10, 10, 0, 0), line.DateValue);
        Assert.AreEqual(new DateOnly(2022, 1, 10), line.DateOnlyValue);
        Assert.AreEqual(new TimeOnly(10, 0, 0), line.TimeOnlyValue);
        Assert.AreEqual((byte)255, line.ByteValue);
        Assert.AreEqual((ushort)65535, line.UShortValue);
        Assert.AreEqual((uint)65536, line.UIntValue);
        Assert.AreEqual((ulong)65537, line.ULongValue);
        Assert.AreEqual((sbyte)-128, line.SByteValue);
        Assert.AreEqual((short)-32768, line.ShortValue);
        Assert.AreEqual((int)-32769, line.IntValue);
        Assert.AreEqual((long)-32770, line.LongValue);
        Assert.AreEqual((decimal)65540, line.DecimalValue);
        Assert.AreEqual((double)1.79E+308, line.DoubleValue);
        Assert.AreEqual((float)3.4028E+38, line.FloatValue);
        Assert.AreEqual(TestEnum.A, line.EnumValue);
    }

    private static void AssertEmptyLine(TestModel line)
    {
        Assert.AreEqual(null, line.StringValue);
        Assert.AreEqual(null, line.CharValue);
        Assert.AreEqual(null, line.BoolValue);
        Assert.AreEqual(null, line.DateValue);
        Assert.AreEqual(null, line.DateOnlyValue);
        Assert.AreEqual(null, line.TimeOnlyValue);
        Assert.AreEqual(null, line.ByteValue);
        Assert.AreEqual(null, line.UShortValue);
        Assert.AreEqual(null, line.UIntValue);
        Assert.AreEqual(null, line.ULongValue);
        Assert.AreEqual(null, line.SByteValue);
        Assert.AreEqual(null, line.ShortValue);
        Assert.AreEqual(null, line.IntValue);
        Assert.AreEqual(null, line.LongValue);
        Assert.AreEqual(null, line.DecimalValue);
        Assert.AreEqual(null, line.DoubleValue);
        Assert.AreEqual(null, line.FloatValue);
        Assert.AreEqual(null, line.EnumValue);
    }

    private static void AssertArray<T>(T[] expected, T[] actual)
    {
        Assert.AreEqual(expected.Length, actual.Length);

        for (int i = 0; i < expected.Length; i++)
        {
            Assert.AreEqual(expected[i], actual[i]);
        }
    }

    private static Stream PrepareStream(string content)
    {
        var stream = new MemoryStream();
        
        using (var writer = new StreamWriter(stream, leaveOpen: true))
        {
            writer.Write(content);
        }

        stream.Position = 0;

        return stream;
    }
}
