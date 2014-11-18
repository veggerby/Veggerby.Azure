using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

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
            return type.Name.EndsWith("Entity") 
                ? type.Name.Substring(0, type.Name.Length - 6).ToLowerInvariant() 
                : type.Name.ToLowerInvariant();
        }

        public static TableEntityKey ToTableEntityKey(this ITableEntity entity)
        {
            return entity != null
                ? new TableEntityKey(entity.PartitionKey, entity.RowKey)
                : null;
        }

        public static IEnumerable<TableEntityKey> ToTableEntityKeys(this IEnumerable<ITableEntity> entities)
        {
            return entities.Select(x => x.ToTableEntityKey()).ToList();
        }
    }
}