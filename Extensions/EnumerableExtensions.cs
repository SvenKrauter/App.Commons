using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace App.Commons.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsEmpty<T>([NotNullWhen(false)] this IEnumerable<T> source) => source == null || !source.Any();

        [Pure]
        public static bool IsNotEmpty([NotNullWhen(true)] this IEnumerable source) => source != null && source.Cast<object>().Any();
    }
}