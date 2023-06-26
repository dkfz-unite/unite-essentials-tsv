using System.Reflection;
using System.Runtime.Serialization;

namespace Unite.Essentials.Tsv.Converters;

internal class EnumMemberConverter
{
    internal static T Convert<T>(string value) where T : Enum
    {
        var type = typeof(T);

        var member = FindMember(value, type);

        return member != null
            ? (T)Enum.Parse(typeof(T), member.Name)
            : (T)Enum.Parse(typeof(T), value);
    }

    internal static object Convert(string value, Type type)
    {
        var member = FindMember(value, type);

        return member != null
            ? Enum.Parse(type, member.Name)
            : Enum.Parse(type, value);
    }

    internal static string Convert(object value, Type type)
    {
        var member = FindMember(value.ToString(), type);

        return member != null
            ? member.GetCustomAttribute<EnumMemberAttribute>().Value
            : value.ToString();
    }

    internal static FieldInfo FindMember(string value, Type type)
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
