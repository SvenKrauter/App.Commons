using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Text;

namespace App.Commons.Extensions
{
    public static class StringExtensions
    {
        public static string Repeat(this string input, int count)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var builder = new StringBuilder(input.Length * count);

            for (var i = 0; i < count; i++)
                builder.Append(input);

            return builder.ToString();
        }

        public static T ParseEnum<T>(this string text, bool ignoreCase = true) where T : struct, Enum
        {
            Enum.TryParse<T>(text, ignoreCase, out var result);

            return result;
        }

        public static Enum ParseEnum(this string text, Type enumType) 
        {
            Enum.TryParse(enumType, text, true, out var result);

            return (Enum)result!;
        }

        [Pure]
        public static bool IsNullOrEmpty([NotNullWhen(false)] this string? str) => string.IsNullOrEmpty(str);

        [Pure]
        public static bool IsNotNullOrEmpty([NotNullWhen(false)] this string? str) => !str.IsNullOrEmpty();

        public static string InsertArgs(this string str, params object?[] args)
        {
            if (str.IsNullOrEmpty()) return str;
            if (args.Length == 0) return str;
            if (!str.Contains("}") || !str.Contains("{")) return str;

            return string.Format(str, args);
        }
    }
}
