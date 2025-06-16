using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace EShopFanerum.Avalonia.ManagerApp.Extensions;

public static class EnumExtension
{
    public static string GetDescription<TEnum>(this TEnum element)
        where TEnum : struct, Enum
    {
        return GetDescription((Enum)element);
    }

    private static string GetDescription(this Enum element, bool emptyResult = false)
    {
        if (element == null)
            throw new ArgumentNullException(nameof(element));

        var type = element.GetType();

        var memberInfo = type.GetMember(element.ToString());

        if (memberInfo.Length > 0)
        {
            var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return ((DescriptionAttribute)attributes[0]).Description;
            }

            attributes = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);

            if (attributes.Length > 0)
            {
                return ((DisplayAttribute)attributes[0]).GetName()!;
            }

            attributes = memberInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false);

            if (attributes.Length > 0)
            {
                return ((EnumMemberAttribute)attributes[0]).Value!;
            }
        }

        return (emptyResult ? null : element.ToString())!;
    }
}