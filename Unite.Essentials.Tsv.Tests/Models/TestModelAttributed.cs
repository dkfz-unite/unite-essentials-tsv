using System;
using Unite.Essentials.Tsv.Attributes;
using Unite.Essentials.Tsv.Tests.Models.Enums;

namespace Unite.Essentials.Tsv.Tests.Models
{
	public class TestModelAttributed
	{
		public const string StringColumn = "string_value";
		public const string IntColumn = "int_value";
		public const string DoubleColumn = "double_value";
		public const string BoolColumn = "bool_value";
		public const string DateColumn = "date_value";
		public const string EnumColumn = "enum_value";

		[Column(StringColumn)]
		public string? StringValue { get; set; }

		[Column(IntColumn)]
		public int IntValue { get; set; }

		[Column(DoubleColumn)]
		public double DoubleValue { get; set; }

		[Column(BoolColumn)]
		public bool BoolValue { get; set; }

		[Column(DateColumn)]
		public DateTime DateValue { get; set; }

		[Column(EnumColumn)]
		public TestEnum EnumValue { get; set; }

		public string? ShadowValue { get; set; }

		public static string TsvHeader()
        {
			return string.Join('\t', StringColumn, IntColumn, DoubleColumn, BoolColumn, DateColumn, EnumColumn);
        }
	}
}
