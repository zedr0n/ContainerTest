using System;
using System.Reflection;

namespace Lib
{
    public static class Reflection
    {
        public static bool Is<T>(this Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }
        public static T GetCustomAttribute<T>(this Type type) where T : Attribute
        {
            return type.GetTypeInfo().GetCustomAttribute(typeof(T)) as T;
        }

    }
}
