using System;
using System.Reflection;

namespace Storage.Common.Extensions
{
    public static class CustomAttributeExtensions
    {
        public static T ForceCustomAttribute<T>(this Type type) where T : Attribute
        {
            var customAttribute = type.GetCustomAttribute<T>();
            var attributeType = typeof (T);
            if (customAttribute == null)
            {
                throw new ArgumentException($"CustomAttribute '{attributeType.Name}' not used with Type '{type.Name}'");
            }
            return customAttribute;
        }
    }
}