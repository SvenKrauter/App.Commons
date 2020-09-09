using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace App.Commons.Extensions
{
    [DebuggerStepThrough]
    public static class MemberInfoExtensions
    {
        public static string? GetDisplayName([NotNull] this MemberInfo source, bool returnMemberNameIfNull = true)
        {
            var memberInfo = source;
            while (memberInfo != null)
            {
                var displayName = memberInfo.TryGetAttribute<DisplayNameAttribute>(false)?.DisplayName ??
                                  // ReSharper disable once RedundantArgumentDefaultValue
                                  memberInfo.TryGetAttribute<DisplayAttribute>(true)?.Name ??
                                  (returnMemberNameIfNull ? source.Name : null);

                if (displayName != null)
                    return displayName;

                break;
            }

            return returnMemberNameIfNull ? source.Name : null;
        }

        public static TAttribute? TryGetAttribute<TAttribute>([NotNull] this MemberInfo source, bool inherit = true)
            where TAttribute : Attribute
        {
            var memberInfo = source;
            while (memberInfo != null)
            {
                var attribute = memberInfo.GetCustomAttribute<TAttribute>(inherit);
                if (attribute != null)
                    return attribute;

                break;
            }

            return null;
        }

        public static bool TryGetIsAttributeIsDefined<TAttribute>([NotNull] this MemberInfo source, bool inherit = true)
            where TAttribute : Attribute
        {
            var memberInfo = source;
            while (memberInfo != null)
            {
                var isDefined = memberInfo.IsDefined<TAttribute>(inherit);
                if (isDefined)
                    return true;

                break;
            }

            return false;
        }

        public static bool IsDefined<T>([NotNull] this MemberInfo source, bool inherit = true) where T : Attribute => Attribute.IsDefined(source, typeof(T), inherit);

        public static int GetMaxLength([NotNull] this MemberInfo source) => source.TryGetAttribute<StringLengthAttribute>()?.MaximumLength ?? 0;

        public static bool GetIsRequired([NotNull] this MemberInfo source) => source.TryGetIsAttributeIsDefined<RequiredAttribute>();

        public static T GetCustomAttributeOrNew<T>([NotNull] this MemberInfo source) where T : Attribute, new() => (T)source.GetCustomAttribute(typeof(T))! ?? new T();

        public static (T Min, T Max) GetRange<T>([NotNull] this MemberInfo source) where T : struct => ((T Min, T Max))source.GetRange();

        public static (object Min, object Max) GetRange([NotNull] this MemberInfo source)
        {
            var range = source.TryGetAttribute<RangeAttribute>();
            return range is null
                ? (0, 0)
                : (range.Minimum, range.Maximum);
        }

        public static Type? GetUnderlyingType(this MemberInfo member) =>
            member.MemberType switch
            {
                MemberTypes.Event => ((EventInfo)member).EventHandlerType,
                MemberTypes.Field => ((FieldInfo)member).FieldType,
                MemberTypes.Method => ((MethodInfo)member).ReturnType,
                MemberTypes.Property => ((PropertyInfo)member).PropertyType,
                _ => throw new ArgumentException("Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo")
            };
    }
}
