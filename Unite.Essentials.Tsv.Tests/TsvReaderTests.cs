using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unite.Essentials.Tsv.Tests.Models;
using Unite.Essentials.Tsv.Tests.Models.Enums;

namespace Unite.Essentials.Tsv.Tests;

[TestClass]
public class TsvReaderTests
{
	[TestMethod]
	public void ShoulReadToModelByProperties()
	{
		var tsv = new StringBuilder();
		tsv.AppendLine(string.Join('\t', "StringValue", "IntValue", "DoubleValue", "BoolValue", "DateValue", "EnumValue"));
		tsv.AppendLine(string.Join('\t', "Row-1", "100", "100.001", "True", "2022-01-10T10:00:00", "A"));
		tsv.AppendLine(string.Join('\t', "", "", "", "", "", ""));

		using var stream = new MemoryStream();
		using var streamWriter = new StreamWriter(stream);
		streamWriter.Write(tsv.ToString());
		streamWriter.Flush();
		stream.Seek(0, SeekOrigin.Begin);

		var models = TsvReader.Read<TestModel>(stream).ToArray();
		Assert.AreEqual(2, models.Length);

		Assert.AreEqual("Row-1", models[0].StringValue);
		Assert.AreEqual(100, models[0].IntValue);
		Assert.AreEqual(100.001, models[0].DoubleValue);
		Assert.AreEqual(true, models[0].BoolValue);
		Assert.AreEqual(new DateTime(2022, 1, 10, 10, 0, 0), models[0].DateValue);
		Assert.AreEqual(TestEnum.A, models[0].EnumValue);

		Assert.AreEqual(null, models[1].StringValue);
		Assert.AreEqual(null, models[1].IntValue);
		Assert.AreEqual(null, models[1].DoubleValue);
		Assert.AreEqual(null, models[1].BoolValue);
		Assert.AreEqual(null, models[1].DateValue);
		Assert.AreEqual(null, models[1].EnumValue);

	}

	[TestMethod]
	public void ShoulReadToModelByAttributes()
	{
		var tsv = new StringBuilder();
		tsv.AppendLine(TestModelAttributed.TsvHeader());
		tsv.AppendLine(string.Join('\t', "Row-1", "100", "100.001", "True", "2022-01-10T10:00:00", "A"));

		using var stream = new MemoryStream();
		using var streamWriter = new StreamWriter(stream);
		streamWriter.Write(tsv.ToString());
		streamWriter.Flush();
		stream.Seek(0, SeekOrigin.Begin);

		var models = TsvReader.Read<TestModelAttributed>(stream).ToArray();
		Assert.AreEqual(1, models.Length);

		Assert.AreEqual("Row-1", models[0].StringValue);
		Assert.AreEqual(100, models[0].IntValue);
		Assert.AreEqual(100.001, models[0].DoubleValue);
		Assert.AreEqual(true, models[0].BoolValue);
		Assert.AreEqual(new DateTime(2022, 1, 10, 10, 0, 0), models[0].DateValue);
		Assert.AreEqual(TestEnum.A, models[0].EnumValue);
		Assert.AreEqual(null, models[0].ShadowValue);
	}

	[TestMethod]
	public void ShouldReadToModelMappedByProperties()
	{
		var tsv = new StringBuilder();
		tsv.AppendLine(string.Join('\t', "StringValue", "DateValue", "EnumValue"));
		tsv.AppendLine(string.Join('\t', "Row-1", "2022-01-10T10:00:00", "A"));

		using var stream = new MemoryStream();
		using var streamWriter = new StreamWriter(stream);
		streamWriter.Write(tsv.ToString());
		streamWriter.Flush();
		stream.Seek(0, SeekOrigin.Begin);

		var map = new ClassMap<TestModel>()
			.Map(x => x.StringValue)
			.Map(x => x.DateValue)
			.Map(x => x.EnumValue);

		var models = TsvReader.Read(stream, map).ToArray();

		Assert.AreEqual(1, models.Length);

		Assert.AreEqual("Row-1", models[0].StringValue);
		Assert.AreEqual(null, models[0].IntValue);
		Assert.AreEqual(null, models[0].DoubleValue);
		Assert.AreEqual(null, models[0].BoolValue);
		Assert.AreEqual(new DateTime(2022, 1, 10, 10, 0, 0), models[0].DateValue);
		Assert.AreEqual(TestEnum.A, models[0].EnumValue);
	}

	[TestMethod]
	public void ShouldReadToModelMappedByNames()
	{
		var tsv = new StringBuilder();
		tsv.AppendLine(string.Join('\t', "String_Value", "Date_Value", "Enum_Value"));
		tsv.AppendLine(string.Join('\t', "Row-1", "2022-01-10T10:00:00", "A"));

		using var stream = new MemoryStream();
		using var streamWriter = new StreamWriter(stream);
		streamWriter.Write(tsv.ToString());
		streamWriter.Flush();
		stream.Seek(0, SeekOrigin.Begin);

		var map = new ClassMap<TestModel>()
			.Map(x => x.StringValue, "String_Value")
			.Map(x => x.DateValue, "Date_Value")
			.Map(x => x.EnumValue, "Enum_Value");

		var models = TsvReader.Read(stream, map).ToArray();

		Assert.AreEqual(1, models.Length);

		Assert.AreEqual("Row-1", models[0].StringValue);
		Assert.AreEqual(null, models[0].IntValue);
		Assert.AreEqual(null, models[0].DoubleValue);
		Assert.AreEqual(null, models[0].BoolValue);
		Assert.AreEqual(new DateTime(2022, 1, 10, 10, 0, 0), models[0].DateValue);
		Assert.AreEqual(TestEnum.A, models[0].EnumValue);
	}
}
