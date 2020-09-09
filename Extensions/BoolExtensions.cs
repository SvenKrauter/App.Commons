using System.Diagnostics.CodeAnalysis;
using App.Commons.Helpers;

namespace App.Commons.Extensions
{
    public static class BoolExtensions
    {
        public static void ThrowIfFalse([DoesNotReturnIf(false)] this bool source, string argName, string? customErrorText = null)
            => Requires.NotFalse(source, argName, customErrorText);

        public static void ThrowIfTrue([DoesNotReturnIf(true)] this bool source, string argName, string? customErrorText = null)
            => Requires.NotTrue(source, argName, customErrorText);
    }
}