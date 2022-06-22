using System;
using Unite.Essentials.Tsv.Tests.Models.Enums;

namespace Unite.Essentials.Tsv.Tests.Models
{
	public class TestModel
	{
		public string StringValue { get; set; }
		public int? IntValue { get; set; }
		public double? DoubleValue { get; set; }
		public bool? BoolValue { get; set; }
		public DateTime? DateValue { get; set; }
		public TestEnum? EnumValue { get; set; }
	}
}
