using System;
using System.ComponentModel;
using System.Linq;

namespace Veggerby.Storage.Azure.Table
{
    public static class Extensions
    {
        public static string GetDisplayName(this Type assemblyType)
        {
            var attribute = assemblyType.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault() as DisplayNameAttribute;
            var displayName = attribute != null ? attribute.DisplayName : assemblyType.Name;
            return displayName;
        }

        public static string GetTableName(this Type type)
        {
            if (type.Name.EndsWith("Entity"))
            {
                return type.Name.Substring(0, type.Name.Length - 6).ToLowerInvariant();
            }

            return type.Name.ToLowerInvariant();
        }
    }
}
