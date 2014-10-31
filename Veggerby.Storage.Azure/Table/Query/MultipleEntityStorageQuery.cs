using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace Veggerby.Storage.Azure.Table.Query
{
    public class MultipleEntityStorageQuery<T> : MultiplePartStorageQuery<TableEntityKey, T> where T : ITableEntity, new()
    {
        public MultipleEntityStorageQuery(IEnumerable<TableEntityKey> keys)
            : base(keys)
        {
        }

        protected override string GetPart(TableEntityKey key)
        {
            return TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, key.PartitionKey),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, key.RowKey));
        }
    }
}