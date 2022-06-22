using System.Runtime.Serialization;

namespace Unite.Essentials.Tsv.Tests.Models.Enums
{
	public enum TestEnum
	{
		[EnumMember(Value = "A")]
		A = 1,

		[EnumMember(Value = "B")]
		B = 2
	}
}
