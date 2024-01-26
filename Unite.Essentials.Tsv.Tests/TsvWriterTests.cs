using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unite.Essentials.Tsv.Tests.Converters;
using Unite.Essentials.Tsv.Tests.Models;
using Unite.Essentials.Tsv.Tests.Models.Enums;

namespace Unite.Essentials.Tsv.Tests;

[TestClass]
public class TsvWriterTests
{
    private static TestModelAttributed[] _models;

    [ClassInitialize]
    public static void InitializeTests(TestContext context)
    {
        _models = new TestModelAttributed[]
        {
            new TestModelAttributed()
            {
                StringValue = "string",
                CharValue = 'c',
                BoolValue = true,
                DateValue = new DateTime(2022, 1, 10, 10, 0, 0),
                DateOnlyValue = new DateOnly(2022, 1, 10),
                TimeOnlyValue = new TimeOnly(10, 0, 0),
                ByteValue = 255,
                UShortValue = 65535,
                UIntValue = 65536,
                ULongValue = 65537,
                SByteValue = -128,
                ShortValue = -32768,
                IntValue = -32769,
                LongValue = -32770,
                DecimalValue = 65540,
                DoubleValue = 1.79E+308,
                FloatValue = 3.4028E+38f,
                EnumValue = TestEnum.A,
                ArrayValue = new[] { 1, 2, 3 }
            },
            new TestModelAttributed()
            {

            }
        };
    }


    [TestMethod]
    public void ShouldWriteByAttributes()
    {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream, leaveOpen: true) { AutoFlush = true };
        TsvWriter.Write<TestModelAttributed>(writer, _models);
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        var tsv = reader.ReadToEnd().Split('\n');

        Assert.AreEqual(TestModelAttributed.TsvHeader(), tsv[0]);
        Assert.AreEqual(string.Join('\t', "string", "c", "True", "01/10/2022 10:00:00", "01/10/2022", "10:00", "255", "65535", "65536", "65537", "-128", "-32768", "-32769", "-32770", "65540", "1.79E+308", "3.4028E+38", "A", "[1,2,3]"), tsv[1]);
        Assert.AreEqual(string.Join('\t', "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""), tsv[2]);
    }

    [TestMethod]
    public void ShouldWriteByAttributesWithoutHeader()
    {
        var tsv = TsvWriter.Write<TestModelAttributed>(_models, header: false).Split('\n');

        Assert.AreEqual(string.Join('\t', "string", "c", "True", "01/10/2022 10:00:00", "01/10/2022", "10:00", "255", "65535", "65536", "65537", "-128", "-32768", "-32769", "-32770", "65540", "1.79E+308", "3.4028E+38", "A", "[1,2,3]"), tsv[0]);
        Assert.AreEqual(string.Join('\t', "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""), tsv[1]);
    }

    [TestMethod]
    public void ShouldWriteByProperties()
    {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream, leaveOpen: true) { AutoFlush = true };
        TsvWriter.Write<TestModel>(writer, _models);
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        var tsv = reader.ReadToEnd().Split('\n');

        Assert.AreEqual(string.Join('\t', "StringValue", "CharValue", "BoolValue", "DateValue", "DateOnlyValue", "TimeOnlyValue", "ByteValue", "UShortValue", "UIntValue", "ULongValue", "SByteValue", "ShortValue", "IntValue", "LongValue", "DecimalValue", "DoubleValue", "FloatValue", "EnumValue", "ArrayValue"), tsv[0]);
        Assert.AreEqual(string.Join('\t', "string", "c", "True", "01/10/2022 10:00:00", "01/10/2022", "10:00", "255", "65535", "65536", "65537", "-128", "-32768", "-32769", "-32770", "65540", "1.79E+308", "3.4028E+38", "A", ""), tsv[1]);
        Assert.AreEqual(string.Join('\t', "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""), tsv[2]);
    }

    [TestMethod]
    public void ShouldWriteByPropertiesWithoutHeader()
    {
        var tsv = TsvWriter.Write<TestModel>(_models, header: false).Split('\n');

        Assert.AreEqual(string.Join('\t', "string", "c", "True", "01/10/2022 10:00:00", "01/10/2022", "10:00", "255", "65535", "65536", "65537", "-128", "-32768", "-32769", "-32770", "65540", "1.79E+308", "3.4028E+38", "A", ""), tsv[0]);
        Assert.AreEqual(string.Join('\t', "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""), tsv[1]);
    }

    [TestMethod]
    public void ShouldWriteMappedByProperties()
    {
        var arrayConverter = new ArrayConverter();
        
        var map = new ClassMap<TestModel>()
            .Map(x => x.StringValue)
            .Map(x => x.CharValue)
            .Map(x => x.BoolValue)
            .Map(x => x.DateValue)
            .Map(x => x.ArrayValue, converter: arrayConverter);

        var tsv = TsvWriter.Write<TestModel>(_models, map).Split('\n');

        Assert.AreEqual(string.Join('\t', "StringValue", "CharValue", "BoolValue", "DateValue", "ArrayValue"), tsv[0]);
        Assert.AreEqual(string.Join('\t', "string", "c", "True", "01/10/2022 10:00:00", "[1,2,3]"), tsv[1]);
        Assert.AreEqual(string.Join('\t', "", "", "", "", ""), tsv[2]);
    }

    [TestMethod]
    public void ShouldWriteMappedByPropertiesWithoutHeader()
    {
        var arrayConverter = new ArrayConverter();
        
        var map = new ClassMap<TestModel>()
            .Map(x => x.StringValue)
            .Map(x => x.CharValue)
            .Map(x => x.BoolValue)
            .Map(x => x.DateValue)
            .Map(x => x.ArrayValue, converter: arrayConverter);

        var tsv = TsvWriter.Write<TestModel>(_models, map, false).Split('\n');

        Assert.AreEqual(string.Join('\t', "string", "c", "True", "01/10/2022 10:00:00", "[1,2,3]"), tsv[0]);
        Assert.AreEqual(string.Join('\t', "", "", "", "", ""), tsv[1]);
    }

    [TestMethod]
    public void ShouldWriteMappedByNames()
    {
        var arrayConverter = new ArrayConverter();
        
        var map = new ClassMap<TestModel>()
            .Map(x => x.StringValue, "string_value")
            .Map(x => x.CharValue, "char_value")
            .Map(x => x.BoolValue, "bool_value")
            .Map(x => x.DateValue, "date_value")
            .Map(x => x.ArrayValue, "array_value", arrayConverter);

        var tsv = TsvWriter.Write<TestModel>(_models, map).Split('\n');

        Assert.AreEqual(string.Join('\t', "string_value", "char_value", "bool_value", "date_value", "array_value"), tsv[0]);
        Assert.AreEqual(string.Join('\t', "string", "c", "True", "01/10/2022 10:00:00", "[1,2,3]"), tsv[1]);
        Assert.AreEqual(string.Join('\t', "", "", "", "", ""), tsv[2]);
    }

    [TestMethod]
    public void ShouldWriteMixedMapping()
    {
        var arrayConverter = new ArrayConverter();

        var map = new ClassMap<TestModel>()
            .AutoMap()
            .Map(x => x.StringValue, "string_value")
            .Map(x => x.CharValue, "char_value")
            .Map(x => x.BoolValue, "bool_value")
            .Map(x => x.DateValue, "date_value")
            .Ignore(x => x.EnumValue)
            .Map(x => x.ArrayValue, "array_value", arrayConverter);
            

        var tsv = TsvWriter.Write<TestModel>(_models, map).Split('\n');

        Assert.AreEqual(string.Join('\t', "string_value", "char_value", "bool_value", "date_value", "DateOnlyValue", "TimeOnlyValue", "ByteValue", "UShortValue", "UIntValue", "ULongValue", "SByteValue", "ShortValue", "IntValue", "LongValue", "DecimalValue", "DoubleValue", "FloatValue", "array_value"), tsv[0]);
        Assert.AreEqual(string.Join('\t', "string", "c", "True", "01/10/2022 10:00:00", "01/10/2022", "10:00", "255", "65535", "65536", "65537", "-128", "-32768", "-32769", "-32770", "65540", "1.79E+308", "3.4028E+38", "[1,2,3]"), tsv[1]);
        Assert.AreEqual(string.Join('\t', "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""), tsv[2]);
    }

    [TestMethod]
    public void ShouldWriteNestedModel()
    {
        var models = new ParentModel[]
        {
            new ParentModel
            {
                StringValue = "Parent-1",
                IntValue = 1,
                ChildModel = new ChildModel
                {
                    StringValue = "Child-1",
                    IntValue = 1
                }
            },
            new ParentModel
            {
                StringValue = "Parent-2",
                IntValue = 2,
                ChildModel = null
            }
        };

        var map = new ClassMap<ParentModel>()
            .Map(x => x.StringValue)
            .Map(x => x.IntValue)
            .Nest(x => x.ChildModel)
            .Map(x => x.ChildModel.StringValue, "ChildModel.StringValue")
            .Map(x => x.ChildModel.IntValue, "ChildModel.IntValue");

        var tsv = TsvWriter.Write<ParentModel>(models, map).Split('\n');

        Assert.AreEqual(string.Join('\t', "StringValue", "IntValue", "ChildModel.StringValue", "ChildModel.IntValue"), tsv[0]);
        Assert.AreEqual(string.Join('\t', "Parent-1", "1", "Child-1", "1"), tsv[1]);
        Assert.AreEqual(string.Join('\t', "Parent-2", "2", "", ""), tsv[2]);
    }
}
