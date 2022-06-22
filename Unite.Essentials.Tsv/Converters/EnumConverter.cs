using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Unite.Essentials.Tsv.Converters
{
	internal class EnumConverter
	{
		internal static T Parse<T>(string value) where T : Enum
		{
			var type = typeof(T);

			var member = FindMember(type, value);

			return member != null
				? (T)Enum.Parse(typeof(T), member.Name)
				: (T)Enum.Parse(typeof(T), value);
		}

		internal static object Parse(Type type, string value)
		{
			var member = FindMember(type, value);

			return member != null
				? Enum.Parse(type, member.Name)
				: Enum.Parse(type, value);
		}

		internal static FieldInfo FindMember(Type type, string value)
		{
			var members = type.GetFields();

			foreach (var member in members)
			{
				var memberAttribute = member.GetCustomAttribute<EnumMemberAttribute>();

				if (memberAttribute != null && string.Equals(memberAttribute.Value, value, StringComparison.InvariantCultureIgnoreCase))
				{
					return member;
				}
			}

			return null;
		}
	}
}
