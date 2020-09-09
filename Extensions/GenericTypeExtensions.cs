using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using App.Commons.Helpers;

namespace App.Commons.Extensions
{
    [SuppressMessage("Stil", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "<Ausstehend>")]
    [DebuggerStepThrough]
    public static class GenericTypeExtensions
    {
        #region Throw

#pragma warning disable CS8777 // Der Parameter muss beim Beenden einen Wert ungleich NULL aufweisen.
        public static void ThrowIfNull<T>([NotNull] this T source, string argName, string? customErrorText = null) => Requires.NotNull(source, argName, customErrorText);
#pragma warning restore CS8777 // Der Parameter muss beim Beenden einen Wert ungleich NULL aufweisen.

        // ReSharper disable UnusedTypeParameter
        public struct StructTypeRequirement<T> where T : struct { }
        public struct ClassTypeRequirement<T> where T : class { }
        // ReSharper restore UnusedTypeParameter

        [return: NotNullIfNotNull("source")]
        public static T GetOrThrowIfDefault<T>([NotNull, NotNullIfNotNull("source")] this T source, string argName, string? customErrorText = null) where T : struct
        {
            if (Equals(source, default(T)))
                throw ExceptionsHelper.ArgumentDefault(argName, customErrorText);

            return source;
        }

        [return: NotNullIfNotNull("source")]
        public static T? GetOrThrowIfNull<T>([NotNull, NotNullIfNotNull("source")] this T? source, string argName, string? customErrorText = null, StructTypeRequirement<T> _ = default) where T : struct => source ?? throw ExceptionsHelper.ArgumentNull(argName, customErrorText);

        [return: NotNullIfNotNull("source")]
        public static T GetOrThrowIfNull<T>([NotNull, NotNullIfNotNull("source")] this T? source, string argName, string? customErrorText = null, ClassTypeRequirement<T> _ = default) where T : class => source ?? throw ExceptionsHelper.ArgumentNull(argName, customErrorText);

        public static void ThrowIfNegative<T>(this T source, string argName, string? customErrorText = null) where T : struct, IConvertible, IComparable
            => Requires.NotNegative(source, argName, customErrorText);

        public static void ThrowIfPositive<T>(this T source, string argName, string? customErrorText = null) where T : struct, IConvertible, IComparable
            => Requires.NotPositive(source, argName, customErrorText);

        public static void ThrowIfDefault<T>(this T source, string argName, string? customErrorText = null)
            => Requires.NotDefault(source, argName, customErrorText);

        public static void ThrowIfNotDefault<T>(this T source, string argName, string? customErrorText = null)
            => Requires.Default(source, argName, customErrorText);

        #endregion

        #region Casting, Conversion

        [return: NotNull]
        public static T[] AsArray<T>(this T obj, params T[] args) where T : notnull => obj is null ? Array.Empty<T>() : args.Append(obj).ToArray();

        [return: NotNull]
        public static List<T> AsList<T>(this T obj, params T[] args) where T : notnull
        {
            var list = new List<T>();

            if (obj is null) return list;

            list.Add(obj);

            if (args?.Length > 0)
                list.AddRange(args);

            return list;
        }

        #endregion

        #region Cloning, Copying

        public static T? Clone<T>(this T source) where T : class, ICloneable => (T)source?.Clone()!;

        #endregion

        #region Attributes

        public static int GetMaxLength<T>(this T source, Expression<Func<T, string?>> propertyExpression) where T : class
            => AttributeHelper.GetMaxLength(propertyExpression);

        public static (int MinLength, int MaxLength) GetStringLength<T, TProp>(this T source, Expression<Func<T, TProp>> propertyExpression) where T : class
            => AttributeHelper.GetStringLength(propertyExpression);

        public static (int Min, int Max) GetRange<T, TProp>(this T source, Expression<Func<T, TProp>> propertyExpression) where T : class
            => AttributeHelper.GetRange(propertyExpression);

        public static string? GetPropertyDisplayText<T>(this T source, Expression<Func<T, object?>> propertyExpression) =>
            typeof(T).GetPropertyDisplayText(propertyExpression);

        #endregion
    }
}