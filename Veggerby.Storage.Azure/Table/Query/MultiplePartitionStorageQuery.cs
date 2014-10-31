using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace Veggerby.Storage.Azure.Table.Query
{
    public class MultiplePartitionStorageQuery<T> : MultiplePartStorageQuery<string, T> where T : ITableEntity, new()
    {
        public MultiplePartitionStorageQuery(IEnumerable<string> keys)
            : base(keys)
        {
        }

        protected override string GetPart(string key)
        {
            return TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, key);
        }
    }
}