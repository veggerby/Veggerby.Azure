using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace Veggerby.Storage.Azure.Table.Query
{
    public class MultipleRowKeysStorageQuery<T> : MultiplePartStorageQuery<string, T> where T : ITableEntity, new()
    {
        public MultipleRowKeysStorageQuery(IEnumerable<string> keys)
            : base(keys)
        {
        }

        protected override string GetPart(string key)
        {
            return "RowKey".Equal(key);
        }
    }
}