using Microsoft.WindowsAzure.Storage.Table;

namespace Veggerby.Storage.Azure.Table.Query
{
    public class ListPartitionStorageQuery<T> : StorageQuery<T> where T : ITableEntity, new()
    {
        public ListPartitionStorageQuery(string partitionKey)
        {
            Query = Query.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
        }
    }
}
