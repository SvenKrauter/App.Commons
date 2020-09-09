using System;
using System.Diagnostics.CodeAnalysis;

namespace App.Commons.Extensions
{
    public static class ObjectExtensions
    {
        [return: NotNull]
        public static Enum ToEnum<TEnum>([NotNull] this object enumeration) 
            where TEnum : struct, Enum => enumeration.ToEnum(typeof(TEnum));

        public static object? ParseEnum(this object? value, Type enumType,
            bool ignoreCase = true)
        {
            if (!(value is string str)) return null;

            Enum.TryParse(enumType, str, ignoreCase, out var result);

            return result;
        }

        [return: NotNull]
        public static Enum ToEnum([NotNull] this object @enum, Type enumType)
        {
            _ = @enum ?? throw new ArgumentNullException(nameof(@enum));

            var type = @enum!.GetType();
            type.IsEnum.ThrowIfFalse(nameof(type.IsEnum));

            return (Enum)Enum.Parse(enumType, @enum.ToString()!);
        }
    }
}