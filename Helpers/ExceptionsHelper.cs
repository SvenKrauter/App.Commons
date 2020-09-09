using System;
// ReSharper disable InconsistentNaming

namespace App.Commons.Helpers
{
    internal static class ExceptionsHelper
    {
        public static Exception ArgumentNull(string argName, string? customErrorText = null)
        {
            var newMessage = customErrorText ?? Requires.Strings.ArgumentMustNotBeNull;

            return new ArgumentNullException(argName, newMessage);
        }

        public static Exception ArgumentDefault(string argName, string? customErrorText = null)
        {
            var newMessage = customErrorText ?? Requires.Strings.ArgumentMustNotBeDefault;

            return new ArgumentException(newMessage, argName);
        }

        public static Exception InvalidOperation(string message) => new InvalidOperationException(message);
    }
}