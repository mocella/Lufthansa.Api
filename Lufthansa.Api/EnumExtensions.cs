using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Lufthansa.Api;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()
            ?.Name ?? enumValue.ToString();
    }

    public static TEnum ParseFromChar<TEnum>(char value) where TEnum : Enum
    {
        return (TEnum)Enum.ToObject(typeof(TEnum), value);
    }

    public static TEnum? ParseFromString<TEnum>(string displayName) where TEnum : Enum
    {
        foreach (var field in typeof(TEnum).GetFields())
            if (Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) is DisplayAttribute attribute)
            {
                if (attribute.Name == displayName) return (TEnum)field.GetValue(null)!;
            }
            else if (field.Name == displayName)
            {
                return (TEnum)field.GetValue(null)!;
            }

        throw new ArgumentException($"'{displayName}' is not a valid display name for {typeof(TEnum)}");
    }
}