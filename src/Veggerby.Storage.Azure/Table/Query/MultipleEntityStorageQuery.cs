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
            return "PartitionKey".Equal(key.PartitionKey).And("RowKey".Equal(key.RowKey));
        }
    }
}